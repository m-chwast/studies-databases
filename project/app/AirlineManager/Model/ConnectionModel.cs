using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using ReactiveUI;
using Tmds.DBus.Protocol;

namespace AirlineManager.Model;

public class ConnectionModel : ModelBase, IDatabase, IDisposable
{
    private const string _host = "cheaply-fortuitous-candlefish.data-1.use1.tembo.io";
    private const int _port = 5432;
    private const string _database = "postgres";
    private readonly string _userID;
    private readonly string _password;

    private string _ConnectionString =>
        $"Host={_host};" +
        $"Port={_port};" +
        $"Database={_database};" +
        $"User Id={_userID};" +
        $"Password={_password};";

    private ConnectionState _databaseConnectionState;
    public ConnectionState DatabaseConnectionState
    {
        get => _databaseConnectionState;
        private set
        {
            this.RaiseAndSetIfChanged(
                ref _databaseConnectionState, 
                value, nameof(DatabaseConnectionState));
            
            if(IsOpen)
                Refresh?.Invoke(this, EventArgs.Empty);
        }
    }

    private NpgsqlConnection? connection;

    public event EventHandler? Refresh;
    public bool IsOpen { get => DatabaseConnectionState == ConnectionState.Open; }

    public ConnectionModel()
    {
        _userID = "db_user_1";
        _password = "db_user1_password";
    }

    public async void Connect()
    {
        connection = new(_ConnectionString);
        connection.StateChange += (o, e) => DatabaseConnectionState = e.CurrentState;
        await connection.OpenAsync();

        await GetData("SELECT a.aircraft_id, at.aircraft_type_name FROM airline.aircraft a JOIN airline.aircraft_type at ON a.aircraft_type_id = at.aircraft_type_id");
    }

    public async Task<List<List<string>>> GetData(string query)
    {
        List<List<string>> data = new();

        await using var cmd = new NpgsqlCommand(query, connection);
        await using var reader = await cmd.ExecuteReaderAsync();
        {
            // this line is simply warning suppresion workaround, otherwise try-catch would be needed here 
            while(await (reader?.ReadAsync() ?? Task<bool>.Factory.StartNew(() => {return false;})))
            {
                var row = new List<string>();
                for(int j = 0; j < reader?.FieldCount; j++)
                {
                    row.Add(reader.GetValue(j)?.ToString() ?? "");
                }
                data.Add(row);
            }
        }

        foreach(var v in data)
        {
            foreach(var s in v)
                Console.Write(s + ", ");
            Console.WriteLine();
        }

        return data;
    }

    public void Dispose()
    {
        connection?.Dispose();
        connection = null;
    }

    ~ConnectionModel()
    {
        Dispose();
    }
}
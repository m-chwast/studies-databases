using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
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

    private NpgsqlDataSource _dataSource;

    public event EventHandler? Refresh;
    public bool IsOpen { get => DatabaseConnectionState == ConnectionState.Open; }

    public ConnectionModel()
    {
        _userID = "db_user_1";
        _password = "db_user1_password";

        _dataSource = NpgsqlDataSource.Create(_ConnectionString);
    }

    public async Task<DataTable> GetData(string query, bool logResult = true)
    {
        DataTable data = new();
        await using var cmd = _dataSource.CreateCommand(query);
        await using var reader = await cmd.ExecuteReaderAsync();
        {
            // this line is simply warning suppresion workaround, otherwise try-catch would be needed here 
            while(await (reader?.ReadAsync() ?? Task<bool>.Factory.StartNew(() => {return false;})))
            {
                var row = new List<string>();
                for(int j = 0; j < reader?.FieldCount; j++)
                {
                    string val = reader.GetValue(j)?.ToString() ?? "";
                    // sanitize by removing excessive whitespaces
                    val = Regex.Replace(val, @"\s+", " ");
                    row.Add(val);
                }
                data.AddRow(row);
            }
        }

        if(logResult)
            Console.WriteLine(data);

        return data;
    }

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    ~ConnectionModel()
    {
        Dispose();
    }
}
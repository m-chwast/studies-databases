using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
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

    private NpgsqlDataSource _dataSource;

    public event EventHandler? Refresh;

    public ConnectionModel()
    {
        _userID = "db_user_1";
        _password = "db_user1_password1";

        _dataSource = NpgsqlDataSource.Create(_ConnectionString);
    }

    public async Task<DataTable> GetData(string query, bool logResult = true)
    {
        DataTable data = new();

        try
        {
            await using var cmd = _dataSource.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();
            
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
        catch
        {
            Console.WriteLine("Exception while reading data! Trying to refresh...");
            
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
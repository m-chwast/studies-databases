using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading.Tasks;
using Npgsql;
using ReactiveUI;

namespace AirlineManager.Model;

public class ConnectionModel : ModelBase, IDatabase, IDisposable
{
    private const string _host = "cheaply-fortuitous-candlefish.data-1.use1.tembo.io";
    private const int _port = 5432;
    private const string _database = "postgres";
    private readonly string _userID;
    private readonly string _password;

    private Timer _refreshTimer;

    private bool _databaseFault = false;
    public bool DatabaseFault
    {
        get => _databaseFault;
        set => this.RaiseAndSetIfChanged(ref _databaseFault, value, nameof(DatabaseFault));
    }

    private int _databaseQueriesTotal = 0;
    public int DatabaseQueriesTotal
    {
        get => _databaseQueriesTotal;
        set => this.RaiseAndSetIfChanged(ref _databaseQueriesTotal, value, nameof(DatabaseQueriesTotal));
    }

    private int _databaseQueriesSuccessful = 0;
    public int DatabaseQueriesSuccessful
    {
        get => _databaseQueriesSuccessful;
        set => this.RaiseAndSetIfChanged(ref _databaseQueriesSuccessful, value, nameof(DatabaseQueriesSuccessful));
    }

    private System.Threading.Mutex _queryCountMutex = new();

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
        _password = "db_user1_password";

        _refreshTimer = new Timer();
        _refreshTimer.Elapsed += (o,e) => DatabaseRefresh();

        _dataSource = NpgsqlDataSource.Create(_ConnectionString);
    }

    public async Task<DataTable> GetData(string query, bool logResult = true)
    {
        _queryCountMutex.WaitOne();
        DatabaseQueriesTotal++;
        _queryCountMutex.ReleaseMutex();

        bool querySuccess = true;
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
                    // remove trailing whitespace
                    if(val[val.Length - 1] == ' ')
                        val = val.Remove(val.Length - 1);

                    row.Add(val);
                }
                data.AddRow(row);
            }
        }
        catch
        {
            querySuccess = false;
            Console.WriteLine("Exception while reading data!");
            if(_refreshTimer.Enabled == false)
            {       
                DatabaseFault = true;
                Console.WriteLine("Trying to refresh...");
                _refreshTimer.Interval = 5000;
                _refreshTimer.AutoReset = false;
                _refreshTimer.Start();
            }
        }

        if(querySuccess)
        {
            _queryCountMutex.WaitOne();
            DatabaseQueriesSuccessful++;
            _queryCountMutex.ReleaseMutex();
        }

        if(logResult)
            Console.WriteLine(data);

        return data;
    }

    private void DatabaseRefresh()
    {
        DatabaseFault = false;
        Console.WriteLine("Invoking Refresh event");
        Refresh?.Invoke(this, EventArgs.Empty);
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
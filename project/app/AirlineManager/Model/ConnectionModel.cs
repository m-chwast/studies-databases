using System.Data;
using Npgsql;
using ReactiveUI;

namespace AirlineManager.Model;

public class ConnectionModel : ModelBase
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
        private set => this.RaiseAndSetIfChanged(
            ref _databaseConnectionState, 
            value, nameof(DatabaseConnectionState));
    }

    public ConnectionModel()
    {
        _userID = "db_user_1";
        _password = "db_user1_password";
    }

    public async void Connect()
    {
        using NpgsqlConnection connection = new(_ConnectionString);
        connection.StateChange += (o, e) => DatabaseConnectionState = e.CurrentState;
        await connection.OpenAsync();

        System.Threading.Thread.Sleep(5000);
    }
}
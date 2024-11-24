using Npgsql;

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

    public ConnectionModel()
    {
        _userID = "db_user_1";
        _password = "db_user1_password";

        Connect();
    }

    public async void Connect()
    {
        using NpgsqlConnection connection = new(_ConnectionString);
        await connection.OpenAsync();
    }
}
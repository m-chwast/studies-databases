using System.Reactive.Linq;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private ConnectionModel _connectionModel = new();

    private ObservableAsPropertyHelper<string> _connectionStatus;
    public string ConnectionStatus => _connectionStatus.Value;

    private ObservableAsPropertyHelper<string> _databaseStats;
    public string DatabaseStats => _databaseStats.Value;

    private ObservableAsPropertyHelper<string> _exceptionMessage;
    public string ExceptionMessage => _exceptionMessage.Value;

    public ConnectionViewModel()
    {
        _connectionStatus = this
            .WhenAnyValue(x => x._connectionModel.DatabaseFault)
            .Select(x => x ? "Fault detected - refreshing..." : "OK")
            .ToProperty(this, x => x.ConnectionStatus);

        _databaseStats = this
            .WhenAnyValue(x => x._connectionModel.DatabaseQueriesTotal,
            x => x._connectionModel.DatabaseQueriesSuccessful)
            .Select(x => GetDatabaseStatsString(x.Item1, x.Item2))
            .ToProperty(this, x => x.DatabaseStats);

        _exceptionMessage = this
            .WhenAnyValue(x => x._connectionModel.ExceptionMessage)
            .Select(x => "Last database exception message: " + x)
            .ToProperty(this, x => x.ExceptionMessage);
    }

    private string GetDatabaseStatsString(int total, int success)
    {
        string stats = "Database Queries: ";
        stats += total.ToString() + " total, ";
        stats += success.ToString() + " successful";
        return stats;
    }

    public IDatabase GetDatabase() => _connectionModel;
}
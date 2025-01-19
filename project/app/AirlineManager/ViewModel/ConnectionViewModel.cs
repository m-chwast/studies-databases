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
            .Select(x => x ? "Wykryto błąd - odświeżanie..." : "OK")
            .ToProperty(this, x => x.ConnectionStatus);

        _databaseStats = this
            .WhenAnyValue(x => x._connectionModel.DatabaseQueriesTotal,
            x => x._connectionModel.DatabaseQueriesSuccessful)
            .Select(x => GetDatabaseStatsString(x.Item1, x.Item2))
            .ToProperty(this, x => x.DatabaseStats);

        _exceptionMessage = this
            .WhenAnyValue(x => x._connectionModel.ExceptionMessage)
            .Select(x => "Ostatni wyjątek bazy danych: " + x)
            .ToProperty(this, x => x.ExceptionMessage);
    }

    private string GetDatabaseStatsString(int total, int success)
    {
        string stats = "Zapytania do bazy danych: ";
        stats += total.ToString() + " Wszystkie, ";
        stats += success.ToString() + " Poprawne";
        return stats;
    }

    public IDatabase GetDatabase() => _connectionModel;
}
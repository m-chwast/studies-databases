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

    public ConnectionViewModel()
    {
        _connectionStatus = this
            .WhenAnyValue(x => x._connectionModel.DatabaseFault)
            .Select(x => x ? "Fault detected - refreshing..." : "OK")
            .ToProperty(this, x => x.ConnectionStatus);

        _databaseStats = this
            .WhenAnyValue(x => x._connectionModel)
            .Select(x => "")
            .ToProperty(this, x => x.DatabaseStats);
    }

    public IDatabase GetDatabase() => _connectionModel;
}
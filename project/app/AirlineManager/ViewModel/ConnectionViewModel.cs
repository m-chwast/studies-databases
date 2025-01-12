using System.Reactive.Linq;
using AirlineManager.Model;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private ConnectionModel _connectionModel = new();

    private ObservableAsPropertyHelper<string> _connectionStatus;
    public string ConnectionStatus => _connectionStatus.Value;

    public ConnectionViewModel(ILogger logger)
    {
        _logger = logger;
        
        _connectionStatus = this
            .WhenAnyValue(x => x._connectionModel.DatabaseFault)
            .Select(x => x ? "Fault detected - refreshing..." : "OK")
            .ToProperty(this, x => x.ConnectionStatus);
    }

    public IDatabase GetDatabase() => _connectionModel;
}
using System.Reactive.Linq;
using AirlineManager.Model;
using ReactiveUI;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private ConnectionModel _connectionModel = new();

    private readonly ObservableAsPropertyHelper<string> _connectionStatus; 
    public string ConnectionStatus => _connectionStatus.Value;

    public ConnectionViewModel(ILogger logger)
    {
        _logger = logger;
        
        _connectionStatus = this
            .WhenAnyValue(x => x._connectionModel.DatabaseConnectionState)
            .Select(status => status.ToString())
            .ToProperty(this, x => x.ConnectionStatus); 

        _logger.LogInformation("Connecting to database");
        _connectionModel.Connect();
    }

    public IDatabase GetDatabase() => _connectionModel;
}
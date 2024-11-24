using System.Reactive.Linq;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private ConnectionModel _connectionModel = new();

    private readonly ObservableAsPropertyHelper<string> _connectionStatus; 
    public string ConnectionStatus => _connectionStatus.Value;

    public ConnectionViewModel()
    {
        _connectionStatus = this
            .WhenAnyValue(x => x._connectionModel.DatabaseConnectionState)
            .Select(status => status.ToString())
            .ToProperty(this, x => x.ConnectionStatus); 

        _connectionModel.Connect();
    }
}
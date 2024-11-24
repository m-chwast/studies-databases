using AirlineManager.Model;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private ConnectionModel _connectionModel = new();
    
    public string ConnectionStatus => "stat1";
}
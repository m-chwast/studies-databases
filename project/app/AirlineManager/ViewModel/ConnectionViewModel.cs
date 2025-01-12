using AirlineManager.Model;
using Microsoft.Extensions.Logging;

namespace AirlineManager.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private ConnectionModel _connectionModel = new();

    public ConnectionViewModel(ILogger logger)
    {
        _logger = logger;
        _logger.LogInformation("Connecting to database");
    }

    public IDatabase GetDatabase() => _connectionModel;
}
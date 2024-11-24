using Microsoft.Extensions.Logging;

namespace AirlineManager.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ConnectionViewModel _connectionViewModel;
    public ConnectionViewModel ConnectionVM { get => _connectionViewModel; }

    public MainWindowViewModel()
    {
        using ILoggerFactory loggerFactory = LoggerFactory
            .Create(builder => builder.AddConsole());
        
        _connectionViewModel = new ConnectionViewModel(
            loggerFactory.CreateLogger("Connection"));
    }
}

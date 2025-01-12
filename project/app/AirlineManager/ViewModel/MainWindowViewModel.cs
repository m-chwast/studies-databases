using AirlineManager.Model;
using Microsoft.Extensions.Logging;

namespace AirlineManager.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ConnectionViewModel _connectionViewModel;
    public ConnectionViewModel ConnectionVM { get => _connectionViewModel; }

    private readonly AircraftViewModel _aircraftViewModel;
    public AircraftViewModel AircraftVM { get => _aircraftViewModel; }

    private readonly PersonnelViewModel _personnelViewModel;
    public PersonnelViewModel PersonnelVM { get => _personnelViewModel; }

    public MainWindowViewModel()
    {
        using ILoggerFactory loggerFactory = LoggerFactory
            .Create(builder => builder.AddConsole());
        
        _connectionViewModel = new ConnectionViewModel(
            loggerFactory.CreateLogger("Connection"));

        IDatabase database = _connectionViewModel.GetDatabase();
        _aircraftViewModel = new AircraftViewModel(database);
        _personnelViewModel = new PersonnelViewModel(database);
    }
}

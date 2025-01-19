using AirlineManager.Model;

namespace AirlineManager.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ConnectionViewModel _connectionViewModel;
    public ConnectionViewModel ConnectionVM { get => _connectionViewModel; }

    private readonly AircraftViewModel _aircraftViewModel;
    public AircraftViewModel AircraftVM { get => _aircraftViewModel; }

    private readonly PersonnelViewModel _personnelViewModel;
    public PersonnelViewModel PersonnelVM { get => _personnelViewModel; }

    private readonly RoutesViewModel _routesViewModel;
    public RoutesViewModel RoutesVM { get => _routesViewModel; }

    private readonly FlightsViewModel _flightsViewModel;
    public FlightsViewModel FlightsVM { get => _flightsViewModel; }

    public MainWindowViewModel()
    {
        _connectionViewModel = new ConnectionViewModel();

        IDatabase database = _connectionViewModel.GetDatabase();
        _aircraftViewModel = new AircraftViewModel(database);
        _personnelViewModel = new PersonnelViewModel(database);
        _routesViewModel = new RoutesViewModel(database);
        _flightsViewModel = new FlightsViewModel(database);
    }
}

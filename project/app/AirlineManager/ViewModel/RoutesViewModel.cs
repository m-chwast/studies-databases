using System.Collections.ObjectModel;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class RoutesViewModel : ViewModelBase
{
    private RoutesModel _model;

    private ObservableCollection<RouteData> _routes = new();
    public ObservableCollection<RouteData> Routes
    {
        get => _routes;
        private set => this.RaiseAndSetIfChanged(ref _routes, value, nameof(Routes));
    }

    private string _newRouteDeparture = "";
    public string NewRouteDeparture
    {
        get => _newRouteDeparture;
        set => this.RaiseAndSetIfChanged(ref _newRouteDeparture, value, nameof(NewRouteDeparture));
    }

    private string _newRouteDestination = "";
    public string NewRouteDestination
    {
        get => _newRouteDestination;
        set => this.RaiseAndSetIfChanged(ref _newRouteDestination, value, nameof(NewRouteDestination));
    }

    private float _newRouteTime = 0;
    public float NewRouteTime
    {
        get => _newRouteTime;
        set => this.RaiseAndSetIfChanged(ref _newRouteTime, value, nameof(NewRouteTime));
    }

    private ObservableCollection<AirportData> _airports = new();
    public ObservableCollection<AirportData> Airports
    {
        get => _airports;
        private set => this.RaiseAndSetIfChanged(ref _airports, value, nameof(Airports));
    }

    public RoutesViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new RoutesModel(database);
    }

    private void Refresh()
    {
        
    }
}
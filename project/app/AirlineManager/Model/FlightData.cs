using System.Collections.ObjectModel;
using ReactiveUI;

namespace AirlineManager.Model;

public class FlightData : ModelBase
{
    public int Id { get; }
    public string Route { get; }
    public string Date { get; }

    private int _aircraft = 0;
    public int Aircraft 
    { 
        get => _aircraft; 
        set => this.RaiseAndSetIfChanged(ref _aircraft, value, nameof(Aircraft)); 
    }

    private ObservableCollection<int> _crew = new();
    public ObservableCollection<int> Crew 
    {
        get => _crew;
        set => this.RaiseAndSetIfChanged(ref _crew, value, nameof(Crew));
    }

    private string _aircraftDetails = string.Empty;
    public string AircraftDetails
    {
        get => _aircraftDetails;
        set => this.RaiseAndSetIfChanged(ref _aircraftDetails, value, nameof(AircraftDetails));
    }

    private string _routeDetails = string.Empty;
    public string RouteDetails
    {
        get => _routeDetails;
        set => this.RaiseAndSetIfChanged(ref _routeDetails, value, nameof(RouteDetails));
    }


    public FlightData(string id, string route, string date)
    {
        Id = int.Parse(id);
        Route = route;
        Date = date;
    }

    public FlightData(FlightData f)
    {
        Id = f.Id;
        Route = f.Route;
        Date = f.Date;
    }
}
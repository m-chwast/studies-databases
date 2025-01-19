using System.Collections.ObjectModel;
using ReactiveUI;

namespace AirlineManager.Model;

public class FlightData : ModelBase
{
    public int Id { get; }

    private string _route = string.Empty;
    public string Route
    {
        get => _route;
        set => this.RaiseAndSetIfChanged(ref _route, value, nameof(Route));
    }

    private string _date = string.Empty;
    public string Date
    {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref _date, value, nameof(Date));
    }

    private string _aircraft = string.Empty;
    public string Aircraft 
    { 
        get => _aircraft; 
        set => this.RaiseAndSetIfChanged(ref _aircraft, value, nameof(Aircraft)); 
    }

    private ObservableCollection<PersonnelData> _crew = new();
    public ObservableCollection<PersonnelData> Crew 
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
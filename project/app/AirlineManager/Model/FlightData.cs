using System.Collections.Generic;

namespace AirlineManager.Model;

public class FlightData
{
    public int Id { get; }
    public string Route { get; }
    public string Date { get; }

    public List<int> Crew { get; set; } = new();
    public int Aircraft { get; set; } = 0;
    public string AircraftDetails { get; set; } = "";
    public string RouteDetails { get; set; } = "";
    

    public FlightData(string id, string route, string date)
    {
        Id = int.Parse(id);
        Route = route;
        Date = date;
    }

    public FlightData()
    {
        Id = 0;
        Route = "";
        Date = "";
    }
}
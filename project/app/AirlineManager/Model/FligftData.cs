using System.Collections.Generic;

namespace AirlineManager.Model;

public class FlightData
{
    public int Id { get; }
    public string Route { get; }
    public string Date { get; }
    public List<int> Crew { get; set; }

    public FlightData(string id, string route, string date)
    {
        Id = int.Parse(id);
        Route = route;
        Date = date;
   
        Crew = new();
    }
}
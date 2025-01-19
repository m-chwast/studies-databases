using System.Collections.Generic;

namespace AirlineManager.Model;

public class FlightData
{
    public int Id { get; }
    public string Route { get; }
    public string Date { get; }
    public string Time { get; }
    public List<int> Crew { get; set; }

    public FlightData(string id, string route, string date, string time)
    {
        Id = int.Parse(id);
        Route = route;
        Date = date;
        Time = time;
   
        Crew = new();
    }
}
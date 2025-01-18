namespace AirlineManager.Model;

public class RouteData
{
    public int Id { get; }
    public string Departure { get; }
    public string Destination { get; }
    public float FlightTime { get; }
    public bool IsSelected { get; set; }

    public RouteData(string id, string departure, string destination, string flightTime, bool isSelected = false)
    {
        Id = int.Parse(id);
        Departure = departure;
        Destination = destination;
        FlightTime = float.Parse(flightTime);
        IsSelected = isSelected;
    }
}
namespace AirlineManager.Model;

public class AircraftData
{
    public int Id { get; }
    public string Name { get; }
    public int Count { get; }

    public AircraftData(string name, string? id = null, string? count = null)
    {
        Id = int.Parse(id ?? "-1");
        Name = name;
        Count = int.Parse(count ?? "-1");
    }
}

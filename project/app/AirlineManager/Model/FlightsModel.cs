namespace AirlineManager.Model;

public class FlightModel : ModelBase
{
    private IDatabase _database;

    public FlightModel(IDatabase database)
    {
        _database = database;
    }
}
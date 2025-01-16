namespace AirlineManager.Model;

public class RoutesModel : ModelBase
{
    private IDatabase _database;

    public RoutesModel(IDatabase database)
    {
        _database = database;
    }
}
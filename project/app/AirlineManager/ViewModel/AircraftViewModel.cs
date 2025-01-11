using AirlineManager.Model;

namespace AirlineManager.ViewModel;

public class AircraftViewModel
{
    private IDatabase _database;

    public AircraftViewModel(IDatabase database)
    {
        _database = database;
        _database.Refresh += (o,e) => Refresh();   
    }

    private void Refresh()
    {
        
    }
}
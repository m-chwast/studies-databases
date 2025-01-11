using AirlineManager.Model;
using DynamicData.Binding;

namespace AirlineManager.ViewModel;

public class AircraftViewModel
{
    private IDatabase _database;

    public IObservableCollection<string>? Aircraft { get; } = null;

    public AircraftViewModel(IDatabase database)
    {
        _database = database;
        _database.Refresh += (o,e) => Refresh(); 
    }

    private void Refresh()
    {
        
    }
}
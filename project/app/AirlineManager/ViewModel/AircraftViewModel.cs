using System.Collections.ObjectModel;
using AirlineManager.Model;

namespace AirlineManager.ViewModel;

public class AircraftViewModel : ViewModelBase
{
    private IDatabase _database;

    public ObservableCollection<string> Aircraft { get; private set; }
   
    public AircraftViewModel(IDatabase database)
    {
        _database = database;
        _database.Refresh += (o,e) => Refresh(); 
    
        Aircraft = new ObservableCollection<string>();    
    }

    private void Refresh()
    {
        InvokeOnUIThread(() => Aircraft.Add("123")); 
    }
}
using System.Collections.ObjectModel;
using AirlineManager.Model;
using Avalonia.Controls;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class AircraftViewModel : ViewModelBase
{
    private IDatabase _database;

    public class AircraftData
    {
        private string _id; 
        public int Id { get => int.Parse(_id); }
        private string _name;
        public string Name { get => _name; }

        public AircraftData(string id, string name)
        {
            _id = id;
            _name = name;
        }
    }
 
    public ObservableCollection<AircraftData> Aircraft { get; private set; } = new();
   
    public AircraftViewModel(IDatabase database)
    {
        _database = database;
        _database.Refresh += (o,e) => Refresh();     
    }

    private async void Refresh()
    {
        const string aircraftQuery = @"
            SELECT a.aircraft_id, at.aircraft_type_name
            FROM airline.aircraft a JOIN airline.aircraft_type at 
            ON a.aircraft_type_id = at.aircraft_type_id";
        var dataTable = await _database.GetData(aircraftQuery);
        InvokeOnUIThread(() => UpdateAirlineCollection(dataTable));
    }

    private void UpdateAirlineCollection(DataTable dataTable)
    {
        ObservableCollection<AircraftData> newAircraft = new();
        foreach(var row in dataTable.Data)
        {
            AircraftData aircraftData = new(row[0], row[1]);
            newAircraft.Add(aircraftData);
        }
        Aircraft = newAircraft;
        this.RaisePropertyChanged(nameof(Aircraft));
    }
}
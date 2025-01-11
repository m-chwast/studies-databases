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
        public int Id { get; }
        public string Name { get; }
        public int Count { get; }

        public AircraftData(string name, string? id = null, int? count = null)
        {
            Id = int.Parse(id ?? "-1");
            Name = name;
            Count = count ?? -1;
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
            AircraftData aircraftData = new(row[1], row[0]);
            newAircraft.Add(aircraftData);
        }
        Aircraft = newAircraft;
        this.RaisePropertyChanged(nameof(Aircraft));
    }
}
using System.Collections.ObjectModel;
using System.Reactive.Linq;
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
 
    private ObservableCollection<AircraftData> _aircraft = new();
    public ObservableCollection<AircraftData> Aircraft 
    { 
        get => _aircraft; 
        private set => this.RaiseAndSetIfChanged(ref _aircraft, value, nameof(Aircraft));   
    }
   
    private bool _isShortList = true;
    public bool IsShortList 
    { 
        get => _isShortList;
        set => this.RaiseAndSetIfChanged(ref _isShortList, value, nameof(IsShortList));
    }

    private readonly ObservableAsPropertyHelper<bool> _idVisible;
    public bool IdVisible => _idVisible.Value;

    private readonly ObservableAsPropertyHelper<bool> _countVisible;
    public bool CountVisible => _countVisible.Value;

    public AircraftViewModel(IDatabase database)
    {
        _database = database;
        _database.Refresh += (o,e) => Refresh();

        _idVisible = this
            .WhenAnyValue(x => x.IsShortList)
            .Select(x => !x)
            .ToProperty(this, x => x.IdVisible);
    
        _countVisible = this
            .WhenAnyValue(x => x.IsShortList)
            .Select(x => x)
            .ToProperty(this, x => x.CountVisible);
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
    }
}
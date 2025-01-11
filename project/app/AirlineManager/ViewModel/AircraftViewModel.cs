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

        public AircraftData(string name, string? id = null, string? count = null)
        {
            Id = int.Parse(id ?? "-1");
            Name = name;
            Count = int.Parse(count ?? "-1");
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
        set
        { 
            Aircraft = new ObservableCollection<AircraftData>();
            this.RaiseAndSetIfChanged(ref _isShortList, value, nameof(IsShortList));
            Refresh();
        }
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
        const string aircraftLongQuery = @"
            SELECT a.aircraft_id, at.aircraft_type_name
            FROM airline.aircraft a JOIN airline.aircraft_type at 
            ON a.aircraft_type_id = at.aircraft_type_id";
        const string aircraftShortQuery = @"
            SELECT at.aircraft_type_name, COUNT(*)
            FROM airline.aircraft a
            JOIN airline.aircraft_type at
            ON a.aircraft_type_id = at.aircraft_type_id
            GROUP BY at.aircraft_type_name";

        bool shortList = IsShortList;
        string query = shortList ? aircraftShortQuery : aircraftLongQuery;
        var dataTable = await _database.GetData(query);
        UpdateAirlineCollection(dataTable, shortList);
    }

    private void UpdateAirlineCollection(DataTable dataTable, bool isShortList)
    {
        ObservableCollection<AircraftData> newAircraft = new();
        foreach(var row in dataTable.Data)
        {
            AircraftData aircraftData = isShortList 
                ? new(row[0], null, row[1])
                : new(row[1], row[0]);
            newAircraft.Add(aircraftData);
        }
        InvokeOnUIThread(() => Aircraft = newAircraft);
    }
}
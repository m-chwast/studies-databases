using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class FlightsViewModel : ViewModelBase
{
    private FlightModel _model;

    private ObservableCollection<FlightData> _flights = new();
    public ObservableCollection<FlightData> Flights
    {
        get => _flights;
        set => this.RaiseAndSetIfChanged(ref _flights, value, nameof(Flights));
    }

    public FlightsViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new FlightModel(database);
    
        TriggerRefreshFlights();
    }

    private void Refresh()
    {
        TriggerRefreshFlights();
    }

    private void TriggerRefreshFlights() => Task.Factory.StartNew(RefreshFlights);

    private async Task RefreshFlights()
    {
        var flights = await _model.GetFlights();
        InvokeOnUIThread(() => Flights = new ObservableCollection<FlightData>(flights));
    }
}
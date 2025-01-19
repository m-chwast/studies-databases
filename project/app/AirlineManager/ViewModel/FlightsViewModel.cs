using System.Collections.ObjectModel;
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
        _model = new FlightModel(database);
    }
}
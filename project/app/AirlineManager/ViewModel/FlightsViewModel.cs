using System;
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



    private FlightData? _selectedFlight;
    public FlightData? SelectedFlight
    {
        get => _selectedFlight;
        set => this.RaiseAndSetIfChanged(ref _selectedFlight, value, nameof(SelectedFlight));
    }

    public FlightsViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new FlightModel(database);
    
        this.WhenAnyValue(x => x.SelectedFlight)
            .Subscribe(async _ => await RefreshFlightDetails());

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

    private async Task RefreshFlightDetails()
    {
        if (SelectedFlight is null)
            return;
        if (SelectedFlight.Id == 0)
            return;

        FlightData detailedFlight = new(SelectedFlight);

        await _model.GetFlightDetails(detailedFlight);
        InvokeOnUIThread(() => 
        {
            SelectedFlight.Aircraft = detailedFlight.Aircraft;
            SelectedFlight.AircraftDetails = detailedFlight.AircraftDetails;
            SelectedFlight.RouteDetails = detailedFlight.RouteDetails;
        });
    }    
}
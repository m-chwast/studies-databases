using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
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

    private FlightData _newFlight = new("0", "", "");
    public FlightData NewFlight
    {
        get => _newFlight;
        set => this.RaiseAndSetIfChanged(ref _newFlight, value, nameof(NewFlight));
    }

    public ReactiveCommand<Unit, Unit> AddFlightCommand { get; }

    private ObservableAsPropertyHelper<bool> _flightDetailsVisible;
    public bool FlightDetailsVisible => _flightDetailsVisible.Value;

    private string _newPersonId = string.Empty;
    public string NewPersonId
    {
        get => _newPersonId;
        set => this.RaiseAndSetIfChanged(ref _newPersonId, value, nameof(NewPersonId));
    }

    public ReactiveCommand<Unit, Unit> AddPersonToCrewCommand { get; }

    public FlightsViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new FlightModel(database);
    
        AddFlightCommand = ReactiveCommand.CreateFromTask(AddFlight, 
            this.WhenAnyValue(x => x.NewFlight.Route, x => x.NewFlight.Date, x => x.NewFlight.Aircraft, 
            (route, date, aircraft) => 
            !string.IsNullOrWhiteSpace(route) 
            && !string.IsNullOrWhiteSpace(date)
            && !string.IsNullOrWhiteSpace(aircraft.ToString())));

        AddPersonToCrewCommand = ReactiveCommand.CreateFromTask(AddPersonToCrew,
            this.WhenAnyValue(x => x.NewPersonId, (person) => !string.IsNullOrEmpty(person)));

        _flightDetailsVisible = this.WhenAnyValue(x => x.SelectedFlight)
            .Select(x => x is not null)
            .ToProperty(this, x => x.FlightDetailsVisible);

        this.WhenAnyValue(x => x.SelectedFlight)
            .Subscribe(async _ => await RefreshFlightDetails());

        TriggerRefreshFlights();
    }

    private async Task AddPersonToCrew()
    {
        if (SelectedFlight is null)
            return;
        if (SelectedFlight.Id == 0)
            return;
        if (!int.TryParse(NewPersonId, out int personId))
            return;

        await _model.AddPersonToCrew(SelectedFlight.Id, personId);
        await RefreshFlightDetails();
    } 

    private async Task AddFlight()
    {
        await _model.AddFlight(NewFlight.Route, NewFlight.Date, NewFlight.Aircraft);
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
            SelectedFlight.Crew = detailedFlight.Crew;
        });
    }    
}
using System;
using System.Reactive.Linq;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class PersonnelViewModel : ViewModelBase
{
    private bool _showFlightAttendants = true;
    public bool ShowFlightAttendants
    {
        get => _showFlightAttendants;
        set => this.RaiseAndSetIfChanged(ref _showFlightAttendants, value, nameof(ShowFlightAttendants));
    }

    private bool _showCaptains = true;
    public bool ShowCaptains
    {
        get => _showCaptains;
        set => this.RaiseAndSetIfChanged(ref _showCaptains, value, nameof(ShowCaptains));
    }   

    private bool _showFirstOfficers = true;
    public bool ShowFirstOfficers
    {
        get => _showFirstOfficers;
        set => this.RaiseAndSetIfChanged(ref _showFirstOfficers, value, nameof(ShowFirstOfficers));
    }

    public PersonnelViewModel(IDatabase database)
    {
        this.WhenAnyValue(x => x.ShowFlightAttendants, x => x.ShowCaptains, x => x.ShowFirstOfficers)
            .Subscribe(_ => TriggerRefresh());
    }

    private void TriggerRefresh()
    {

    }
}
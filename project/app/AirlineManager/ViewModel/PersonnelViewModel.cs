using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class PersonnelViewModel : ViewModelBase
{
    private PersonnelModel _model;

    private ObservableCollection<PersonnelData> _personnel = new();
    public ObservableCollection<PersonnelData> Personnel
    {
        get => _personnel;
        private set => this.RaiseAndSetIfChanged(ref _personnel, value, nameof(Personnel));
    }

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

    private string _newPersonName = "";
    public string NewPersonName
    {
        get => _newPersonName;
        set => this.RaiseAndSetIfChanged(ref _newPersonName, value, nameof(NewPersonName));
    }

    private string _newPersonSurname = "";
    public string NewPersonSurname
    {
        get => _newPersonSurname;
        set => this.RaiseAndSetIfChanged(ref _newPersonSurname, value, nameof(NewPersonSurname));
    }

    private ObservableCollection<string> _roles = new();
    public ObservableCollection<string> Roles
    {
        get => _roles;
        private set => this.RaiseAndSetIfChanged(ref _roles, value, nameof(Roles));
    }

    public PersonnelViewModel(IDatabase database)
    {
        _model = new PersonnelModel(database);

        this.WhenAnyValue(x => x.ShowFlightAttendants, x => x.ShowCaptains, x => x.ShowFirstOfficers)
            .Subscribe(_ => TriggerRefresh());
    }

    private async Task Refresh()
    {
        var newData = await _model.GetNewData(ShowFlightAttendants, ShowCaptains, ShowFirstOfficers);
        InvokeOnUIThread(() => Personnel = new ObservableCollection<PersonnelData>(newData));
    }

    private void TriggerRefresh() => Task.Factory.StartNew(
        async () => await Refresh());
}
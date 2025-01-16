using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

    private string _selectedRole = "";
    public string SelectedRole
    {
        get => _selectedRole;
        set => this.RaiseAndSetIfChanged(ref _selectedRole, value, nameof(SelectedRole));
    }

    private string _deletePersonId = "";
    public string DeletePersonId
    {
        get => _deletePersonId;
        set => this.RaiseAndSetIfChanged(ref _deletePersonId, value, nameof(DeletePersonId));
    }

    private string _deletePersonSurname = "";
    public string DeletePersonSurname
    {
        get => _deletePersonSurname;
        set => this.RaiseAndSetIfChanged(ref _deletePersonSurname, value, nameof(DeletePersonSurname));
    }

    private ObservableCollection<SelectablePersonnelData> _deletePersonnel = new();
    public ObservableCollection<SelectablePersonnelData> DeletePersonnel
    {
        get => _deletePersonnel;
        private set => this.RaiseAndSetIfChanged(ref _deletePersonnel, value, nameof(DeletePersonnel));
    }

    public ReactiveCommand<Unit, Unit> DeletePersonCommand { get; }

    public ReactiveCommand<Unit, Unit> AddPersonCommand { get; }

    public PersonnelViewModel(IDatabase database)
    {
        database.Refresh += async (o,e) => { await RefreshRoles(); await Refresh(); await RefreshPersonsToDelete(); };

        _model = new PersonnelModel(database);

        this.WhenAnyValue(x => x.ShowFlightAttendants, x => x.ShowCaptains, x => x.ShowFirstOfficers)
            .Subscribe(_ => TriggerRefresh());

        this.WhenAnyValue(x => x.DeletePersonId, x => x.DeletePersonSurname)
            .Subscribe(_ => TriggerPersonsToDeleteRefresh());

        AddPersonCommand = ReactiveCommand.CreateFromTask(AddPerson, 
            this.WhenAnyValue(
            x => x.NewPersonName, x => x.NewPersonSurname, x => x.SelectedRole, 
            (a, b, c) => !string.IsNullOrWhiteSpace(a) 
            && !string.IsNullOrWhiteSpace(b)
            && !string.IsNullOrWhiteSpace(c)));

        DeletePersonCommand = ReactiveCommand.CreateFromTask(DeletePerson);

        TriggerRefreshRoles();
    }

    private async Task RefreshRoles()
    {
        var roles = await _model.GetRoles();
        InvokeOnUIThread(() => Roles = new ObservableCollection<string>(roles));
    }

    private async Task Refresh()
    {
        var newData = await _model.GetNewData(ShowFlightAttendants, ShowCaptains, ShowFirstOfficers);
        InvokeOnUIThread(() => Personnel = new ObservableCollection<PersonnelData>(newData));
    }

    private async Task RefreshPersonsToDelete()
    {
        bool idParsed = int.TryParse(DeletePersonId, out int id);
        var persons = await _model.GetFilteredPersonnel(idParsed ? id : null, DeletePersonSurname);
        ObservableCollection<SelectablePersonnelData> selectablePersons = new();
        foreach(var person in persons)
            selectablePersons.Add(new SelectablePersonnelData(person));
        InvokeOnUIThread(() => DeletePersonnel = selectablePersons);
    }

    private void TriggerPersonsToDeleteRefresh() => Task.Factory.StartNew(RefreshPersonsToDelete);

    private void TriggerRefresh() => Task.Factory.StartNew(Refresh);
    private void TriggerRefreshRoles() => Task.Factory.StartNew(RefreshRoles);

    private async Task AddPerson() 
    {
        bool added = await _model.AddPerson(NewPersonName, NewPersonSurname, SelectedRole);
        if(added)
        {
            Console.WriteLine("Person added successfully");
            NewPersonName = "";
            NewPersonSurname = "";
            TriggerRefresh();
        }
        else 
        {
            Console.WriteLine("Failed to add person");
        }
    }

    private async Task DeletePerson()
    {
        IEnumerable<int> selectedId = DeletePersonnel.Where(x => x.IsSelected).Select(x => x.Id);
        bool deleted = await _model.DeletePersons(selectedId);
        if(deleted)
            Console.WriteLine("Person deleted successfully");
        else
            Console.WriteLine("Failed to delete person");
    }
}
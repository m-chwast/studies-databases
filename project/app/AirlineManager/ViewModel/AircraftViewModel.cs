using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AirlineManager.Model;
using Avalonia.Controls;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class AircraftViewModel : ViewModelBase
{
    private AircraftModel _model;
 
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
            TriggerRefresh();
        }
    }

    private readonly ObservableAsPropertyHelper<bool> _idVisible;
    public bool IdVisible => _idVisible.Value;

    private readonly ObservableAsPropertyHelper<bool> _countVisible;
    public bool CountVisible => _countVisible.Value;

    public AircraftViewModel(IDatabase database)
    {
        _model = new AircraftModel(database);

        database.Refresh += async (o,e) => { await Refresh(); };

        _idVisible = this
            .WhenAnyValue(x => x.IsShortList)
            .Select(x => !x)
            .ToProperty(this, x => x.IdVisible);
    
        _countVisible = this
            .WhenAnyValue(x => x.IsShortList)
            .Select(x => x)
            .ToProperty(this, x => x.CountVisible);

        TriggerRefresh();
    }

    private async Task Refresh()
    {
        var newData = await _model.GetNewData(IsShortList);
        InvokeOnUIThread(() => Aircraft = new ObservableCollection<AircraftData>(newData));
    }

    private void TriggerRefresh()
    {
        Task.Factory.StartNew(Refresh);
    }
}
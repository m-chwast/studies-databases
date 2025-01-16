using System.Collections.ObjectModel;
using AirlineManager.Model;
using ReactiveUI;

namespace AirlineManager.ViewModel;

public class RoutesViewModel : ViewModelBase
{
    private RoutesModel _model;

    private ObservableCollection<RouteData> _routes = new();
    public ObservableCollection<RouteData> Routes
    {
        get => _routes;
        private set => this.RaiseAndSetIfChanged(ref _routes, value, nameof(Routes));
    }

    public RoutesViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new RoutesModel(database);
    }

    private void Refresh()
    {
        
    }
}
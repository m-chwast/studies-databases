using AirlineManager.Model;

namespace AirlineManager.ViewModel;

public class RoutesViewModel : ViewModelBase
{
    private RoutesModel _model;

    public RoutesViewModel(IDatabase database)
    {
        database.Refresh += (o,e) => Refresh();

        _model = new RoutesModel(database);
    }

    private void Refresh()
    {
        
    }
}
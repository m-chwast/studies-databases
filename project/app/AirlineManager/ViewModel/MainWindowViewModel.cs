using System.Collections.Generic;

namespace AirlineManager.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private readonly List<ViewModelBase> _viewModels = [];

    private readonly ConnectionViewModel _connectionViewModel;
    public ConnectionViewModel ConnectionVM { get => _connectionViewModel; }

    public MainWindowViewModel()
    {
        _connectionViewModel = new ConnectionViewModel();
    }
}

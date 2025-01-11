using System;
using Avalonia.Threading;
using ReactiveUI;

namespace AirlineManager.ViewModel
{
    public abstract class ViewModelBase : ReactiveObject
    {
        public void InvokeOnUIThread(Action a) => Dispatcher.UIThread.Invoke(a);
    }
}
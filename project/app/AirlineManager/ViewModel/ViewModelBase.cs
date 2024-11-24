using Avalonia.Threading;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace AirlineManager.ViewModel
{
    public abstract class ViewModelBase : ReactiveObject, INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {
            DisplayName ??= string.Empty;
        }

        protected static void DispatcherInvoke(Action a)
        {
            Dispatcher.UIThread.Invoke(a);
        }

        public virtual string DisplayName { get; protected set; }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            this.RaisePropertyChanged(propertyName);
        }

        public void Dispose()
        {
            this.OnDispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {
        }

#if DEBUG
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            Debug.WriteLine(msg);
        }
#endif
    }
}
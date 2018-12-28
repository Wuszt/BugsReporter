using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BugsReporterDevelopment
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T previous, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(previous, value))
                return false;

            previous = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if(eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System.ComponentModel;

namespace dpas.Core
{
    public class ViewModel : Disposable, INotifyPropertyChanged
    {
        public bool IsChange { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (PropertyChanged != null)
                    PropertyChanged = null;
            }
            base.Dispose(disposing);
        }

        public void Raise(string property, bool isChanged = true)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (isChanged && !IsChange)
                isChanged = true;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }
    }
}

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Coligo.Core
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>(Expression<Func<T>> prop)
        {
            var mexpr = prop.Body as MemberExpression;
            if (mexpr != null)
            {
                var name = mexpr.Member.Name;
                OnPropertyChanged(name);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyOfPropertyChange<T>(Expression<Func<T>> prop)
        {
            OnPropertyChanged<T>(prop);
        }

        protected void NotifyOfPropertyChange([CallerMemberName] string prop = "")
        {
            OnPropertyChanged(prop);
        }

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }



    }
}
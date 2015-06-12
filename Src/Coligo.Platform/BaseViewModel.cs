using Coligo.Core;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Coligo.Platform.Binder;
#endif

namespace Coligo.Platform
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Refresh()
        {
            OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> prop)
        {
            var mexpr = prop.Body as MemberExpression;
            if (mexpr != null)
            {
                var name = mexpr.Member.Name;
                OnPropertyChanged(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

#if WINDOWS_PHONE_APP

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public void NavigateWithView<TView>()
            where TView : FrameworkElement
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(TView));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void NavigateWithModel<TViewModel>()
            where TViewModel : BaseViewModel
        {
            var viewType = ViewHelper.GetViewTypeForModel(typeof(TViewModel));
            if (viewType != null)
            {
                var rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null)
                {
                    rootFrame.Navigate(viewType);
                }
            }
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        protected void OnUIThread(Action<object> action, object state)
        {
            if (ColigoEngine.SyncContext != null)
            {
//                SendOrPostCallback callback = s => action(s);
                ColigoEngine.SyncContext.Send(s => action(s), state);
            }
        }

    }
}

using Coligo.Core;
using WpfTabsSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Coligo.Platform.Container;

namespace WpfTabsSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DefaultContainer container = new DefaultContainer();

            ColigoEngine.Initialize(container);

            container.AsSingle<MainViewModel>();
            container.AsNew<Tab1ViewModel>();
            container.AsNew<Tab2ViewModel>();

            this.DispatcherUnhandledException += (s, de) =>
            {
                var ex = de.Exception;
            };
        }
    }
}

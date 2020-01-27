using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Models;
using TKProcessor.WPF.ViewModels;

namespace TKProcessor.WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();

            container.Singleton<IEventAggregator, EventAggregator>();

            container.PerRequest<ShellViewModel, ShellViewModel>();

            container.RegisterInstance(
                typeof(IMapper), 
                "IMapper",
                new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile())).CreateMapper()
            );
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);

            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}

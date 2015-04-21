using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using TungHoanhReader.Services;
using TungHoanhReader.Services.DialogService;
using TungHoanhReader.ViewModels;
using TungHoanhReader.Wrapper;
using INavigationService = TungHoanhReader.Services.INavigationService;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TungHoanhReader
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        private static IUnityContainer _container = new UnityContainer();
        // public static readonly IUnityContainer Container = new UnityContainer();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public static IUnityContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(SessionStateService);
            _container.RegisterInstance<IEventAggregator>(new EventAggregator());
            _container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            _container.RegisterType<ISettingService, SettingServices>(new ContainerControlledLifetimeManager());
            _container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IToastService, ToastService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISessionStateService, SessionStateService>(new ContainerControlledLifetimeManager());
            _container.Resolve<ISettingService>().Init();
            this.Suspending += App_Suspending;
            this.UnhandledException += App_UnhandledException;
            return Task.FromResult<object>(null);
        }

        void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {

            global::System.Diagnostics.Debug.WriteLine(e.Message);
            global::System.Diagnostics.Debug.WriteLine(e.Exception.StackTrace);
            global::System.Diagnostics.Debug.WriteLine(e.Handled);
            global::System.Diagnostics.Debug.WriteLine(sender);
            //var dialogService = _container.Resolve<IDialogService>();
            //string content =e.ToString();
            //string title = "Error";
            //string exitCommand = "Exit";
            //var exit = new UICommand(exitCommand, ex => { Current.Exit(); });
            //dialogService.Show(content, title, exit);
        }

        void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            //_container.Resolve<ISettingService>().SaveStorage();
        }


        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
        }

       
        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
            //SessionStateService.RegisterKnownType(typeof(TagTruyenModel));
            //SessionStateService.RegisterKnownType(typeof(SiteTruyenVM));
            //SessionStateService.RegisterKnownType(typeof(Truyen));
            //SessionStateService.RegisterKnownType(typeof(Chapter));
        }
    }
}
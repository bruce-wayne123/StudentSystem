using StudXam.DI;
using StudXam.Interfaces;
using StudXam.Services;
using StudXam.Views;
using Xamarin.Forms;

namespace StudXam
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }

        public static IDependencyInjectionService DIService { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage();
            DIService = new DI.DependencyInjectionService();
            RegisterDependencies();
            DIService.Build();
        }

        private void RegisterDependencies()
        {
            DIService.RegisterType<FilePickerService, IFilePickerService>(true);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
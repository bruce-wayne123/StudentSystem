using StudXam.Interfaces;
using StudXam.Services;
using StudXam.SqlServices;
using StudXam.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace StudXam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;
        public LoginPage()
        {
            var sqlService = new SqlService(DependencyService.Get<ISQLiteDb>());
            var pageService = new NavigationService();
            _viewModel = new LoginViewModel(sqlService, pageService);
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}
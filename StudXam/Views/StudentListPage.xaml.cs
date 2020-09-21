using StudXam.Interfaces;
using StudXam.Services;
using StudXam.SqlServices;
using StudXam.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudXam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentListPage : ContentPage
    {
        private StudentListPageViewModel _viewModel;

        public StudentListPage()
        {
            var sqlService = new SqlService(DependencyService.Get<ISQLiteDb>());
            var pageService = new NavigationService();
            _viewModel = new StudentListPageViewModel(sqlService, pageService);
            InitializeComponent();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadDataCommand.Execute(null);
        }
    }
}
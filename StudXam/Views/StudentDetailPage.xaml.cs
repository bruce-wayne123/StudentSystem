using StudXam.Interfaces;
using StudXam.Models;
using StudXam.Services;
using StudXam.SqlServices;
using StudXam.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudXam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentDetailPage : ContentPage
    {
        private StudentDetailViewModel _viewModel;

        public StudentDetailPage(Student selectedStudent)
        {
            var sqlService = new SqlService(DependencyService.Get<ISQLiteDb>());
            var pageService = new NavigationService();
            _viewModel = new StudentDetailViewModel(sqlService, pageService, selectedStudent);
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
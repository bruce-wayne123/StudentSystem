using StudXam.Helpers;
using StudXam.Interfaces;
using StudXam.Models;
using StudXam.Services;
using StudXam.SqlServices;
using StudXam.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using GlobalStrings = StudXam.Locale.Strings;

namespace StudXam.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private_Props

        private ISqlService _sqLiteService;
        private NavigationService _pageService;
        private string _userName;
        private string _pin;

        #endregion Private_Props

        #region Public_Props

        public ICommand SubmitCommand { get; private set; }

        public string UserName
        {
            get => _userName;
            set => SetValue(ref _userName, value);
        }

        public string Pin
        {
            get => _pin;
            set => SetValue(ref _pin, value);
        }

        #endregion Public_Props

        #region Constructor

        public LoginViewModel(SqlService sqlService, NavigationService pageService)
        {
            _sqLiteService = sqlService;
            _pageService = pageService;
            SubmitCommand = new Command(async () => await Submit());
        }

        #endregion Constructor

        #region Methods

        private async Task Submit()
        {
            try
            {
                GlobalConstants.IsAdminLoggedIn = false;
                if (!(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pin)))
                {
                    if (UserName.Contains(GlobalStrings.Admin))
                    {
                        await _pageService.PushAsync(new StudentListPage());
                        GlobalConstants.IsAdminLoggedIn = true;
                    }
                    else
                    {
                        var dbList = await _sqLiteService.GetAllDataAsync<Student>();
                        var studentSearch = new ObservableCollection<Student>(dbList).ToList().Where(obj => obj.Name.ToLower().Contains(UserName.ToLower())).FirstOrDefault();
                        if (studentSearch != null)
                        {
                            await _pageService.PushAsync(new StudentDetailPage(studentSearch));
                        }
                        else
                        {
                            await _pageService.DisplayAlert(GlobalStrings.LabelLogin, GlobalStrings.MessageCredentialsCheck, GlobalStrings.GlobalOk);
                            return;
                        }
                    }
                    DependencyService.Get<IToast>().Show($"{GlobalStrings.Welcome}      {UserName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _pageService.DisplayAlert(GlobalStrings.LabelLogin, GlobalStrings.ExceptionMessage, GlobalStrings.GlobalOk);
            }
        }

        #endregion Methods
    }
}
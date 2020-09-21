using StudXam.Interfaces;
using StudXam.Models;
using StudXam.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using GlobalStrings = StudXam.Locale.Strings;

namespace StudXam.ViewModels
{
    public class StudentListPageViewModel : BaseViewModel
    {
        #region Private_Props
        private ObservableCollection<Student> _studentList;
        private ISqlService _sqLiteService;
        private INavigationService _navigationService;
        #endregion Private_Props

        #region Public_Props
        public ICommand LoadDataCommand { get; private set; }
        public Student SelectedStudent { get; set; }

        public ObservableCollection<Student> StudentList
        {
            get => _studentList;
            set => SetValue(ref _studentList, value);
        }

        public ICommand StudentSelectedCommand
        {
            get
            {
                return new Command(_ =>
                {
                    if (SelectedStudent == null)
                        return;

                    _navigationService.PushAsync(new StudentDetailPage(SelectedStudent));

                    SelectedStudent = null;
                });
            }
        }
        #endregion Private_Props

        #region Constructor
        public StudentListPageViewModel(ISqlService sqlService, INavigationService navigationService)
        {
            _sqLiteService = sqlService;
            _navigationService = navigationService;
             LoadDataCommand = new Command(async () => await FetchStudentList());
        }
        #endregion Constructor

        #region Methods
        private async Task FetchStudentList()
        {
            try
            {
                base.ExecuteOnAppearingAsync();
                var studenDBList = await _sqLiteService.GetAllDataAsync<Student>();
                if (studenDBList.Any())
                {
                    StudentList = new ObservableCollection<Student>(studenDBList);
                }
                else
                {
                    var studentList = new List<Student>();
                    studentList.Add(new Student { Name = GlobalStrings.StudentAnkur, Gender = GenderEnum.Male.ToString(), BirthDate = new DateTime(1997, 03, 05), Address = GlobalStrings.AddressAnkur, Class = GlobalStrings.Class6, EmailID = GlobalStrings.EmailAnkur, IsAudited = false });
                    studentList.Add(new Student { Name = GlobalStrings.StudentManish, Gender = GenderEnum.Male.ToString(), BirthDate = new DateTime(2001, 11, 23), Address = GlobalStrings.AddressManish, Class = GlobalStrings.Class3, EmailID = GlobalStrings.EmailManish, IsAudited = true });
                    studentList.Add(new Student { Name = GlobalStrings.StudentSamir, Gender = GenderEnum.Male.ToString(), BirthDate = new DateTime(1999, 05, 14), Address = GlobalStrings.AddressSamir, Class = GlobalStrings.Class1, EmailID = GlobalStrings.EmailSamir, IsAudited = false });
                    studentList.Add(new Student { Name = GlobalStrings.StudentTamanna, Gender = GenderEnum.Female.ToString(), BirthDate = new DateTime(1995, 03, 10), Address = GlobalStrings.AddressTamanna, Class = GlobalStrings.Class6, EmailID = GlobalStrings.EmailTamnna, IsAudited = true });
                    studentList.Add(new Student { Name = GlobalStrings.StudentHema, Gender = GenderEnum.Female.ToString(), BirthDate = new DateTime(1994, 12, 03), Address = GlobalStrings.AddressHema, Class = GlobalStrings.Class8, EmailID = GlobalStrings.EmailHema, IsAudited = false });
                    await _sqLiteService.AddAllData(studentList);
                    var updatedDbList = await _sqLiteService.GetAllDataAsync<Student>();
                    StudentList = new ObservableCollection<Student>(updatedDbList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _navigationService.DisplayAlert(GlobalStrings.FetchStudentList, GlobalStrings.ExceptionMessage, GlobalStrings.GlobalOk);
            }
        }
        #endregion Methods

    }
}
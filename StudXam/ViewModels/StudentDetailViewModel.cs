using StudXam.Helpers;
using StudXam.Interfaces;
using StudXam.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using GlobalStrings = StudXam.Locale.Strings;

namespace StudXam.ViewModels
{
    public class StudentDetailViewModel : BaseViewModel
    {
        #region Private_Props

        private ISqlService _sqLiteService;
        private Student selectedStudent;
        private ObservableCollection<FileModel> _attachFileList;
        private INavigationService _navigationService;

        #endregion Private_Props

        #region Public_Props

        public ICommand SubmitCommand { get; private set; }
        public ICommand LoadDataCommand { get; private set; }

        public Student SelectedStudent
        {
            get => selectedStudent;
            set => SetValue(ref selectedStudent, value);
        }

        public ObservableCollection<FileModel> AttachFileList
        {
            get => _attachFileList;
            set => SetValue(ref _attachFileList, value);
        }

        #endregion Public_Props

        #region Constructor

        public StudentDetailViewModel(ISqlService sqlService, INavigationService navigationService, Student selectedStudent)
        {
            _sqLiteService = sqlService;
            _navigationService = navigationService;
            SelectedStudent = selectedStudent;
            AttachFileList = new ObservableCollection<FileModel>();
            SubmitCommand = new Command(async () => await Submit());
            LoadDataCommand = new Command(async () => await FetchAttachments());
        }

        #endregion Constructor

        #region Methods

        private async Task FetchAttachments()
        {
            try
            {
                var attachMentList = await _sqLiteService.GetAllDataAsync<FileModel>();
                var studentAttachMentList = new ObservableCollection<FileModel>(attachMentList).ToList().Where(obj => obj.RollNo == SelectedStudent.RollNo);
                if (studentAttachMentList != null && studentAttachMentList.Any())
                {
                    AttachFileList = new ObservableCollection<FileModel>(studentAttachMentList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _navigationService.DisplayAlert(GlobalStrings.LabelUpdate, GlobalStrings.ExceptionMessage, GlobalStrings.GlobalOk);
            }
        }

        async private Task Submit()
        {
            try
            {
                if (SelectedStudent != null)
                {
                    await _sqLiteService.UpdateData(SelectedStudent);
                    if (AttachFileList.Any())
                    {
                        foreach (var fileItem in AttachFileList)
                        {
                            fileItem.RollNo = SelectedStudent.RollNo;
                        }
                        await _sqLiteService.AddAllData(AttachFileList.ToList());
                    }
                    await _navigationService.DisplayAlert(SelectedStudent.Name, GlobalStrings.MessageRecordUpdate, GlobalStrings.GlobalOk);
                    if (GlobalConstants.IsAdminLoggedIn)
                    {
                        await _navigationService.PopAsync();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _navigationService.DisplayAlert(GlobalStrings.LabelUpdate, GlobalStrings.ExceptionMessage, GlobalStrings.GlobalOk);
            }
        }
    }

    #endregion Methods
}
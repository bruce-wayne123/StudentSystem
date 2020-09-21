using StudXam.Interfaces;
using StudXam.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using GlobalStrings = StudXam.Locale.Strings;

namespace StudXam.CommonControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttachmentUploadView : ContentView, INotifyPropertyChanged
    {
        #region Properties

        public ObservableCollection<FileModel> FileList { get; set; }

        public event EventHandler<SelectionChangedEventArgs> FilePickerSelectionChanged;

        public ICommand PickFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }

        public DataTemplate CollectionCustomTemplate
        {
            get => (DataTemplate)GetValue(CollectionCustomTemplateProperty);
            set => SetValue(CollectionCustomTemplateProperty, value);
        }

        public ObservableCollection<FileModel> FileCollectionList
        {
            get => (ObservableCollection<FileModel>)GetValue(FileCollectionListProperty);
            set => SetValue(FileCollectionListProperty, value);
        }

        public ICommand SelectedItemsChangedCommand
        {
            get => (ICommand)GetValue(SelectedItemsChangedCommandProperty);
            set => SetValue(SelectedItemsChangedCommandProperty, value);
        }

        public object SelectedItemsChangedCommandParameter
        {
            get => GetValue(SelectedItemsChangedCommandParameterProperty);
            set => SetValue(SelectedItemsChangedCommandParameterProperty, value);
        }

        public Style AddButtonCustomStyle
        {
            get => (Style)GetValue(AddButtonCustomStyleProperty);
            set => SetValue(AddButtonCustomStyleProperty, value);
        }

        public int MaxItemsInList
        {
            get => (int)GetValue(MaxItemsInListProperty);
            set => SetValue(MaxItemsInListProperty, value);
        }

        public static readonly BindableProperty AddButtonCustomStyleProperty = BindableProperty.Create(
            nameof(AddButtonCustomStyle),
            typeof(Style),
            typeof(AttachmentUploadView),
            propertyChanged: AddButtonCustomStyleChanged
            );

        public static readonly BindableProperty SelectedItemsChangedCommandProperty = BindableProperty.Create(
            nameof(SelectedItemsChangedCommand),
            typeof(ICommand),
            typeof(AttachmentUploadView)
            );

        public static readonly BindableProperty SelectedItemsChangedCommandParameterProperty = BindableProperty.Create(
            nameof(SelectedItemsChangedCommandParameter),
            typeof(object),
            typeof(AttachmentUploadView)
            );

        public static readonly BindableProperty CollectionCustomTemplateProperty = BindableProperty.Create(
            nameof(CollectionCustomTemplate),
            typeof(DataTemplate),
            typeof(AttachmentUploadView),
            propertyChanged: CollectionTemplateChanged
            );

        public static readonly BindableProperty MaxItemsInListProperty = BindableProperty.Create(
            nameof(MaxItemsInList),
            typeof(int),
            typeof(AttachmentUploadView),
            10
            );

        public static readonly BindableProperty FileCollectionListProperty = BindableProperty.Create(
            nameof(FileCollectionList),
            typeof(ObservableCollection<FileModel>),
            typeof(AttachmentUploadView),
            defaultBindingMode: BindingMode.TwoWay
            );

        #endregion Properties

        #region Constructor

        public AttachmentUploadView()
        {
            InitializeComponent();
            FileList = new ObservableCollection<FileModel>();
            PickFileCommand = new Command(async () => await AddFileToListAsync());
            DeleteFileCommand = new Command<FileModel>((file) =>
            {
                FileList?.Remove(file);
                FileCollectionList = new ObservableCollection<FileModel>(FileList);
            });
            BindingContext = this;
        }

        #endregion Constructor

        #region Methods

        private static void CollectionTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                var view = bindable as AttachmentUploadView;
                view.CollectionCustomTemplate = newValue as DataTemplate;
            }
        }

        private static void AddButtonCustomStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                var view = bindable as AttachmentUploadView;
                view.AddButtonCustomStyle = newValue as Style;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private async Task AddFileToListAsync()
        {
            if (MaxItemsInList != FileList?.Count)
            {
                var fileData = await App.DIService.Resolve<IFilePickerService>().GetFileAsync();
                if (fileData.IsFiledPicked)
                {
                    var fileAlreadyExist = FileList.Any(f => f.FileName == fileData.FileName);
                    if (!fileAlreadyExist)
                    {
                        FileList?.Add(
                            new FileModel
                            {
                                FileId = FileList.Count + 1,
                                FileName = fileData.FileName,
                                Base6String = fileData.Base6String,
                                FileType = fileData.FileType,
                                IsFiledPicked = fileData.IsFiledPicked,
                                SysfileName = string.Format(@"{0}" + fileData.FileType, Guid.NewGuid()),
                                NewUpload = 1,
                                IsDelete = 0,
                                Active = true
                            });
                        FileCollectionList?.ForEach((olditem) =>
                        {
                            if (olditem.NewUpload == 0 && !FileList.Any(o => o == olditem))
                            {
                                FileList.Add(olditem);
                            }
                        });
                        FileCollectionList = new ObservableCollection<FileModel>(FileList);
                    }
                    else
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.Current.MainPage.DisplayAlert(GlobalStrings.TitleFilePicker, GlobalStrings.MessageFileExists, GlobalStrings.GlobalOk);
                        });
                    }
                }
                else
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.Current.MainPage.DisplayAlert(GlobalStrings.TitleFilePicker, fileData.Message, GlobalStrings.GlobalOk);
                    });
                }
            }
            else
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert(GlobalStrings.TitleFilePicker, string.Format(GlobalStrings.MessageMaxFileLimit, MaxItemsInList.ToString()), GlobalStrings.GlobalOk);
                });
            }
        }

        private void CollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilePickerSelectionChanged?.Invoke(sender, e);
            if (e?.CurrentSelection != null)
            {
                SelectedItemsChangedCommand?.Execute(e?.CurrentSelection?.ToList());
            }
            else
            {
                SelectedItemsChangedCommand?.Execute(null);
            }
        }
    }

    #endregion Methods
}
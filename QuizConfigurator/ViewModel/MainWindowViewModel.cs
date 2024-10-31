using QuizConfigurator.Command;
using QuizConfigurator.Dialogs;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAccess;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace QuizConfigurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _selectedViewModel;
        private QuestionPackViewModel? _activePack;
        private ObservableCollection<QuestionPackViewModel> _packs;
        private bool _isFullscreen;  
        
        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
                SetConfigurationViewCommand?.RaiseCanExecuteChanged();
                SetPlayerViewCommand?.RaiseCanExecuteChanged();
            }
        }    
        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }
        public ICreatePackDialogService CreatePackDialogService { get; }
        public QuestionPacksRepository QuestionPacksRepository { get; }
        public ObservableCollection<QuestionPackViewModel>? Packs
        {
            get => _packs; 
            set
            {
                _packs = value;
                RaisePropertyChanged();
                DeletePackCommand.RaiseCanExecuteChanged();
            }
        }
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged("ActivePack");
            }
        }     
        public bool IsFullscreen 
        { 
            get => _isFullscreen;
            set
            {
                _isFullscreen = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel(ICreatePackDialogService createPackDialogService)
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this, new PackOptionsDialogService());
            SelectedViewModel = ConfigurationViewModel;
            QuestionPacksRepository = new QuestionPacksRepository();
            CreatePackDialogService = createPackDialogService;
            OpenCreatePackCommand = new(OpenCreatePackDialog);
            SetActivePackCommand = new(SetActivePack);
            DeletePackCommand = new(DeletePack, CanDeletePack);
            SaveCommand = new(Save);
            ExitAppCommand = new(ExitApp);
            ToggleFullscreenCommand = new(ToggleFullscreen);

            SetConfigurationViewCommand = new(_selectedViewModel => SelectedViewModel = ConfigurationViewModel, 
                                              _selectedViewModel => SelectedViewModel != ConfigurationViewModel);

            SetPlayerViewCommand = new(_selectedViewModel => SelectedViewModel = PlayerViewModel,
                                       _selectedViewModel => SelectedViewModel != PlayerViewModel);
            Packs = new ObservableCollection<QuestionPackViewModel>();
            
            Load();
           }

        private void ToggleFullscreen(object obj)
        {
            IsFullscreen = !IsFullscreen;
        }

        public DelegateCommand SetPlayerViewCommand { get; }
        public DelegateCommand SetConfigurationViewCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand OpenCreatePackCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand ExitAppCommand { get; }
        public DelegateCommand ToggleFullscreenCommand { get; }


        private void SetActivePack(object obj)
        {
            if (obj is QuestionPackViewModel pack)
            {
                ActivePack = pack;
            }
        }
        
        private void OpenCreatePackDialog(object obj)
        {
            var newPack = CreatePackDialogService.ShowDialog();
            if (newPack != null)
            {
                Packs?.Add(newPack);
                ActivePack = newPack;
            }
            DeletePackCommand.RaiseCanExecuteChanged();
        }
       
        private async void Load()
        {
            ObservableCollection<QuestionPack> loadedPacks = await QuestionPacksRepository.Read();

            if (loadedPacks == null)
            {
                Packs.Add(new QuestionPackViewModel(new QuestionPack("<Untitled Pack>")));
            }
            else
            {
                foreach (var pack in loadedPacks)
                {
                    Packs.Add(new QuestionPackViewModel(pack));
                }
            }

            ActivePack = Packs.FirstOrDefault();
        }
 
        private void Save(object obj)
        {
            QuestionPacksRepository.Write(Packs);
        }

        private bool CanDeletePack(object? arg)
        {
            return Packs.Count > 1;
        }

        private void DeletePack(object obj)
        {
            Packs?.Remove(ActivePack);
            ActivePack = Packs?.FirstOrDefault();
            DeletePackCommand.RaiseCanExecuteChanged();
        }

        private void ExitApp(object obj)
        {
            Save(null);
            Environment.Exit(0);
        }
    }
}

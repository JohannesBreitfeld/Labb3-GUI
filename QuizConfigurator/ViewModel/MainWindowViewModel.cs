using QuizConfigurator.Command;
using QuizConfigurator.Dialogs;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAccess;
using QuizConfigurator.Services;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
using System.Windows;

namespace QuizConfigurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;
        private QuestionPackViewModel? _activePack;
        private ObservableCollection<QuestionPackViewModel>? _packs;
        private bool _isFullscreen;  
        
        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
                SetConfigurationViewCommand?.RaiseCanExecuteChanged();
                SetPlayerViewCommand?.RaiseCanExecuteChanged();
                ConfigurationViewModel?.OpenPackOptionsCommand?.RaiseCanExecuteChanged();
                ConfigurationViewModel?.AddQuestionCommand?.RaiseCanExecuteChanged();
                ConfigurationViewModel?.ImportQuestionsCommand?.RaiseCanExecuteChanged();
                SetActivePackCommand?.RaiseCanExecuteChanged();
                OpenCreatePackCommand?.RaiseCanExecuteChanged();
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
            ConfigurationViewModel = new ConfigurationViewModel(this, new PackOptionsDialogService(), new ImportQuestionsDialogService());
            SelectedViewModel = ConfigurationViewModel;
            QuestionPacksRepository = new QuestionPacksRepository();
            CreatePackDialogService = createPackDialogService;
            OpenCreatePackCommand = new(OpenCreatePackDialog, 
                                        (object? _) => SelectedViewModel == ConfigurationViewModel);
            SetActivePackCommand = new(SetActivePack, 
                                       (object? _)=> SelectedViewModel == ConfigurationViewModel);
            DeletePackCommand = new(DeletePack, 
                                    (object? _) => Packs?.Count > 1);
            SaveCommand = new(Save);
            ExitAppCommand = new(ExitApp);
            ToggleFullscreenCommand = new((object _) => IsFullscreen = !IsFullscreen);
            SetConfigurationViewCommand = new((object _) => SelectedViewModel = ConfigurationViewModel, 
                                              (object? _) => SelectedViewModel != ConfigurationViewModel);
            SetPlayerViewCommand = new((object _) => SelectedViewModel = PlayerViewModel,
                                       (object? _) => SelectedViewModel != PlayerViewModel);
            Packs = [];
            
            Load();
           }

        public DelegateCommand SetPlayerViewCommand { get; }
        public DelegateCommand SetConfigurationViewCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand OpenCreatePackCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand ExitAppCommand { get; }
        public DelegateCommand ToggleFullscreenCommand { get; }

        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel pack)
            {
                ActivePack = pack;
            }
        }
        
        private void OpenCreatePackDialog(object? obj)
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
            ObservableCollection<QuestionPack>? loadedPacks = await QuestionPacksRepository.Read();

            if (loadedPacks == null)
            {
                Packs?.Add(new QuestionPackViewModel(new QuestionPack("<Untitled Pack>")));
            }
            else
            {
                foreach (var pack in loadedPacks)
                {
                    Packs?.Add(new QuestionPackViewModel(pack));
                }
            }
            ActivePack = Packs?.FirstOrDefault();
        }
 
        private void Save(object? obj)
        {
            if (Packs != null)
            {
                QuestionPacksRepository.Write(Packs);
            }
        }

        private void DeletePack(object? obj)
        {
            if (ActivePack != null)
            {
                var confirmationWindow =  MessageBox.Show($"Are you sure you want to delete '{ActivePack}'?", "Delete question pack?", MessageBoxButton.YesNo);

                if (confirmationWindow == MessageBoxResult.Yes)
                {
                    Packs?.Remove(ActivePack);
                    ActivePack = Packs?.FirstOrDefault();
                    DeletePackCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void ExitApp(object? obj)
        {
            Save(null);
            Environment.Exit(0);
        }
    }
}

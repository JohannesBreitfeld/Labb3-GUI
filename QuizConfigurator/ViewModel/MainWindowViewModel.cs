using QuizConfigurator.Command;
using QuizConfigurator.Dialogs;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAcess;
using System.Collections.ObjectModel;

namespace QuizConfigurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }
        public ICreatePackDialogService CreatePackDialogService { get; }
        public QuestionPacksRepository QuestionPacksRepository { get; }
        private ObservableCollection<QuestionPackViewModel> _packs;
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
        private QuestionPackViewModel? _activePack;

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
  
        public MainWindowViewModel(ICreatePackDialogService createPackDialogService)
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this, new PackOptionsDialogService());
            QuestionPacksRepository = new QuestionPacksRepository();
            CreatePackDialogService = createPackDialogService;
            OpenCreatePackCommand = new(OpenCreatePackDialog);
            SetActivePackCommand = new(SetActivePack);
            DeletePackCommand = new(DeletePack, CanDeletePack);
            SaveCommand = new(Save);

            Packs = new ObservableCollection<QuestionPackViewModel>();
            Load();
           
            
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

        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand OpenCreatePackCommand { get; }
        public DelegateCommand SaveCommand { get; }
        private void SetActivePack(object obj)
        {
            ActivePack = obj as QuestionPackViewModel;
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

    }
}

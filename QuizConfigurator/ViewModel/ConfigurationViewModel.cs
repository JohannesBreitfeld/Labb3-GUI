using QuizConfigurator.Command;
using QuizConfigurator.Model;
using QuizConfigurator.Services;

namespace QuizConfigurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel, 
            IEditPackDialogService packOptionsDialogService,
            IImportQuestionsDialogService importQuestionsDialogService)
        {
            this.mainWindowViewModel = mainWindowViewModel;
       
            AddQuestionCommand = new (AddQuestion, CanExecute);
            DeleteQuestionCommand = new(DeleteQuestion, (_activeQuestion) => ActiveQuestion != null);
            ImportQuestionsCommand = new(ImportQuestion, CanExecute);
            OpenPackOptionsCommand = new(OpenPackOptionsDialog, CanExecute);
            PackOptionsDialogService = packOptionsDialogService;
            ImportQuestionsDialogService = importQuestionsDialogService;
        }

        private Question? _activeQuestion;
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack => mainWindowViewModel?.ActivePack; 
        public Question? ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                DeleteQuestionCommand.RaiseCanExecuteChanged();
            }
        }

        private IImportQuestionsDialogService ImportQuestionsDialogService { get; }
        private IEditPackDialogService PackOptionsDialogService { get; }

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand DeleteQuestionCommand { get; }
        public DelegateCommand ImportQuestionsCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }

        public bool CanExecute(object? arg) => mainWindowViewModel?.SelectedViewModel == mainWindowViewModel?.ConfigurationViewModel;
    
        private void AddQuestion(object obj)
        {
            ActiveQuestion = new Question("New question", "Correct Answer", "First Incorrect Answer", "Second Incorrect Answer", "Third Incorrect Answer");
            ActivePack?.Questions.Add(ActiveQuestion);
        }

        private void DeleteQuestion(object obj)
        {
            if (ActiveQuestion != null)
            {
                ActivePack?.Questions.Remove(ActiveQuestion);
                ActiveQuestion = null;
            }
        }
       
        private void OpenPackOptionsDialog(object obj)
        {
            if (mainWindowViewModel?.ActivePack != null)
            {
                mainWindowViewModel.ActivePack = PackOptionsDialogService.ShowDialog(mainWindowViewModel.ActivePack);
            }
        }

        private async void ImportQuestion(object obj)
        {
            if (mainWindowViewModel?.ActivePack != null)
            {
                mainWindowViewModel.ActivePack = await ImportQuestionsDialogService.ShowDialog(mainWindowViewModel.ActivePack);
            }
        }

    }
}

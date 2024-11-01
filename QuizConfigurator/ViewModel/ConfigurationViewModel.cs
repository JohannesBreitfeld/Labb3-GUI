using QuizConfigurator.Command;
using QuizConfigurator.Model;
using QuizConfigurator.Services;

namespace QuizConfigurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel, IPackOptionsDialogService packOptionsDialogService)
        {
            this.mainWindowViewModel = mainWindowViewModel;
       
            AddQuestionCommand = new (AddQuestion);
            DeleteQuestionCommand = new(DeleteQuestion, (_activeQuestion) => ActiveQuestion != null);
          
            OpenPackOptionsCommand = new(OpenPackOptionsDialog);
            PackOptionsDialogService = packOptionsDialogService;
        }

        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack => mainWindowViewModel?.ActivePack; 
        private Question? _activeQuestion;
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

        public IPackOptionsDialogService PackOptionsDialogService { get; }

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand DeleteQuestionCommand { get; }

        public DelegateCommand OpenPackOptionsCommand { get; }
   
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

    }
}

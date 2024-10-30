using QuizConfigurator.Command;
using QuizConfigurator.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace QuizConfigurator.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private Question _currentQuestion;
        private int _currentQuestionIndex;
        private ObservableCollection<string> _currentAnswers;
        private ObservableCollection<Question> _questionsInRandomOrder;
        public int Score { get; set; }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
       
        public ObservableCollection<string> CurrentAnswers 
        { 
            get => _currentAnswers;
            set
            {
                _currentAnswers = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> QuestionsInRandomOrder { 
            get => _questionsInRandomOrder;
            set
            {
                _questionsInRandomOrder = value;
                RaisePropertyChanged();
            }
        }
        public Question CurrentQuestion 
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged();
            } 
        }
        public int CurrentQuestionIndex 
        { 
            get => _currentQuestionIndex;
            set { 
                _currentQuestionIndex = value;
                RaisePropertyChanged();
            } 
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            QuestionsInRandomOrder = new ObservableCollection<Question>();
            _currentQuestionIndex = 0;
            StartGameCommand = new(StartGame);
            AnswerSelectedCommand = new(AnswerSelected);
        }
        public DelegateCommand AnswerSelectedCommand { get; }
        public DelegateCommand StartGameCommand { get; }

        private void StartGame(object obj)
        {
            Score = 0;
            if (ActivePack != null && ActivePack.Questions.Count > 0)
            {
                Random random = new Random();
                List<Question> q =  new(ActivePack.Questions.OrderBy(q => random.Next()).ToList());
                q.ForEach(q => QuestionsInRandomOrder.Add(q));
                ShowNextQuestion();
            }
        }

        private void ShowNextQuestion()
        {
            if (CurrentQuestionIndex < QuestionsInRandomOrder.Count)
            {
                CurrentQuestion = QuestionsInRandomOrder[CurrentQuestionIndex];
                SetAnswers();
            }
            else
            {
                // Game Over
            }
        }
        private void SetAnswers()
        {
            Random random = new Random();
            var answers = new List<string> { CurrentQuestion.CorrectAnswer };
            answers.AddRange(CurrentQuestion.IncorrectAnswers);
            CurrentAnswers = new ObservableCollection<string>(answers.OrderBy(a => random.Next()).ToList());
        }
        public void AnswerSelected(object selectedAnswer)
        {
            if (selectedAnswer is string answer)
            {
                if (answer == CurrentQuestion.CorrectAnswer)
                {
                    Score++;
                }
                else
                {
                    // Wrong answer
                }
            }
            CurrentQuestionIndex++;
            ShowNextQuestion();
        }
    }
}
//private DispatcherTimer timer;
//private int _timeLeft;
//public int TimeLeft
//{
//    get { return _timeLeft; }
//    set 
//    {
//        _timeLeft = value;
//        RaisePropertyChanged();
//    }
//}
//TimeLeft = ActivePack.TimeLimitInSeconds;
////Behöver konfigureras
//timer = new DispatcherTimer();
//timer.Interval = TimeSpan.FromSeconds(1);
//timer.Tick += Timer_Tick;
////timer.Start();
//UpdateButtonCommand = new DelegateCommand(UpdateButton);
//AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);

//private void Timer_Tick(object? sender, EventArgs e)
//{
//    //TimeLeft--;
//}
using QuizConfigurator.Command;
using QuizConfigurator.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace QuizConfigurator.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private DispatcherTimer timer;
        private Question _currentQuestion;
        private int _currentQuestionIndex;
        private int _timeLeft;
        private int _score;
        private bool _isPlaying;
        private bool _isGameOver;
        private ObservableCollection<string> _currentAnswers;
        private ObservableCollection<Question> _questionsInRandomOrder;
        private readonly MainWindowViewModel? mainWindowViewModel;
        public int Score 
        {
            get => _score;
            set
            {
                _score = value;
                RaisePropertyChanged();
            }
        }
        public bool IsGameOver 
        {
            get => _isGameOver;
            set
            {
                _isGameOver = value;
                RaisePropertyChanged();
            } 
        }
        public bool IsPlaying 
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        public int TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                _timeLeft = value;
                RaisePropertyChanged();
                if (value == 0)
                {
                    AnswerSelected(null);
                }
            }
        }
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
            StartGameCommand = new(StartGame);
            AnswerSelectedCommand = new(AnswerSelected);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        public DelegateCommand AnswerSelectedCommand { get; }
        public DelegateCommand StartGameCommand { get; }

        private void StartGame(object obj)
        {
            IsGameOver = false;
            IsPlaying = true;
            Score = 0;
            CurrentQuestionIndex = 0;
            if (ActivePack != null && ActivePack.Questions.Count > 0)
            {
                Random random = new Random();
                QuestionsInRandomOrder =  new(ActivePack.Questions.OrderBy(q => random.Next()).ToList());

                ShowNextQuestion();
            }
        }

        private void ShowNextQuestion()
        {
            CurrentQuestionIndex++;

            if (CurrentQuestionIndex - 1 < QuestionsInRandomOrder.Count)
            {
                TimeLeft = ActivePack.TimeLimitInSeconds;
                timer.Start();
                CurrentQuestion = QuestionsInRandomOrder[CurrentQuestionIndex - 1];
                SetAnswers();
            }
            else
            {
                timer.Stop();
                IsGameOver = true;
                IsPlaying = false;
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
            ShowNextQuestion();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }
    }
}



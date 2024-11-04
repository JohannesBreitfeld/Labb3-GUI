using QuizConfigurator.Command;
using QuizConfigurator.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;

namespace QuizConfigurator.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private DispatcherTimer timer;
        private Question? _currentQuestion;
        private int _currentQuestionIndex;
        private int _timeLeft;
        private int _score;
        private bool _isPlaying;
        private bool _isGameOver;
        private ObservableCollection<string>? _currentAnswers;
        private ObservableCollection<Question>? _questionsInRandomOrder;
        private readonly MainWindowViewModel? mainWindowViewModel;
        private ObservableCollection<ButtonColor>? _buttonColors;
        private bool _buttonsEnabled;

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
        public ObservableCollection<string>? CurrentAnswers 
        { 
            get => _currentAnswers;
            set
            {
                _currentAnswers = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question>? QuestionsInRandomOrder { 
            get => _questionsInRandomOrder;
            set
            {
                _questionsInRandomOrder = value;
                RaisePropertyChanged();
            }
        }
        public Question? CurrentQuestion 
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
        public ObservableCollection<ButtonColor>? ButtonColors
        {
            get => _buttonColors;
            set
            {
                _buttonColors = value;
                RaisePropertyChanged();
            }
        }
        public bool ButtonsEnabled 
        {
            get => _buttonsEnabled;
            set
            {
                _buttonsEnabled = value;
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
            else
            {
                IsGameOver = true;
                IsPlaying = false;
            }
        }

        private void ShowNextQuestion()
        {
            CurrentQuestionIndex++;
            
            if (ActivePack != null && QuestionsInRandomOrder != null)
            {
                if (CurrentQuestionIndex - 1 < QuestionsInRandomOrder.Count)
                {
                    TimeLeft = ActivePack.TimeLimitInSeconds;
                    timer.Start();
                    ButtonsEnabled = true;
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
        }
        private void SetAnswers()
        {
            if (CurrentQuestion != null && CurrentQuestion.CorrectAnswer != null && CurrentQuestion.IncorrectAnswers != null)
            {
                Random random = new Random();
                var answers = new List<string> { CurrentQuestion.CorrectAnswer };
                answers.AddRange(CurrentQuestion.IncorrectAnswers);
                CurrentAnswers = new ObservableCollection<string>(answers.OrderBy(a => random.Next()).ToList());
                
                ButtonColors = new ObservableCollection<ButtonColor>();
                foreach (var answer in CurrentAnswers)
                {
                    ButtonColors.Add(new ButtonColor { Answer = answer, Color = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)) }); 
                }
            }
        }
        public async void AnswerSelected(object? selectedAnswer)
        {
            if (selectedAnswer is string answer && CurrentQuestion != null)
            {
                if (answer == CurrentQuestion.CorrectAnswer)
                {
                    Score++;
                    UpdateButtonColors(answer, true);
                }
                else
                {
                    UpdateButtonColors(answer, false);
                }
                timer.Stop();
                
                ButtonsEnabled = false;
                await Task.Delay(2000);
            }
            ShowNextQuestion();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }
        private void UpdateButtonColors(string selectedAnswer, bool isCorrect)
        {
            for (int i = 0; i < ButtonColors?.Count; i++)
            {
                if (ButtonColors[i].Answer == CurrentQuestion?.CorrectAnswer)
                {
                    ButtonColors[i].Color = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)); 
                }
                else if (ButtonColors[i].Answer == selectedAnswer && !isCorrect)
                {
                    ButtonColors[i].Color = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)); 
                }
                RaisePropertyChanged(nameof(ButtonColors));
            }
        }
    }
}



using QuizConfigurator.Command;
using System.Windows.Threading;

namespace QuizConfigurator.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private int _timeLeft;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        //public int TimeLeft
        //{
        //    get { return _timeLeft; }
        //    set 
        //    {
        //        _timeLeft = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public DelegateCommand UpdateButtonCommand { get; }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            //TimeLeft = ActivePack.TimeLimitInSeconds;
            ////Behöver konfigureras
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += Timer_Tick;
            ////timer.Start();
            //UpdateButtonCommand = new DelegateCommand(UpdateButton);
            //AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            //TimeLeft--;
        }

    }
}

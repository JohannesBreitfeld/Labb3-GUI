using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;


namespace QuizConfigurator.ViewModel
{
    class QuestionPackViewModel : ViewModelBase
    {
        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);
        }
        private readonly QuestionPack model;

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> Questions { get; }

        public override string ToString()
        {
            return $"{Name}  ({Difficulty})";
        }
    }
}

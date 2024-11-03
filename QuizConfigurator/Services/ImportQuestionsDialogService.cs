using QuizConfigurator.Dialogs;
using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAccess.OpenTdbApi;
using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;
using QuizConfigurator.ViewModel;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;

namespace QuizConfigurator.Services
{
    class ImportQuestionsDialogService : ViewModelBase, IImportQuestionsDialogService
    {
        private ObservableCollection<TriviaCategory>? _triviaCategories;
        private int _numberOfQuestions;
        private TriviaCategory? _triviaCategoryChosen;
        private string? _difficultyChosen;

        public ObservableCollection<TriviaCategory>? TriviaCategories
        {
            get => _triviaCategories;
            set
            {
                _triviaCategories = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<string>? Difficulties { get; }

        public int NumberOfQuestions
        {
            get => _numberOfQuestions;
            set
            {
                _numberOfQuestions = value;
                RaisePropertyChanged();
            }
        }
        public TriviaCategory? TriviaCategoryChosen
        {
            get => _triviaCategoryChosen;
            set
            {
                _triviaCategoryChosen = value;
                RaisePropertyChanged();
            }
        }
        public string? DifficultyChosen
        {
            get => _difficultyChosen;
            set
            {
                _difficultyChosen = value;
                RaisePropertyChanged();
            }
        }

        public ImportQuestionsDialogService()
        {
            Difficulties = new((string[])Enum.GetNames(typeof(Difficulty)));
        }

        public async Task<QuestionPackViewModel> ShowDialog(QuestionPackViewModel activePack)
        {
            var dialog = new ImportQuestionsDialog() { DataContext = this };

            TriviaCategories = await new TriviaCategoryGetter().Get();
       
            if (TriviaCategories != null)
            {
                TriviaCategoryChosen = TriviaCategories.FirstOrDefault();
                NumberOfQuestions = 10;
                DifficultyChosen = nameof(Difficulty.Medium);

                var result = dialog.ShowDialog();

                if (result == true)
                {
                    var statusMessage = new Dictionary<int, string>
                    {
                        { 0, "Success: Returned results successfully." },
                        { 1, "No Results: The API doesn't have enough questions for your query." },
                        { 2, "Invalid Parameter: Contains an invalid parameter." },
                        { 3, "Token Not Found: Session Token does not exist." },
                        { 4, "Token Empty: Session Token has returned all possible questions." },
                        { 5, "Rate Limit: Too many requests have occurred." }
                    };
             
                    OpenTdbApiDataReader openTdbApiDataReader = new();
                    var json = await openTdbApiDataReader.GetJsonAsString(NumberOfQuestions, TriviaCategoryChosen.id, DifficultyChosen.ToLower());

                    var options = new JsonSerializerOptions
                    {
                        Converters = { new HtmlDecodeConverter() }
                    };
                    QuestionPackDTO? QuestionPackDTO = JsonSerializer.Deserialize<QuestionPackDTO>(json, options);

                    ResultToQuestion resultToQuestion = new();
                    resultToQuestion.AddQuestionsToQuestionsPackViewModel(QuestionPackDTO, activePack);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to receive trivia categories from Open Trivia Database", "Error Occurred", MessageBoxButton.OK);
            }

            return activePack;
        }
    }
}

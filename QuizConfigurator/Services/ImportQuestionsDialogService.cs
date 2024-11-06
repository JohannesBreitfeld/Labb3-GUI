using QuizConfigurator.Dialogs;
using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAccess.OpenTdbApi;
using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;
using QuizConfigurator.ViewModel;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;

namespace QuizConfigurator.Services
{
    class ImportQuestionsDialogService : ViewModelBase, IImportQuestionsDialogService
    {
        private ObservableCollection<TriviaCategory?>? _triviaCategories;
        private int _numberOfQuestions;
        private TriviaCategory? _triviaCategoryChosen;
        private string? _difficultyChosen;
        private string? _statusMessage;

        public ObservableCollection<TriviaCategory?>? TriviaCategories
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
        public string? StatusMessage 
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }
        private Dictionary<int, string> statusMessages = new Dictionary<int, string>
                    {
                        { 0, "Success: Returned results successfully." },
                        { 1, "No Results: The API doesn't have enough questions for your query." },
                        { 2, "Invalid Parameter: Contains an invalid parameter." },
                        { 3, "Token Not Found: Session Token does not exist." },
                        { 4, "Token Empty: Session Token has returned all possible questions." },
                        { 5, "Rate Limit: Too many requests have occurred." }
                    };

        public ImportQuestionsDialogService()
        {
            Difficulties = new((string[])Enum.GetNames(typeof(Difficulty)));
        }

        public async Task<QuestionPackViewModel> ShowDialog(QuestionPackViewModel activePack)
        {
            var dialog = new ImportQuestionsDialog() { DataContext = this };
            var loadingWindow = new ApiStatusDialog() { DataContext = this };
            StatusMessage = "Loading categories from Open Trivia Database";
            loadingWindow.Show();
            
            NumberOfQuestions = 10;
            DifficultyChosen = nameof(Difficulty.Medium);

            TriviaCategories = await new TriviaCategoryGetter().Get();
            loadingWindow.Close();
            TriviaCategoryChosen = TriviaCategories.FirstOrDefault();
            
            if (TriviaCategories == null)
            {
                MessageBox.Show("Failed to receive trivia categories from Open Trivia Database", "Error Occurred", MessageBoxButton.OK);
            }
            else
            {
                var result = dialog.ShowDialog();

                if (result == true)
                {
                    await RelayAndReciveInformationFromOTDB(activePack);
                }
            }
            return activePack;
        }

        private async Task RelayAndReciveInformationFromOTDB(QuestionPackViewModel activePack)
        {
            var statusWindow = new ApiStatusDialog() { DataContext = this };
            StatusMessage = "Waiting for Open Trivia Database";
            statusWindow.Show();

            OpenTdbApiDataReader openTdbApiDataReader = new();
            var json = await openTdbApiDataReader.GetJsonAsString(NumberOfQuestions, TriviaCategoryChosen.id, DifficultyChosen.ToLower());

            if (json == string.Empty)
            {
                StatusMessage = "Error occured while retrieving the quiestions from Open Trivia Database";
            }
            else
            {
                var options = new JsonSerializerOptions { Converters = { new HtmlDecodeConverter() } };
                QuestionPackDTO? questionPackDTO = JsonSerializer.Deserialize<QuestionPackDTO>(json, options);

                if (questionPackDTO != null && statusMessages.TryGetValue(questionPackDTO.response_code, out string? message))
                {
                    StatusMessage = message;
                }
                else
                {
                    StatusMessage = "Unknown error occurred.";
                }

                ResultToQuestion resultToQuestion = new();
                resultToQuestion.AddQuestionsToQuestionsPackViewModel(questionPackDTO, activePack);
            }
        }
    }
}

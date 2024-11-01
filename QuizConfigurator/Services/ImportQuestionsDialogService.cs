using QuizConfigurator.Dialogs;
using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.Model.DataAccess.OpenTdbApi;
using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;
using QuizConfigurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace QuizConfigurator.Services
{
    class ImportQuestionsDialogService : ViewModelBase, IImportQuestionsDialogService
    {
        private ObservableCollection<TriviaCategory>? _triviaCategories;

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
        public int NumberOfQuestions { get; set; }
        public TriviaCategory TriviaCategoryChosen { get; set; }
        public string DifficultyChosen { get; set; }


        public ImportQuestionsDialogService()
        {
            Difficulties = new((string[])Enum.GetNames(typeof(Difficulty)));
        }

        private async Task<ObservableCollection<TriviaCategory>>? GetTriviaCategories()
        {
            string url = "https://opentdb.com/api_category.php";

            using HttpClient client = new();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                TriviaCategories? triviaCategories = JsonSerializer.Deserialize<TriviaCategories>(json);

                return triviaCategories != null
                    ? triviaCategories.Trivia_categories
                    : null;
            }
            return null;
        }

        public async Task<QuestionPackViewModel> ShowDialog(QuestionPackViewModel activePack)
        {
            var dialog = new ImportQuestionsDialog()
            {
                DataContext = this
            };
            TriviaCategories = await GetTriviaCategories();
            bool? result = null;

            if (TriviaCategories != null)
            {
                TriviaCategoryChosen = TriviaCategories.FirstOrDefault();
                NumberOfQuestions = 10;
                result = dialog.ShowDialog();
                
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to receive trivia categories from Open Trivia Database", "Error Occurred", MessageBoxButton.OK);
            }

            if (result == true)
            {
                OpenTdbApiDataReader openTdbApiDataReader = new();
                var call = openTdbApiDataReader.GetJsonAsString(NumberOfQuestions, TriviaCategoryChosen.Id, DifficultyChosen);


                var json = await call;
                Root? root = JsonSerializer.Deserialize<Root>(json);
                ResultToQuestion resultToQuestion = new();
                resultToQuestion.AddQuestionsToQuestionsPackViewModel(root, activePack);
            }







            return activePack;
        }
    }
}

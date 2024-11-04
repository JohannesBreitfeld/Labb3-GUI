using QuizConfigurator.Model;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi
{
    class TriviaCategoryGetter
    {
        public async Task<ObservableCollection<TriviaCategory?>>? Get()
        {
            string url = "https://opentdb.com/api_category.php";

            using HttpClient client = new();
            
            try
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    TriviaCategories? triviaCategories = JsonSerializer.Deserialize<TriviaCategories>(json);

                    return triviaCategories?.trivia_categories;
                }
            }
            catch 
            {
                return null;
            }
            return null;
        }
    }
}

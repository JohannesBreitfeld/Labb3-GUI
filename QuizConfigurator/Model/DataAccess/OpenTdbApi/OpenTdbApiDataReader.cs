using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi
{
    class OpenTdbApiDataReader
    {
        public async Task<String> GetJsonAsString(int numberOfQuestions, int categoryId, string difficulty)
        {
            using var client = new HttpClient();

            var url = $"https://opentdb.com/api.php?amount={numberOfQuestions}&category={categoryId}&difficulty={difficulty}&type=multiple";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); ;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO
{
    class Result
    {

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("difficulty")]
        public string difficulty { get; set; }

        [JsonPropertyName("category")]
        public string category { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }
    }
}

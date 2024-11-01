using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO
{
    class Result
    {
        public string? type { get; set; }
        public string? difficulty { get; set; }
        public string? category { get; set; }
        public string? question { get; set; }
        public string? correctAnswer { get; set; }
        public List<string>? incorrectAnswers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO
{
    class QuestionPackDTO
    {
        public int response_code { get; set; }
        public List<Result>? results { get; set; }
    }
}

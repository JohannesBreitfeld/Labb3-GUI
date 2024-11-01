using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO
{
    class Root
    {
        public int responseCode { get; set; }
        public List<Result>? results { get; set; }
    }
}

using QuizConfigurator.Enums;
using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;
using QuizConfigurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi
{
    class ResultToQuestion
    {
        public QuestionPackViewModel AddQuestionsToQuestionsPackViewModel(QuestionPackDTO root, QuestionPackViewModel qp)
        { 
            if (root.results != null)
            {
                foreach (var result in root.results)
                {
                    Question question = (Question)result;
                    qp.Questions.Add(question);
                }
            }
            return qp;
        }
    }
}

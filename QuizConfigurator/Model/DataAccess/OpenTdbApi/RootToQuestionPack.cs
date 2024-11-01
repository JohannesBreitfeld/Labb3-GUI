using QuizConfigurator.Enums;
using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Model.DataAccess.OpenTdbApi
{
    class RootToQuestionPack
    {
        public QuestionPack ConvertToQuestion(Root root, string name, Difficulty difficulty, int timelimit)
        {
            QuestionPack qp = new QuestionPack(name, difficulty, timelimit);

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

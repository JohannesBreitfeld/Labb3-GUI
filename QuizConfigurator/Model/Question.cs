using QuizConfigurator.Model.DataAccess.OpenTdbApi.DTO;

namespace QuizConfigurator.Model
{
    class Question
    {
        public Question(string query, string correctAnswer,
            string incorrecerAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrecerAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }
    
        public Question()
        {
            
        }
        public string? Query { get; set; }
        public string? CorrectAnswer { get; set; }
        public string[]? IncorrectAnswers { get; set; }
        
        public static explicit operator Question(Result questionDto)
        {
            string? query = questionDto.Question;
            string? correctAnswer = questionDto.CorrectAnswer;
            string? incorrectAnswer1 = questionDto.IncorrectAnswers[0];
            string? incorrectAnswer2 = questionDto.IncorrectAnswers[1];
            string? incorrectAnswer3 = questionDto.IncorrectAnswers[2];

            return new Question(query, correctAnswer, incorrectAnswer1, incorrectAnswer2, incorrectAnswer3);
        }
    }
}

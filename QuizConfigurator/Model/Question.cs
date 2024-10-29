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
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }
    }
}

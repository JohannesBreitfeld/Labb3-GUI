namespace QuizConfigurator.Model
{


    public class TriviaCategories
    {
        public List<TriviaCategory>? Trivia_categories { get; set; }
    }

    public class TriviaCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

}

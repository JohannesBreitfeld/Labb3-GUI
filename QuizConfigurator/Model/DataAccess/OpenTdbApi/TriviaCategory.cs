using System.Collections.ObjectModel;

namespace QuizConfigurator.Model
{

    public class TriviaCategories
    {
        public ObservableCollection<TriviaCategory>? Trivia_categories { get; set; }
    }

    public class TriviaCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

}

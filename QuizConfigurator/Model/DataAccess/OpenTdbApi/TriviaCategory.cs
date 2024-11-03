using System.Collections.ObjectModel;

namespace QuizConfigurator.Model
{

    public class TriviaCategories
    {
        public ObservableCollection<TriviaCategory>? trivia_categories { get; set; }
    }

    public class TriviaCategory
    {
        public int id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

}

using QuizConfigurator.Dialogs;
using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.ViewModel;
using System.Collections.ObjectModel;

namespace QuizConfigurator.Services
{
    internal class CreatePackDialogService : ICreatePackDialogService
    {
        public ObservableCollection<Difficulty> Difficulties { get; }
        public Difficulty SetDifficulty { get; set; }
        public string PackName { get; set; }
        public int TimeLimit { get; set; }

        public CreatePackDialogService()
        {
            Difficulties = new((Difficulty[])Enum.GetValues(typeof(Difficulty)));
            SetDifficulty = Difficulty.Medium;
            TimeLimit = 30;
            PackName = "<Pack Name>";
        }

        public QuestionPackViewModel? ShowDialog()
        {
            var dialog = new CreateNewPackDialog() { DataContext = this };

            QuestionPackViewModel? newPack = null;

            var result = dialog.ShowDialog();

            if (result == true)
            {
                newPack = new QuestionPackViewModel(new QuestionPack(PackName, SetDifficulty, TimeLimit));
            }
            return newPack;
        }
    }
}


using QuizConfigurator.Command;
using QuizConfigurator.Dialogs;
using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QuizConfigurator.Dialogs
{
    internal class CreatePackDialogService : ICreatePackDialogService
    {
        public ObservableCollection<Difficulty> Difficulties { get; }
        public Difficulty SetDifficulty { get; set; }
        public CreatePackDialogService()
        {
            Difficulties = new((Difficulty[])System.Enum.GetValues(typeof(Difficulty)));
            SetDifficulty = Difficulty.Medium;
        }
        public QuestionPackViewModel? ShowDialog()
        {
            var dialog = new CreateNewPackDialog()
            {
                DataContext = this
            };

            QuestionPackViewModel? newPack = null;

            var result = dialog.ShowDialog();

            if (result == true)
            {
                newPack = (new QuestionPackViewModel(
                            new QuestionPack(dialog.PackNameTextBox.Text,
                             SetDifficulty,
                             (int)dialog.TimeLimitSlider.Value)));
            }
            return newPack;
        }

    }
}
//public List<Difficulty> Difficulties { get; } = new List<Difficulty> { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard };
//private readonly MainWindowViewModel? mainWindowViewModel;
//public DelegateCommand CloseWindowCommand { get; }
//public CreateNewPackDialog dialog { get; }
//public CreatePackDialogService(MainWindowViewModel mainWindowViewModel)
//{
//    this.mainWindowViewModel = mainWindowViewModel;
//    dialog = new CreateNewPackDialog();
//    CloseWindowCommand = new(CloseWindow);
//}

//private void CloseWindow(object obj)
//{
//    dialog.Close();
//}

//public void ShowDialog()
//{
//    dialog.ShowDialog();
//}
//public void CreateNewPack()
//{
//    mainWindowViewModel?.Packs?.Add(new QuestionPackViewModel(
//        new QuestionPack(dialog.PackNameTextBox.Text,
//        Difficulty.Medium,
//        (int)dialog.TimeLimitSlider.Value)));
//    dialog.Close();
//}

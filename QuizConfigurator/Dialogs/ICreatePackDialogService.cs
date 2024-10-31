using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Dialogs
{
    internal interface ICreatePackDialogService
    {
        QuestionPackViewModel? ShowDialog();
    }
}
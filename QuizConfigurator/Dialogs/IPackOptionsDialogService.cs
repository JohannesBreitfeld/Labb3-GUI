using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Dialogs
{
    internal interface IPackOptionsDialogService
    {
        QuestionPackViewModel ShowDialog(QuestionPackViewModel activePack);
    }
}
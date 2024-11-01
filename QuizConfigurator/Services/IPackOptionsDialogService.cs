using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Services
{
    internal interface IPackOptionsDialogService
    {
        QuestionPackViewModel ShowDialog(QuestionPackViewModel activePack);
    }
}
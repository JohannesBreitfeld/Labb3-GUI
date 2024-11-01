using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Services
{
    internal interface IEditPackDialogService
    {
        QuestionPackViewModel ShowDialog(QuestionPackViewModel activePack);
    }
}
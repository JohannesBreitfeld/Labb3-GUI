using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Services
{
    internal interface IImportQuestionsDialogService
    {
        Task<QuestionPackViewModel> ShowDialog(QuestionPackViewModel activePack);
    }
}
using QuizConfigurator.ViewModel;

namespace QuizConfigurator.Services
{
    internal interface ICreatePackDialogService
    {
        QuestionPackViewModel? ShowDialog();
    }
}
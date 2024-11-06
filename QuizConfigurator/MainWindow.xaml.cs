using QuizConfigurator.Dialogs;
using QuizConfigurator.Services;
using QuizConfigurator.ViewModel;
using System.Windows;

namespace QuizConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new CreatePackDialogService());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SaveCommand.Execute(null);
            }
        }
    }
}
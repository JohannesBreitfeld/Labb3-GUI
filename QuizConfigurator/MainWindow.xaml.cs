using QuizConfigurator.Dialogs;
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
    }
}
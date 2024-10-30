using QuizConfigurator.ViewModel;
using System.Windows.Controls;

namespace QuizConfigurator.View
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is PlayerViewModel viewModel)
            {
                viewModel.StartGameCommand.Execute(null);
            }
        }
    }
}

using QuizConfigurator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel>? Packs { get; set; }
        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }
        private QuestionPackViewModel? _activePack;

		public QuestionPackViewModel? ActivePack
		{
			get  => _activePack; 
			set 
			{
				_activePack = value;
				RaisePropertyChanged();
			}
		}
		public MainWindowViewModel()
		{
			PlayerViewModel = new PlayerViewModel(this);
			ConfigurationViewModel = new ConfigurationViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("my Pack"));
        }
	}
}

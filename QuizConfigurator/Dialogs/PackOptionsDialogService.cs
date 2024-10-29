﻿using QuizConfigurator.Enums;
using QuizConfigurator.Model;
using QuizConfigurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizConfigurator.Dialogs
{
    internal class PackOptionsDialogService : IPackOptionsDialogService
    {
        public ObservableCollection<Difficulty> Difficulties { get; }
        public string PackName { get; set; }
        public Difficulty SetDifficulty { get; set; }
        public int TimeLimit { get; set; }
        public PackOptionsDialogService()
        {
            Difficulties = new((Difficulty[])System.Enum.GetValues(typeof(Difficulty)));
        }
        public QuestionPackViewModel ShowDialog(QuestionPackViewModel activePack)
        {
            PackName = activePack.Name;
            SetDifficulty = activePack.Difficulty;
            TimeLimit = activePack.TimeLimitInSeconds;

            var dialog = new PackOptionsDialog()
            {
                DataContext = this
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                activePack.Name = PackName;
                activePack.Difficulty = SetDifficulty;
                activePack.TimeLimitInSeconds = TimeLimit;
            }
            return activePack;
        }

    }
}
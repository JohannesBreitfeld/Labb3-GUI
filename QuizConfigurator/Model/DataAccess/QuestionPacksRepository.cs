﻿using QuizConfigurator.Model;
using QuizConfigurator.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using QuizConfigurator.Enums;

namespace QuizConfigurator.Model.DataAccess
{
    internal class QuestionPacksRepository
    {

        public string FilePath = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}", "QuizConfigurator", "QuestionPacks.json");

        
        public async Task<ObservableCollection<QuestionPack>?> Read()
        {
            ObservableCollection<QuestionPack>? packs = null;
            
            if (File.Exists(FilePath))
            {
                await using FileStream openStream = File.OpenRead(FilePath);
               
                packs =  await JsonSerializer.DeserializeAsync<ObservableCollection<QuestionPack>>(openStream);
            }
            return packs;
        }

        public async Task Write(ObservableCollection<QuestionPackViewModel> packs)
        {
            string? directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            await using FileStream createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, packs);
        }
    }
}

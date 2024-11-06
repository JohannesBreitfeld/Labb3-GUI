using QuizConfigurator.Model;
using QuizConfigurator.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using QuizConfigurator.Enums;

namespace QuizConfigurator.Model.DataAccess
{
    internal class QuestionPacksRepository
    {

        public string FilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\QuizConfigurator\\QuestionPacks.json";

        
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

        public async void Write(ObservableCollection<QuestionPackViewModel> packs)
        {
            await using FileStream createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, packs);
        }
    }
}

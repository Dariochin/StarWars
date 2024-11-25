using Newtonsoft.Json;
using StarWars.Models;
using StarWars.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StarWars
{
    public sealed partial class MainPage : Page
    {
        private List<StarWarsCharacter> _characters;
        private SwapiService _swapiService;

        public MainPage()
        {
            this.InitializeComponent();
            _swapiService = new SwapiService();
            LoadCharactersAsync();
        }

        private async void LoadCharactersAsync()
        {
            _characters = await _swapiService.GetCharactersAsync();
            CharacterListView.ItemsSource = _characters;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBox.Text.ToLower();
            var filteredCharacters = _characters.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.HomePlanet.Name.ToLower().Contains(searchTerm)).ToList();
            CharacterListView.ItemsSource = filteredCharacters;
        }

        private async void SaveXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var serializer = new XmlSerializer(typeof(List<StarWarsCharacter>));
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("starwars_data.xml", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                serializer.Serialize(stream.AsStreamForWrite(), _characters);
            }
        }

        private async void SaveJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(_characters);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("starwars_data.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private async void LoadXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var serializer = new XmlSerializer(typeof(List<StarWarsCharacter>));
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync("starwars_data.xml");
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                _characters = (List<StarWarsCharacter>)serializer.Deserialize(stream.AsStreamForRead());
            }
            CharacterListView.ItemsSource = _characters;
        }

        private async void LoadJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync("starwars_data.json");
            var json = await FileIO.ReadTextAsync(file);
            _characters = JsonConvert.DeserializeObject<List<StarWarsCharacter>>(json);
            CharacterListView.ItemsSource = _characters;
        }
    }
}
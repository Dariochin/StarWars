using Newtonsoft.Json;
using StarWars.Models;
using StarWars.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

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
            PerformSearch();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            CharacterListView.ItemsSource = _characters;
        }

        private void PerformSearch()
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
            ShowNotification("Data saved as XML");
        }

        private async void SaveJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(_characters);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("starwars_data.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
            ShowNotification("Data saved as JSON");
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
            ShowNotification("Data loaded from XML");
        }

        private async void LoadJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync("starwars_data.json");
            var json = await FileIO.ReadTextAsync(file);
            _characters = JsonConvert.DeserializeObject<List<StarWarsCharacter>>(json);
            CharacterListView.ItemsSource = _characters;
            ShowNotification("Data loaded from JSON");
        }

        private async void CharacterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var character = e.ClickedItem as StarWarsCharacter;
            if (character != null)
            {
                var dialog = new ContentDialog
                {
                    Title = character.Name,
                    Content = new CharacterDetailsControl(character),
                    CloseButtonText = "Close"
                };

                await dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.SetTitleBar(null);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Colors.Transparent;
            ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Colors.White;
        }

        private void ShowNotification(string message)
        {
            var notificationGrid = new Grid
            {
                Background = new SolidColorBrush(Colors.Black) { Opacity = 0.7 },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var notificationText = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 16,
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            notificationGrid.Children.Add(notificationText);
            var rootGrid = (Grid)Content;
            rootGrid.Children.Add(notificationGrid);

            var storyboard = new Storyboard();
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            Storyboard.SetTarget(fadeInAnimation, notificationGrid);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");
            storyboard.Children.Add(fadeInAnimation);

            storyboard.Begin();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                rootGrid.Children.Remove(notificationGrid);
                timer.Stop();
            };
            timer.Start();
        }
    }
}
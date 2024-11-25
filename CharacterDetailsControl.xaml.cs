using StarWars.Models;
using Windows.UI.Xaml.Controls;

namespace StarWars
{
    public sealed partial class CharacterDetailsControl : UserControl
    {
        public CharacterDetailsControl(StarWarsCharacter character)
        {
            this.InitializeComponent();
            PopulateCharacterDetails(character);
        }

        private void PopulateCharacterDetails(StarWarsCharacter character)
        {
            NameTextBlock.Text = character.Name;
            HeightTextBlock.Text = $"Height: {character.Height}";
            MassTextBlock.Text = $"Mass: {character.Mass}";
            SkinColorTextBlock.Text = $"Skin Color: {character.SkinColor}";
            BirthYearTextBlock.Text = $"Birth Year: {character.BirthYear}";
            GenderTextBlock.Text = $"Gender: {character.Gender}";


            if (character.HomePlanet != null)
            {
                PlanetNameTextBlock.Text = $"Name: {character.HomePlanet.Name}";
                PlanetGravityTextBlock.Text = $"Gravity: {character.HomePlanet.Gravity}";
                PlanetTerrainTextBlock.Text = $"Terrain: {character.HomePlanet.Terrain}";
                PlanetSurfaceWaterTextBlock.Text = $"Surface Water: {character.HomePlanet.SurfaceWater}";
                PlanetPopulationTextBlock.Text = $"Population: {character.HomePlanet.Population}";
            }

            VehiclesItemsControl.ItemsSource = character.Vehicles;
            StarshipsItemsControl.ItemsSource = character.Starships;
        }
    }
}


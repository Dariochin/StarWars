using System.Collections.Generic;

namespace StarWars.Models
{
    public class StarWarsCharacter
    {
        public string Name { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        public string SkinColor { get; set; }
        public string BirthYear { get; set; }
        public string Gender { get; set; }
        public Planet HomePlanet { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Starship> Starships { get; set; }
    }

    public class Planet
    {
        public string Name { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
        public string SurfaceWater { get; set; }
        public string Population { get; set; }
    }

    public class Vehicle
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string VehicleClass { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
    }

    public class Starship
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
        public string StarshipClass { get; set; }
    }
}


using Newtonsoft.Json.Linq;
using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StarWars.Services
{
    public class SwapiService
    {
        private readonly HttpClient _httpClient;

        public SwapiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://swapi.dev/api/");
        }

        public async Task<List<StarWarsCharacter>> GetCharactersAsync()
        {
            var characters = new List<StarWarsCharacter>();
            var response = await _httpClient.GetAsync("people");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var results = json["results"] as JArray;

                foreach (var result in results)
                {
                    var character = new StarWarsCharacter
                    {
                        Name = result["name"].ToString(),
                        Height = result["height"].ToString(),
                        Mass = result["mass"].ToString(),
                        SkinColor = result["skin_color"].ToString(),
                        BirthYear = result["birth_year"].ToString(),
                        Gender = result["gender"].ToString(),
                        HomePlanet = await GetPlanetAsync(result["homeworld"].ToString()),
                        Vehicles = await GetVehiclesAsync(result["vehicles"] as JArray),
                        Starships = await GetStarshipsAsync(result["starships"] as JArray)
                    };
                    characters.Add(character);
                }
            }
            return characters;
        }

        private async Task<Planet> GetPlanetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                return new Planet
                {
                    Name = json["name"].ToString(),
                    Gravity = json["gravity"].ToString(),
                    Terrain = json["terrain"].ToString(),
                    SurfaceWater = json["surface_water"].ToString(),
                    Population = json["population"].ToString()
                };
            }
            return null;
        }

        private async Task<List<Vehicle>> GetVehiclesAsync(JArray vehicleUrls)
        {
            var vehicles = new List<Vehicle>();
            foreach (var url in vehicleUrls)
            {
                var response = await _httpClient.GetAsync(url.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    vehicles.Add(new Vehicle
                    {
                        Name = json["name"].ToString(),
                        Model = json["model"].ToString(),
                        VehicleClass = json["vehicle_class"].ToString(),
                        MaxAtmospheringSpeed = json["max_atmosphering_speed"].ToString()
                    });
                }
            }
            return vehicles;
        }

        private async Task<List<Starship>> GetStarshipsAsync(JArray starshipUrls)
        {
            var starships = new List<Starship>();
            foreach (var url in starshipUrls)
            {
                var response = await _httpClient.GetAsync(url.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    starships.Add(new Starship
                    {
                        Name = json["name"].ToString(),
                        Model = json["model"].ToString(),
                        Manufacturer = json["manufacturer"].ToString(),
                        MaxAtmospheringSpeed = json["max_atmosphering_speed"].ToString(),
                        StarshipClass = json["starship_class"].ToString()
                    });
                }
            }
            return starships;
        }
    }
}


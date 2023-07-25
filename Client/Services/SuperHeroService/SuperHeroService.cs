using System.Net.Http.Json;
using BlazorCrudApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorCrudApp.Client.Services.SuperHeroService
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        public SuperHeroService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public List<SuperHero> Heroes { get; set; } = new List<SuperHero>();
        public List<Comic> Comics { get; set; } = new List<Comic>();

        public async Task CreateHero(SuperHero hero)
        {
            var result = await _httpClient.PostAsJsonAsync("api/superhero", hero);
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();

            Heroes = response;
            _navigationManager.NavigateTo("superheroes");
        }

        public async Task DeleteHero(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/superhero/{id}");
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();

            Heroes = response;
            _navigationManager.NavigateTo("superheroes");
        }

        public async Task GetComics()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Comic>>("api/superhero/comics");

            if (result != null)
                Comics = result;
        }

        public async Task<SuperHero> GetSuperHeroById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<SuperHero>($"api/superhero/{id}");

            if (result != null)
                return result;
            throw new Exception("Hero not found!");
        }

        public async Task GetSuperHeroes()
        {
            var result = await _httpClient.GetFromJsonAsync<List<SuperHero>>("api/superhero");

            if (result != null)
                Heroes = result;
        }

        public async Task UpdateHero(SuperHero hero)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/superhero/{hero.Id}", hero);
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();

            Heroes = response;
            _navigationManager.NavigateTo("superheroes");
        }
    }
}
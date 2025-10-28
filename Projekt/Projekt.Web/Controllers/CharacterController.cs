using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projekt.Model.ApiResponses;
using Projekt.Model.DataModels;
using Projekt.Services.ConcreteServices;

namespace Projekt.Web.Controllers
{
    public class CharacterController : BaseController
    {
        private readonly HttpClient _httpClient;
        protected readonly ICharacterService characterService;
        public CharacterController(IHttpClientFactory httpClientFactory, ICharacterService _characterService, ILogger logger, IMapper mapper, IStringLocalizer localizer)
            : base(logger,mapper,localizer)
        {
            _httpClient = httpClientFactory.CreateClient();
            characterService = _characterService;
        }

        public IActionResult Index(){
            // UWAGA. Jak będzie dodane logowanie, tutaj wrzucić userId
            var characters = characterService.GetCharacters(null);
            return View(characters);
        }

        public async Task<IActionResult> Details(int id){
            var character = characterService.GetCharacter(id);

            var proficiencies = await GetNamesByIndex(character.Proficiencies, "proficiencies");
            var traits = await GetNamesByIndex(character.Traits, "traits");
            ViewData["Proficiencies"] = proficiencies;
            ViewData["Traits"] = traits;

            return View(character);
        }

        public async Task<IList<string>> GetNamesByIndex(IList<string> indexes, string url){
            IList<string> data = new List<string>(); 
            foreach (string index in indexes)
            {
                var name = await GetNameByIndex(url + "/" + index);
                data.Add(name);
            }
            return data;
        }

        public async Task<string> GetNameByIndex(string url){
            var response = await _httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/2014/{url}");
            using var doc = JsonDocument.Parse(response);
            return doc.RootElement.GetProperty("name").GetString();
        }

        public async Task<IActionResult> Add(){

            var raceResponse = await _httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/2014/races");
            using var raceDoc = JsonDocument.Parse(raceResponse);
            
            var classResponse = await _httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/2014/classes");
            using var classDoc = JsonDocument.Parse(classResponse);

            var aligmentResponse = await _httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/2014/alignments");
            using var alignmentDoc = JsonDocument.Parse(aligmentResponse);

            var races = raceDoc.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Select(r => new SelectListItem
            {
                Text = r.GetProperty("name").GetString(),
                Value = r.GetProperty("index").GetString()
            })
            .ToList();

            var classes = classDoc.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Select(c => new SelectListItem
            {
                Text = c.GetProperty("name").GetString(),
                Value = c.GetProperty("index").GetString()
            })
            .ToList();

            var alignments = alignmentDoc.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Select(c => new SelectListItem
            {
                Text = c.GetProperty("name").GetString(),
                Value = c.GetProperty("index").GetString()
            })
            .ToList();
            
            ViewData["Races"] = races;
            ViewData["Classes"] = classes;
            ViewData["Alignments"] = alignments;

            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Character character){
            var className = character.Class;
            var raceName = character.Race;

            var raceUrl = $"https://www.dnd5eapi.co/api/2014/races/{raceName}/";
            var raceResponse = await _httpClient.GetStringAsync(raceUrl);
            using var raceDoc = JsonDocument.Parse(raceResponse);

            character.Traits = new List<string>();
            if (raceDoc.RootElement.TryGetProperty("traits", out var traitsArray))
            {
                foreach (var trait in traitsArray.EnumerateArray())
                {
                    var index = trait.GetProperty("index").GetString();
                    character.Traits.Add(index);
                }
            }

            int speed = raceDoc.RootElement.GetProperty("speed").GetInt32();
            character.Speed = speed;

            var classResponse = await _httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/2014/classes/{className}/");
            using var doc2 = JsonDocument.Parse(classResponse);
            int maxHP = doc2.RootElement.GetProperty("hit_die").GetInt32() + (int)((character.Constitution - 10) / 2);
            character.MaxHP = maxHP;
            character.CurrentHP = maxHP;
            character.TemporaryHP = 0;

            if (doc2.RootElement.TryGetProperty("proficiencies", out var proficienciesArray))
            {
                foreach (var proficiency in proficienciesArray.EnumerateArray())
                {
                    var index = proficiency.GetProperty("index").GetString();
                    character.Proficiencies.Add(index);
                }
            }
            
            character.ArmorClass = 10 + (int)((character.Dexterity - 10) / 2); 
            characterService.SaveCharacter(character);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetProficiencies(string className)
        {
            if (string.IsNullOrEmpty(className))
                return BadRequest("Missing class name");
            
            var endpoint = $"https://www.dnd5eapi.co/api/2014/classes/{className}";
            var result = await _httpClient.GetFromJsonAsync<DndClassProficiencyResponse>(endpoint);

            if (result?.Proficiency_Choices == null)
                return NotFound();

            var profOptions = result.Proficiency_Choices
                .SelectMany(pc => pc.From.Options)
                .Select(o => new SelectListItem
                {
                    Text = o.Item.Name,
                    Value = o.Item.Index
                })
                .ToList();

            return Json(profOptions);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpells(string className)
        {
            if (string.IsNullOrEmpty(className))
                return BadRequest("Missing class name");

            var endpoint = $"https://www.dnd5eapi.co/api/2014/classes/{className}/spells";
            var result = await _httpClient.GetFromJsonAsync<DndClassSpellResponse>(endpoint);

            if (result?.Spells == null)
                return NotFound();

            var spells = result.Spells
                .SelectMany(pc => pc.Results)
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Index
                })
                .ToList();

            return Json(spells);
        }
    }
}

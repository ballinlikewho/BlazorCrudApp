using BlazorCrudApp.Server.Data;
using BlazorCrudApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrudApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var superHeroes = await _dataContext.SuperHeroes.Include(h => h.Comic).ToListAsync();
            return Ok(superHeroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
        {
            var hero = await _dataContext.SuperHeroes
            .Include(h => h.Comic)
            .FirstOrDefaultAsync(h => h.Id == id);

            if(hero != null)
            {
                return Ok(hero);
            }
            else
            {
                return NotFound("Could not find hero by id.");
            }
        }

        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _dataContext.Comics.ToListAsync();
            return Ok(comics);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
        {
            var dbHero = await _dataContext.SuperHeroes.Include(h =>  h.Comic).FirstOrDefaultAsync(h => h.Id == id);

            if(dbHero == null)
                return NotFound($"Could not update hero. Id: {id}");
            
            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _dataContext.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _dataContext.SuperHeroes.Include(h =>  h.Comic).FirstOrDefaultAsync(h => h.Id == id);

            if(dbHero == null)
                return NotFound($"Could not delete hero. Id: {id}");
            
            _dataContext.SuperHeroes.Remove(dbHero);

            await _dataContext.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _dataContext.SuperHeroes.Include(h => h.Comic).ToListAsync();
        }
    }
}
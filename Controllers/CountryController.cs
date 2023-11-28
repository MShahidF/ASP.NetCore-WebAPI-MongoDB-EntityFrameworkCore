using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Models;
using ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Context;

namespace ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : Controller
    {
        private readonly MongoDbContext _mongoDbContext;

        public CountryController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;     
        }

        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                var countries = await _mongoDbContext.Countries.ToListAsync();
                var modifiedCountries = countries.Select(p => new
                {
                    id = p.CountryId.ToString(),
                    p.CountryName,
                    p.CountryCode
                });

                return Results.Ok(modifiedCountries);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }    
        }

        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objectId)) 
                {
                    var country = await _mongoDbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == objectId);

                    if (country is null)
                    {
                        return Results.NotFound($"Country with id = {id} is not found");
                    }

                    var modifiedCountry = new
                    {
                        id,
                        country.CountryName,
                        country.CountryCode
                    };

                    return Results.Ok(modifiedCountry);
                }

                return Results.BadRequest($"Id = {id} is inavlid");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> Create(Country country)
        {
            try
            {
                await _mongoDbContext.Countries.AddAsync(country);
                await _mongoDbContext.SaveChangesAsync();

                return Results.Ok(country);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out ObjectId objectId))
                {
                    var country = await _mongoDbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == objectId);

                    if (country is null)
                    {
                        return Results.NotFound($"Country with id = {id} is not found");
                    }

                    _mongoDbContext.Countries.Remove(country);
                    await _mongoDbContext.SaveChangesAsync();

                    return Results.Ok($"Removed Country with id = {id} successfully");
                }

                return Results.BadRequest($"Id = {id} is inavlid");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update(string id, Country country)
        {
            try
            {
                if (ObjectId.TryParse(id, out ObjectId objectId))
                {
                    var existingCountry = await _mongoDbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == objectId);

                    if (existingCountry is null)
                    {
                        return Results.NotFound($"Country with id = {id} is not found");
                    }

                    existingCountry.CountryName = country.CountryName;
                    existingCountry.CountryCode = country.CountryCode;

                    _mongoDbContext.Countries.Update(existingCountry);
                    await _mongoDbContext.SaveChangesAsync();

                    var modifiedCountry = new
                    {
                        id,
                        existingCountry.CountryName,
                        existingCountry.CountryCode
                    };

                    return Results.Ok(modifiedCountry);
                }

                return Results.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IResult> Patch(string id, JsonPatchDocument<Country> jsonPatchDocument)
        {
            try
            {
                if (ObjectId.TryParse(id, out ObjectId objectId))
                {
                    var existingCountry = await _mongoDbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == objectId);

                    if (existingCountry is null)
                    {
                        return Results.NotFound($"Country with id = {id} is not found");
                    }

                    jsonPatchDocument.ApplyTo(existingCountry);

                    _mongoDbContext.Update(existingCountry);
                    await _mongoDbContext.SaveChangesAsync();

                    var modifiedCountry = new
                    {
                        id,
                        existingCountry.CountryName,
                        existingCountry.CountryCode
                    };

                    return Results.Ok(modifiedCountry);
                }

                return Results.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}

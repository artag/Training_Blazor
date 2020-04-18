using BethanysPieShopHRM.Shared.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _repository;

        public CountryController(ICountryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            return Ok(_repository.GetAllCountries());
        }

        // GET: api/<controller>/<id>
        [HttpGet("{id}")]
        public IActionResult GetCountry(int id)
        {
            return Ok(_repository.GetCountry(id));
        }
    }
}

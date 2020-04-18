using BethanysPieShopHRM.Shared.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryRepository _repository;

        public JobCategoryController(IJobCategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetAllJobCategories()
        {
            return Ok(_repository.GetAllJobCategories());
        }

        // GET: api/<controller>/<id>
        [HttpGet("{id}")]
        public IActionResult GetJobCategory(int id)
        {
            return Ok(_repository.GetJobCategory(id));
        }
    }
}

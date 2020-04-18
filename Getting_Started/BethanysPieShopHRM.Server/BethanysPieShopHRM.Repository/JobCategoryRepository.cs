using System.Collections.Generic;
using System.Linq;
using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.Shared.Repository;

namespace BethanysPieShopHRM.Repository
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly AppDbContext _context;

        public JobCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<JobCategory> GetAllJobCategories()
        {
            return _context.JobCategories;
        }

        public JobCategory GetJobCategory(int id)
        {
            return _context.JobCategories.FirstOrDefault(c => c.JobCategoryId == id);
        }
    }
}

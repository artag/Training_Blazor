using System.Collections.Generic;

namespace BethanysPieShopHRM.Shared.Repository
{
    public interface IJobCategoryRepository
    {
        IEnumerable<JobCategory> GetAllJobCategories();
        JobCategory GetJobCategory(int id);
    }
}

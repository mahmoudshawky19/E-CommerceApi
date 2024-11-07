using E_CommerceApi.Data;
using E_CommerceApi.Models;
using E_CommerceApi.Repositery;
using E_CommerceApi.Repositery.InterfaceCategory;
 
namespace E_CommerceApi.Repository
{
    public class CategoryRepository : Repositery<Category>, ICategoryRepositery
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        // any additional logic
    }
}
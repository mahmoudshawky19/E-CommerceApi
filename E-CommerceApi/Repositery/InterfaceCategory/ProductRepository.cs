using E_CommerceApi.Data;
using E_CommerceApi.Models;

namespace E_CommerceApi.Repositery.InterfaceCategory
{
    public class ProductRepository : Repositery<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}

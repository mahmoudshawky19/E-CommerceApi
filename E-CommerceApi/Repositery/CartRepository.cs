using E_CommerceApi.Data;
using E_CommerceApi.Models;
using E_CommerceApi.Repositery.InterfaceCategory;

namespace E_CommerceApi.Repositery
{
    public class CartRepository : Repositery<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}

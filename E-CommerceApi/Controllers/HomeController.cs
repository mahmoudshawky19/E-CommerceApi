using E_CommerceApi.Repositery.InterfaceCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public HomeController(  IProductRepository productRepository)
        {
        this.productRepository = productRepository;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {

            var products = productRepository.GetAll();
            return Ok(  products);
        }
        [HttpGet]
        [Route("Details")]

        public IActionResult Details(int productId)
        {

            var product = productRepository.GetOne([] , e=>e.Id ==productId);
            if(product == null ) 
            return Ok(product);
            return NotFound(); 
        }

    }
}

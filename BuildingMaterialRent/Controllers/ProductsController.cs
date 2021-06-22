using BuildingMaterialRent.Data;
using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMaterialRent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly RentDbContext _context;

        public ProductsController(RentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return _context.Products.ToList();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Product Get(int id)
        {
            return _context.Products.SingleOrDefault(p => p.ProductId == id);
        }
    }
}

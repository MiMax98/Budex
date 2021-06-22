using BuildingMaterialRent.Data;
using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMaterialRent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly RentDbContext _context;
        public CategoriesController(RentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Category> Get()
        {
            return _context.Categories.ToList();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Category Get(int id)
        {
            var category = _context
                .Categories
                .Include(c => c.Products)
                .SingleOrDefault(c => c.CategoryId == id);
            return category;
        }
    }
}

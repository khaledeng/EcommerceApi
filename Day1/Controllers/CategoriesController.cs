using Day1.data;
using Day1.DTOs;
using Day1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            return _context.Categories.ToList();
        }


        [HttpGet("{id}")]

        public ActionResult<Category> GetById(int id)
        {
            var cat= _context.Categories.Find(id);
            if(cat==null)
            {
                return NotFound();
            }
            return cat;
        }

        [HttpPost]

        public ActionResult Create(CategoryCreateDto dto)
        {
            var Category = new Category
            {
                Name = dto.name
            };
            _context.Categories.Add(Category);
            _context.SaveChanges();


            return CreatedAtAction(nameof(GetById), new { id = Category.Id }, Category);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var cat=_context.Categories.Find(id);
            if (cat == null)
                return Content("No Cat To Delete");
            _context.Categories.Remove(cat);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCat(int id,CategoryCreateDto dto)
        {

            var Cat = _context.Categories.Find(id);

            if(Cat == null)
                return BadRequest();
            Cat.Name = dto.name;
            _context.SaveChanges();
            return NoContent();

        }
    }
}

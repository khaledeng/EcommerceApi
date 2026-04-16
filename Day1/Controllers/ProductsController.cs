using AutoMapper;
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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        IMapper _mapper;
        public ProductsController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<List<ReadProductDto>> GetAll()
        {
                 var list=_context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();
            //List<ProductCreateDto> products = new List<ProductCreateDto>();

            var products = _mapper.Map<List<ReadProductDto>>(list);

            return Ok(products);

        }

        [HttpGet("{id}")]
        public ActionResult<ReadProductDto> GetById(int id)
        {
            var prd = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == id);
            if (prd == null)
                return NotFound();
            var prddto = _mapper.Map<ReadProductDto>(prd);
                
             return Ok(prddto);
        }



        [HttpPost]
        public ActionResult Create(ProductCreateDto dto)
        {

            var prds = _mapper.Map<Product>(dto);
            _context.Products.Add(prds);
            _context.SaveChanges();
            var result = _mapper.Map<ReadProductDto>(prds);

            return CreatedAtAction(nameof(GetById), new { id = prds.Id }, result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var prd = _context.Products.Find(id);
            if (prd == null)
            {
                return Content("Not Found");
            }
            _context.Products.Remove(prd);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id,ProductCreateDto dto)
        {
            var prd = _context.Products.Find(id);

            if (prd == null)
                return BadRequest();

            _mapper.Map(dto,prd);
            _context.SaveChanges();
            return NoContent();
                

        }


    }
}

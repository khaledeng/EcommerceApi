using AutoMapper;
using Day1.data;
using Day1.DTOs;
using Day1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
            private readonly AppDbContext _context;
             IMapper _mapper;



        public SuppliersController(AppDbContext context,IMapper mapper)
            {
                _context = context;
                 _mapper = mapper;
            }

            [HttpGet]
            public ActionResult<List<ReadSupplierDto>> GetAll()
            {
                var list= _context.Suppliers.ToList();
                
                var suppliers = _mapper.Map<List<ReadSupplierDto>>(list);
          

                return Ok(suppliers);
            }


            [HttpGet("{id}")]

            public ActionResult<Supplier> GetById(int id)
            {
                var sup = _context.Suppliers.Find(id);
                if (sup == null)
                {
                    return NotFound();
                }
            var supDto = _mapper.Map<ReadSupplierDto>(sup);
                    
                return Ok(supDto);
            }

            [HttpPost]

            public ActionResult Create(SupplierCreateDto dto)
            {
            var sup = _mapper.Map<Supplier>(dto);
                _context.Suppliers.Add(sup);
                _context.SaveChanges();

                return Created();
            }

            [HttpDelete("{id}")]
            public ActionResult Delete(int id)
            {
                var sup = _context.Suppliers.Find(id);
                if (sup == null)
                    return Content("No Cat To Delete");
                _context.Suppliers.Remove(sup);
                _context.SaveChanges();
                return NoContent();
            }
        }
    }

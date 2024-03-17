using CLOUDPART_Test.Data;
using CLOUDPART_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata;

namespace CLOUDPART_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("getbyid/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p=>p.Id == Id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        [HttpGet("{productName}")]
        public async Task<IActionResult> GetProduct([FromRoute] string productName)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(productName))
                .ToListAsync();
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { Id = product.Id }, product);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProduct([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if(!_context.Products.Any(p=>p.Id == product.Id))
            {
                return NotFound();
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

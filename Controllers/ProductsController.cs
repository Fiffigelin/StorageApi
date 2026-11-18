using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.DTOs;
using StorageApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageApiContext _context;
    public ProductsController(StorageApiContext context)
    {
        _context = context;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var data = await _context.Product.ToListAsync();

        /**
        * Den utkommenterade raden är min egen.
        * Raden under är vad IDE rekommenderade med intellisensen 
        */
        // var productsDto = data.Select(d => ProductToDto(d)).ToList();
        var productsDto = data.Select(ProductToDto).ToList();
        return productsDto;
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return ProductToDto(product);
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, CreateProductDto dto)
    {
        var data = await _context.Product.FindAsync(id);

        if (data is null)
        {
            return NotFound("Produkten kunde inte hittas.");
        }

        // helt glömt bort detta med att EF kör med Change Tracking.
        data.Name = dto.Name;
        data.Price = dto.Price;
        data.Category = dto.Category;
        data.Shelf = dto.Shelf;
        data.Count = dto.Count;
        data.Description = dto.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Product
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto dto)
    {
        var product = DtoToProduct(dto);

        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        var result = ProductToDto(product);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            result
        );
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? id)
    {
        return _context.Product.Any(e => e.Id == id);
    }

    private static Product DtoToProduct(CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            Shelf = dto.Shelf,
            Count = dto.Count,
            Description = dto.Description
        };
    }

    private static ProductDto ProductToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Count = product.Count
        };
    }
}

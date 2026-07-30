using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SanphamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SanphamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ByKhohang/{khohangId}")]
    public async Task<ActionResult<IEnumerable<Sanpham>>> GetSanphamsByKhohang(int khohangId)
    {
        return await _context.Sanphams
            .Where(s => s.KhohangId == khohangId)
            .OrderByDescending(s => s.Id)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sanpham>> GetSanpham(int id)
    {
        var sanpham = await _context.Sanphams.FindAsync(id);

        if (sanpham == null)
        {
            return NotFound();
        }

        return sanpham;
    }

    [HttpPost]
    public async Task<ActionResult<Sanpham>> PostSanpham(Sanpham sanpham)
    {
        bool exists = await _context.Sanphams.AnyAsync(s => s.KhohangId == sanpham.KhohangId && s.MaSanPham == sanpham.MaSanPham);
        if (exists)
        {
            return BadRequest(new { message = "Mã sản phẩm đã tồn tại trong kho này." });
        }

        _context.Sanphams.Add(sanpham);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSanpham), new { id = sanpham.Id }, sanpham);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSanpham(int id, Sanpham sanpham)
    {
        if (id != sanpham.Id)
        {
            return BadRequest();
        }

        bool exists = await _context.Sanphams.AnyAsync(s => s.KhohangId == sanpham.KhohangId && s.MaSanPham == sanpham.MaSanPham && s.Id != id);
        if (exists)
        {
            return BadRequest(new { message = "Mã sản phẩm đã tồn tại trong kho này." });
        }

        _context.Entry(sanpham).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SanphamExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSanpham(int id)
    {
        var sanpham = await _context.Sanphams.FindAsync(id);
        if (sanpham == null)
        {
            return NotFound();
        }

        _context.Sanphams.Remove(sanpham);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SanphamExists(int id)
    {
        return _context.Sanphams.Any(e => e.Id == id);
    }
}

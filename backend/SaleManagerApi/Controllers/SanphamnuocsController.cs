using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SanphamnuocsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SanphamnuocsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ByKhohang/{khohangId}")]
    public async Task<ActionResult<IEnumerable<Sanphamnuoc>>> GetSanphamnuocsByKhohang(int khohangId)
    {
        return await _context.Sanphamnuocs
            .Where(s => s.KhohangId == khohangId)
            .OrderByDescending(s => s.Id)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sanphamnuoc>> GetSanphamnuoc(int id)
    {
        var sanphamnuoc = await _context.Sanphamnuocs.FindAsync(id);

        if (sanphamnuoc == null)
        {
            return NotFound();
        }

        return sanphamnuoc;
    }

    [HttpPost]
    public async Task<ActionResult<Sanphamnuoc>> PostSanphamnuoc(Sanphamnuoc sanphamnuoc)
    {
        bool exists = await _context.Sanphamnuocs.AnyAsync(s => s.KhohangId == sanphamnuoc.KhohangId && s.MaSanPham == sanphamnuoc.MaSanPham);
        if (exists)
        {
            return BadRequest(new { message = "Mã sản phẩm đã tồn tại trong kho này." });
        }

        _context.Sanphamnuocs.Add(sanphamnuoc);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSanphamnuoc), new { id = sanphamnuoc.Id }, sanphamnuoc);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSanphamnuoc(int id, Sanphamnuoc sanphamnuoc)
    {
        if (id != sanphamnuoc.Id)
        {
            return BadRequest();
        }

        bool exists = await _context.Sanphamnuocs.AnyAsync(s => s.KhohangId == sanphamnuoc.KhohangId && s.MaSanPham == sanphamnuoc.MaSanPham && s.Id != id);
        if (exists)
        {
            return BadRequest(new { message = "Mã sản phẩm đã tồn tại trong kho này." });
        }

        _context.Entry(sanphamnuoc).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SanphamnuocExists(id))
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
    public async Task<IActionResult> DeleteSanphamnuoc(int id)
    {
        var sanphamnuoc = await _context.Sanphamnuocs.FindAsync(id);
        if (sanphamnuoc == null)
        {
            return NotFound();
        }

        _context.Sanphamnuocs.Remove(sanphamnuoc);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SanphamnuocExists(int id)
    {
        return _context.Sanphamnuocs.Any(e => e.Id == id);
    }
}

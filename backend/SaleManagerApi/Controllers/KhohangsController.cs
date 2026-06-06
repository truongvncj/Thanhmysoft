using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KhohangsController : ControllerBase
{
    private readonly AppDbContext _context;

    public KhohangsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Khohang>>> GetKhohangs([FromQuery] bool? onlyOpen = false)
    {
        var query = _context.Khohangs.AsQueryable();

        if (onlyOpen == true)
        {
            query = query.Where(k => !k.IsClosed);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Khohang>> GetKhohang(int id)
    {
        var khohang = await _context.Khohangs.FindAsync(id);

        if (khohang == null)
        {
            return NotFound();
        }

        return khohang;
    }

    [HttpPost]
    public async Task<ActionResult<Khohang>> PostKhohang(Khohang khohang)
    {
        _context.Khohangs.Add(khohang);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetKhohang), new { id = khohang.Id }, khohang);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutKhohang(int id, Khohang khohang)
    {
        if (id != khohang.Id)
        {
            return BadRequest();
        }

        _context.Entry(khohang).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!KhohangExists(id))
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
    public async Task<IActionResult> DeleteKhohang(int id)
    {
        var khohang = await _context.Khohangs.FindAsync(id);
        if (khohang == null)
        {
            return NotFound();
        }

        _context.Khohangs.Remove(khohang);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool KhohangExists(int id)
    {
        return _context.Khohangs.Any(e => e.Id == id);
    }
}

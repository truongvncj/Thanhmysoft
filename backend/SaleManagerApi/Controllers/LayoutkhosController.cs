using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LayoutkhosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LayoutkhosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Layoutkho>>> GetLayoutkhos([FromQuery] int? khohangId)
    {
        var query = _context.Layoutkhos.AsQueryable();

        if (khohangId.HasValue)
        {
            query = query.Where(l => l.KhohangId == khohangId.Value);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Layoutkho>> GetLayoutkho(int id)
    {
        var layoutkho = await _context.Layoutkhos.FindAsync(id);

        if (layoutkho == null)
        {
            return NotFound();
        }

        return layoutkho;
    }

    [HttpPost]
    public async Task<ActionResult<Layoutkho>> PostLayoutkho(Layoutkho layoutkho)
    {
        _context.Layoutkhos.Add(layoutkho);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLayoutkho), new { id = layoutkho.Id }, layoutkho);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLayoutkho(int id, Layoutkho layoutkho)
    {
        if (id != layoutkho.Id)
        {
            return BadRequest();
        }

        _context.Entry(layoutkho).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LayoutkhoExists(id))
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
    public async Task<IActionResult> DeleteLayoutkho(int id)
    {
        var layoutkho = await _context.Layoutkhos.FindAsync(id);
        if (layoutkho == null)
        {
            return NotFound();
        }

        _context.Layoutkhos.Remove(layoutkho);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LayoutkhoExists(int id)
    {
        return _context.Layoutkhos.Any(e => e.Id == id);
    }
}

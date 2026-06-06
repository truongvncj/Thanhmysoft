using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NhaVanTaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public NhaVanTaisController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NhaVanTai>>> GetNhaVanTais()
    {
        return await _context.NhaVanTais.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NhaVanTai>> GetNhaVanTai(string id)
    {
        var nvt = await _context.NhaVanTais.FindAsync(id);

        if (nvt == null)
        {
            return NotFound();
        }

        return nvt;
    }

    [HttpPost]
    public async Task<ActionResult<NhaVanTai>> PostNhaVanTai(NhaVanTai nvt)
    {
        // Check if exists
        if (NhaVanTaiExists(nvt.MaNvt))
        {
            return Conflict(new { message = "Mã Nhà Vận Tải đã tồn tại" });
        }
        
        _context.NhaVanTais.Add(nvt);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNhaVanTai), new { id = nvt.MaNvt }, nvt);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutNhaVanTai(string id, NhaVanTai nvt)
    {
        if (id != nvt.MaNvt)
        {
            return BadRequest();
        }

        _context.Entry(nvt).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NhaVanTaiExists(id))
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
    public async Task<IActionResult> DeleteNhaVanTai(string id)
    {
        var nvt = await _context.NhaVanTais.FindAsync(id);
        if (nvt == null)
        {
            return NotFound();
        }

        _context.NhaVanTais.Remove(nvt);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool NhaVanTaiExists(string id)
    {
        return _context.NhaVanTais.Any(e => e.MaNvt == id);
    }
}

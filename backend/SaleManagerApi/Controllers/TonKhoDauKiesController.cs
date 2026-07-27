using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TonKhoDauKiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TonKhoDauKiesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/TonKhoDauKies/ByKhohang/{khohangId}
    [HttpGet("ByKhohang/{khohangId}")]
    public async Task<ActionResult<IEnumerable<TonKhoDauKy>>> GetTonKhoByKhohang(int khohangId)
    {
        return await _context.TonKhoDauKies
            .Where(t => t.KhohangId == khohangId)
            .OrderBy(t => t.DayHang)
            .ThenBy(t => t.SttDayHang)
            .ToListAsync();
    }

    // GET: api/TonKhoDauKies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TonKhoDauKy>> GetTonKhoDauKy(int id)
    {
        var tonKhoDauKy = await _context.TonKhoDauKies.FindAsync(id);

        if (tonKhoDauKy == null)
        {
            return NotFound();
        }

        return tonKhoDauKy;
    }

    // PUT: api/TonKhoDauKies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTonKhoDauKy(int id, TonKhoDauKy tonKhoDauKy)
    {
        if (id != tonKhoDauKy.Id)
        {
            return BadRequest();
        }

        if (tonKhoDauKy.HanSuDung.HasValue)
        {
            tonKhoDauKy.HanSuDung = tonKhoDauKy.HanSuDung.Value.ToUniversalTime();
        }
        if (tonKhoDauKy.NgaySanXuat.HasValue)
        {
            tonKhoDauKy.NgaySanXuat = tonKhoDauKy.NgaySanXuat.Value.ToUniversalTime();
        }
        if (tonKhoDauKy.ThoiDiemGhiNhan.HasValue)
        {
            tonKhoDauKy.ThoiDiemGhiNhan = tonKhoDauKy.ThoiDiemGhiNhan.Value.ToUniversalTime();
        }

        _context.Entry(tonKhoDauKy).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TonKhoDauKyExists(id))
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

    // POST: api/TonKhoDauKies
    [HttpPost]
    public async Task<ActionResult<TonKhoDauKy>> PostTonKhoDauKy(TonKhoDauKy tonKhoDauKy)
    {
        // Set UTC dates if needed
        if (tonKhoDauKy.HanSuDung.HasValue)
        {
            tonKhoDauKy.HanSuDung = tonKhoDauKy.HanSuDung.Value.ToUniversalTime();
        }
        if (tonKhoDauKy.NgaySanXuat.HasValue)
        {
            tonKhoDauKy.NgaySanXuat = tonKhoDauKy.NgaySanXuat.Value.ToUniversalTime();
        }
        if (tonKhoDauKy.ThoiDiemGhiNhan.HasValue)
        {
            tonKhoDauKy.ThoiDiemGhiNhan = tonKhoDauKy.ThoiDiemGhiNhan.Value.ToUniversalTime();
        }

        _context.TonKhoDauKies.Add(tonKhoDauKy);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTonKhoDauKy", new { id = tonKhoDauKy.Id }, tonKhoDauKy);
    }

    // DELETE: api/TonKhoDauKies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTonKhoDauKy(int id)
    {
        var tonKhoDauKy = await _context.TonKhoDauKies.FindAsync(id);
        if (tonKhoDauKy == null)
        {
            return NotFound();
        }

        _context.TonKhoDauKies.Remove(tonKhoDauKy);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TonKhoDauKyExists(int id)
    {
        return _context.TonKhoDauKies.Any(e => e.Id == id);
    }
}

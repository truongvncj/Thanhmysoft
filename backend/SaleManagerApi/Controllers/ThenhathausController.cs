using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThenhathausController : ControllerBase
{
    private readonly AppDbContext _context;

    public ThenhathausController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Thenhathau>>> GetThenhathaus([FromQuery] string? maNvt)
    {
        var query = _context.Thenhathaus.AsQueryable();
        
        if (!string.IsNullOrEmpty(maNvt))
        {
            query = query.Where(t => t.MaNvt == maNvt);
        }
        
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Thenhathau>> GetThenhathau(string id)
    {
        var thenhathau = await _context.Thenhathaus.FindAsync(id);

        if (thenhathau == null)
        {
            return NotFound();
        }

        return thenhathau;
    }

    [HttpPost]
    public async Task<ActionResult<Thenhathau>> PostThenhathau(Thenhathau thenhathau)
    {
        if (string.IsNullOrEmpty(thenhathau.MaLx))
        {
            thenhathau.MaLx = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); // Short random ID
        }

        if (ThenhathauExists(thenhathau.MaLx))
        {
            return Conflict(new { message = "Mã lái xe (thẻ) đã tồn tại" });
        }
        
        thenhathau.Sothe = thenhathau.Sothe?.Trim();

        if (_context.Thenhathaus.Any(t => t.Sothe.Trim() == thenhathau.Sothe))
        {
            return Conflict(new { message = "Số thẻ này đã được đăng ký" });
        }

        var nvt = await _context.NhaVanTais.FirstOrDefaultAsync(n => n.MaNvt == thenhathau.MaNvt);
        thenhathau.TenNvt = nvt != null ? nvt.TenNvt : "";

        // Fix PostgreSQL DateTime kind issue
        thenhathau.Thoihanthe = DateTime.SpecifyKind(thenhathau.Thoihanthe, DateTimeKind.Utc);

        _context.Thenhathaus.Add(thenhathau);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetThenhathau), new { id = thenhathau.MaLx }, thenhathau);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutThenhathau(string id, Thenhathau thenhathau)
    {
        if (id != thenhathau.MaLx)
        {
            return BadRequest();
        }

        thenhathau.Sothe = thenhathau.Sothe?.Trim();

        if (_context.Thenhathaus.Any(t => t.Sothe.Trim() == thenhathau.Sothe && t.MaLx != thenhathau.MaLx))
        {
            return Conflict(new { message = "Số thẻ này đã được đăng ký" });
        }

        var nvt = await _context.NhaVanTais.FirstOrDefaultAsync(n => n.MaNvt == thenhathau.MaNvt);
        thenhathau.TenNvt = nvt != null ? nvt.TenNvt : "";

        // Fix PostgreSQL DateTime kind issue
        thenhathau.Thoihanthe = DateTime.SpecifyKind(thenhathau.Thoihanthe, DateTimeKind.Utc);

        _context.Entry(thenhathau).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ThenhathauExists(id))
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
    public async Task<IActionResult> DeleteThenhathau(string id)
    {
        var thenhathau = await _context.Thenhathaus.FindAsync(id);
        if (thenhathau == null)
        {
            return NotFound();
        }

        _context.Thenhathaus.Remove(thenhathau);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    public class ChangeStatusRequest
    {
        public int TrangThai { get; set; }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeStatusRequest request)
    {
        var thenhathau = await _context.Thenhathaus.FindAsync(id);
        if (thenhathau == null) return NotFound();

        thenhathau.TrangThai = request.TrangThai;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật trạng thái thành công" });
    }

    public class ExtendCardRequest
    {
        public DateTime NewDate { get; set; }
    }

    [HttpPut("{id}/extend")]
    public async Task<IActionResult> ExtendCard(string id, [FromBody] ExtendCardRequest request)
    {
        var thenhathau = await _context.Thenhathaus.FindAsync(id);
        if (thenhathau == null) return NotFound();

        thenhathau.Thoihanthe = DateTime.SpecifyKind(request.NewDate, DateTimeKind.Utc);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Gia hạn thẻ thành công" });
    }

    private bool ThenhathauExists(string id)
    {
        return _context.Thenhathaus.Any(e => e.MaLx == id);
    }
}

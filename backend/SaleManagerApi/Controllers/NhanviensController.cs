using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NhanviensController : ControllerBase
{
    private readonly AppDbContext _context;

    public NhanviensController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Nhanvien>>> GetNhanviens([FromQuery] string? vitri)
    {
        var query = _context.Nhanviens.AsQueryable();

        if (!string.IsNullOrEmpty(vitri))
        {
            query = query.Where(n => n.Vitri == vitri);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Nhanvien>> GetNhanvien(string id)
    {
        var nhanvien = await _context.Nhanviens.FindAsync(id);

        if (nhanvien == null)
        {
            return NotFound();
        }

        return nhanvien;
    }

    [HttpPost]
    public async Task<ActionResult<Nhanvien>> PostNhanvien(Nhanvien nhanvien)
    {
        if (await _context.Nhanviens.AnyAsync(n => n.Mnv.ToLower() == nhanvien.Mnv.ToLower()))
        {
            return BadRequest(new { message = "Mã nhân viên/bảo vệ này đã tồn tại trong hệ thống. Vui lòng nhập mã khác!" });
        }

        _context.Nhanviens.Add(nhanvien);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNhanvien), new { id = nhanvien.Mnv }, nhanvien);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutNhanvien(string id, Nhanvien nhanvien)
    {
        if (id != nhanvien.Mnv)
        {
            return BadRequest(new { message = "Mã nhân viên không khớp." });
        }

        _context.Entry(nhanvien).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NhanvienExists(id))
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
    public async Task<IActionResult> DeleteNhanvien(string id)
    {
        var nhanvien = await _context.Nhanviens.FindAsync(id);
        if (nhanvien == null)
        {
            return NotFound();
        }

        _context.Nhanviens.Remove(nhanvien);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool NhanvienExists(string id)
    {
        return _context.Nhanviens.Any(e => e.Mnv == id);
    }
}

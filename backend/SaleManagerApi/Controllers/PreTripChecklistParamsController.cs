using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreTripChecklistParamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PreTripChecklistParamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PreTripChecklistParam>>> GetParams()
    {
        return await _context.PreTripChecklistParams
            .Where(p => p.IsActive)
            .OrderBy(p => p.Stt)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PreTripChecklistParam>> GetParam(int id)
    {
        var param = await _context.PreTripChecklistParams.FindAsync(id);

        if (param == null || !param.IsActive)
        {
            return NotFound();
        }

        return param;
    }

    [HttpPost]
    public async Task<ActionResult<PreTripChecklistParam>> PostParam(PreTripChecklistParam param)
    {
        _context.PreTripChecklistParams.Add(param);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParam), new { id = param.Id }, param);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutParam(int id, PreTripChecklistParam param)
    {
        if (id != param.Id)
        {
            return BadRequest();
        }

        _context.Entry(param).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ParamExists(id))
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
    public async Task<IActionResult> DeleteParam(int id)
    {
        var param = await _context.PreTripChecklistParams.FindAsync(id);
        if (param == null)
        {
            return NotFound();
        }

        // Hard delete for simplicity if preferred, but soft-delete is implemented
        param.IsActive = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ParamExists(int id)
    {
        return _context.PreTripChecklistParams.Any(e => e.Id == id);
    }
}

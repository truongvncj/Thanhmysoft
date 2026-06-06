using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KiemtravesinhxeParamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KiemtravesinhxeParamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/KiemtravesinhxeParams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KiemtravesinhxeParam>>> GetKiemtravesinhxeParams()
        {
            return await _context.KiemtravesinhxeParams
                .Where(p => p.IsActive)
                .OrderBy(p => p.Stt)
                .ToListAsync();
        }

        // GET: api/KiemtravesinhxeParams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KiemtravesinhxeParam>> GetKiemtravesinhxeParam(int id)
        {
            var kiemtravesinhxeParam = await _context.KiemtravesinhxeParams.FindAsync(id);

            if (kiemtravesinhxeParam == null || !kiemtravesinhxeParam.IsActive)
            {
                return NotFound();
            }

            return kiemtravesinhxeParam;
        }

        // PUT: api/KiemtravesinhxeParams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKiemtravesinhxeParam(int id, KiemtravesinhxeParam kiemtravesinhxeParam)
        {
            if (id != kiemtravesinhxeParam.Id)
            {
                return BadRequest();
            }

            _context.Entry(kiemtravesinhxeParam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KiemtravesinhxeParamExists(id))
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

        // POST: api/KiemtravesinhxeParams
        [HttpPost]
        public async Task<ActionResult<KiemtravesinhxeParam>> PostKiemtravesinhxeParam(KiemtravesinhxeParam kiemtravesinhxeParam)
        {
            _context.KiemtravesinhxeParams.Add(kiemtravesinhxeParam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKiemtravesinhxeParam", new { id = kiemtravesinhxeParam.Id }, kiemtravesinhxeParam);
        }

        // DELETE: api/KiemtravesinhxeParams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKiemtravesinhxeParam(int id)
        {
            var kiemtravesinhxeParam = await _context.KiemtravesinhxeParams.FindAsync(id);
            if (kiemtravesinhxeParam == null)
            {
                return NotFound();
            }

            kiemtravesinhxeParam.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KiemtravesinhxeParamExists(int id)
        {
            return _context.KiemtravesinhxeParams.Any(e => e.Id == id);
        }
    }
}

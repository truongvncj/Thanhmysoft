using System;
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
    public class DailyKiemtravesinhxesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DailyKiemtravesinhxesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DailyKiemtravesinhxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyKiemtravesinhxe>>> GetDailyKiemtravesinhxes()
        {
            return await _context.DailyKiemtravesinhxes
                .Include(d => d.Details)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        // GET: api/DailyKiemtravesinhxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyKiemtravesinhxe>> GetDailyKiemtravesinhxe(int id)
        {
            var dailyKiemtravesinhxe = await _context.DailyKiemtravesinhxes
                .Include(d => d.Details)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dailyKiemtravesinhxe == null)
            {
                return NotFound();
            }

            return dailyKiemtravesinhxe;
        }

        // POST: api/DailyKiemtravesinhxes
        [HttpPost]
        public async Task<ActionResult<DailyKiemtravesinhxe>> PostDailyKiemtravesinhxe(DailyKiemtravesinhxe dailyKiemtravesinhxe)
        {
            dailyKiemtravesinhxe.CreatedAt = DateTime.UtcNow;
            
            _context.DailyKiemtravesinhxes.Add(dailyKiemtravesinhxe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDailyKiemtravesinhxe", new { id = dailyKiemtravesinhxe.Id }, dailyKiemtravesinhxe);
        }
    }
}

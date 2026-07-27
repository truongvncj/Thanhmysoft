using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    public class AdminLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto dto)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.Name.ToLower() == dto.Username.ToLower() && a.Password == dto.Password);

        if (admin == null)
        {
            return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng." });
        }

        return Ok(new { message = "Đăng nhập thành công", admin = new { admin.Name, admin.Right } });
    }

    public class CarrierLoginDto
    {
        public string MaNvt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("carrier-login")]
    public async Task<IActionResult> CarrierLogin([FromBody] CarrierLoginDto dto)
    {
        var nvt = await _context.NhaVanTais
            .FirstOrDefaultAsync(n => n.MaNvt.ToLower() == dto.MaNvt.ToLower() && n.Password == dto.Password);

        if (nvt == null)
        {
            return Unauthorized(new { message = "Mã nhà vận tải hoặc mật khẩu không đúng." });
        }

        return Ok(new { message = "Đăng nhập thành công", carrier = new { nvt.MaNvt, nvt.TenNvt } });
    }

    public class DriverLoginDto
    {
        public string Sothe { get; set; } = string.Empty;
    }

    [HttpPost("driver-login")]
    public async Task<IActionResult> DriverLogin([FromBody] DriverLoginDto dto)
    {
        var driver = await _context.Thenhathaus
            .FirstOrDefaultAsync(t => t.Sothe.Trim().ToLower() == dto.Sothe.Trim().ToLower());

        if (driver == null)
        {
            return Unauthorized(new { message = "Số thẻ không tồn tại." });
        }

        if (driver.TrangThai != 1)
        {
            return Unauthorized(new { message = "Thẻ nhà thầu chưa được duyệt hoặc đã bị khóa." });
        }

        if (driver.Thoihanthe.Date < DateTime.UtcNow.AddHours(7).Date)
        {
            return Unauthorized(new { message = "Thẻ đã hết hạn. Vui lòng liên hệ nhà vận tải để gia hạn." });
        }

        return Ok(new { message = "Đăng nhập thành công", driver = new { driver.MaLx, driver.Ten, driver.Sothe, driver.TenNvt } });
    }

    public class NhanvienLoginDto
    {
        public string Mnv { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? KhohangId { get; set; }
    }

    [HttpPost("nhanvien-login")]
    public async Task<IActionResult> NhanvienLogin([FromBody] NhanvienLoginDto dto)
    {
        var nhanvien = await _context.Nhanviens
            .FirstOrDefaultAsync(n => n.Mnv.ToLower() == dto.Mnv.ToLower() && n.Password == dto.Password);

        if (nhanvien == null)
        {
            return Unauthorized(new { message = "Mã nhân viên hoặc mật khẩu không đúng." });
        }

        var khohangName = "";
        if (dto.KhohangId.HasValue)
        {
            var kho = await _context.Khohangs.FindAsync(dto.KhohangId.Value);
            if (kho != null)
            {
                khohangName = kho.TenKho;
            }
        }

        return Ok(new { 
            message = "Đăng nhập thành công", 
            nhanvien = new { nhanvien.Mnv, nhanvien.Tnv, nhanvien.Chucdanh, nhanvien.Vitri, nhanvien.Permissions },
            khohang = new { id = dto.KhohangId, name = khohangName }
        });
    }
}

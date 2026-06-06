using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=ThanhMy;Username=postgres;Password=postgres");

using var context = new AppDbContext(optionsBuilder.Options);
var today = DateTime.UtcNow.AddHours(7).Date;
var tomorrow = today.AddDays(1);

var items = context.Danhsachxetrongkhos
    .Where(d => d.TrangThai == 3)
    .Select(d => new { d.BienSo, d.GioDangKy, d.XacNhanRaCong_Time, d.TrangThai })
    .ToList();

foreach(var i in items) {
    Console.WriteLine($"{i.BienSo} | DangKy: {i.GioDangKy} | RaCong: {i.XacNhanRaCong_Time} | Status: {i.TrangThai}");
}

using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Models;

namespace SaleManagerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<NhaVanTai> NhaVanTais { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Thenhathau> Thenhathaus { get; set; }
    public DbSet<DangTai> DangTais { get; set; }
    public DbSet<Layoutkho> Layoutkhos { get; set; }
    public DbSet<PreTripChecklistParam> PreTripChecklistParams { get; set; }
    public DbSet<DailyPreTripChecklist> DailyPreTripChecklists { get; set; }
    public DbSet<KiemtravesinhxeParam> KiemtravesinhxeParams { get; set; }
    public DbSet<DailyKiemtravesinhxe> DailyKiemtravesinhxes { get; set; }
    public DbSet<DailyKiemtravesinhxeDetail> DailyKiemtravesinhxeDetails { get; set; }
    public DbSet<Nhanvien> Nhanviens { get; set; }
    public DbSet<Khohang> Khohangs { get; set; }
    public DbSet<LenhDieuChuyenNoiBo> LenhDieuChuyenNoiBos { get; set; }
    public DbSet<ChiTietDieuChuyenNoiBo> ChiTietDieuChuyenNoiBos { get; set; }
    public DbSet<Danhsachxetrongkho> Danhsachxetrongkhos { get; set; }
    public DbSet<ChungTuVaoKho> ChungTuVaoKhos { get; set; }
    public DbSet<TonKhoDauKy> TonKhoDauKies { get; set; }
    public DbSet<TonKhoHienTai> TonKhoHienTais { get; set; }
    public DbSet<PhatSinhSanPham> PhatSinhSanPhams { get; set; }
    public DbSet<Sanpham> Sanphams { get; set; }
    public DbSet<PhieuDemKho> PhieuDemKhos { get; set; }
    public DbSet<ChiTietDemKho> ChiTietDemKhos { get; set; }
    public DbSet<XuatKhoTam> XuatKhoTams { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed some initial data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Dell XPS 15", Price = 1500.00m, StockQuantity = 10 },
            new Product { Id = 2, Name = "iPhone 15 Pro", Price = 999.99m, StockQuantity = 20 },
            new Product { Id = 3, Name = "Bàn phím cơ Keychron Q1", Price = 150.00m, StockQuantity = 50 }
        );

        modelBuilder.Entity<Admin>().HasData(
            new Admin { Name = "tr", Password = "123123", Right = 1 }
        );

        modelBuilder.Entity<PreTripChecklistParam>().HasData(
            new PreTripChecklistParam 
            { 
                Id = 1, 
                NoiDung = "Hồ sơ và Nhân viên lái xe", 
                Stt = 1, 
                DieuKien = "Bằng lái xe phù hợp với loại xe điều khiển và còn hạn...",
                IsActive = true
            }
        );
    }
}

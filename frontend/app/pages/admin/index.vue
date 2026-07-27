<template>
  <div class="min-h-screen bg-slate-50 flex flex-col">
    <!-- Header / Navbar -->
    <header class="bg-white shadow-sm px-6 py-4 flex justify-between items-center sticky top-0 z-10 border-b border-slate-100">
      <div class="flex items-center gap-3">
        <div class="w-10 h-10 bg-green-500 rounded-xl flex items-center justify-center shadow-lg shadow-green-500/30">
          <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"></path></svg>
        </div>
        <h1 class="text-xl font-bold text-slate-800 tracking-tight">Admin Dashboard</h1>
      </div>
      <button @click="logout" class="flex items-center gap-2 text-sm font-semibold text-red-600 bg-red-50 hover:bg-red-100 px-5 py-2.5 rounded-xl transition-all duration-200">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
        Đăng xuất
      </button>
    </header>

    <!-- Main Content -->
    <main class="flex-1 w-full max-w-7xl mx-auto p-6 lg:p-8 animate-[fadeIn_0.4s_ease-out]">
      <div class="mb-8">
        <h2 class="text-2xl font-bold text-slate-800">Tổng quan hệ thống</h2>
        <p class="text-slate-500 mt-1">Quản lý các module chính của hệ thống Thanh Mỹ</p>
      </div>

      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        
        <!-- Nhà vận tải Card -->
        <NuxtLink to="/nhavantais" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-blue-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-blue-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-blue-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-blue-600 transition-colors">Nhà Vận Tải</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý danh sách, thông tin và hợp đồng các nhà vận tải đối tác.</p>
          </div>
        </NuxtLink>

        <!-- Lái xe Card -->
        <NuxtLink to="/admin/laixe" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-emerald-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-emerald-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-emerald-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm14 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-emerald-600 transition-colors">Lái Xe</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý hồ sơ tài xế, bằng lái xe, phương tiện và trạng thái hoạt động.</p>
          </div>
        </NuxtLink>

        <!-- Nhân viên Card -->
        <NuxtLink to="/admin/nhanvien" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-amber-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-amber-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-amber-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-amber-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-amber-600 transition-colors">Nhân Viên</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý hồ sơ nhân viên, tài khoản, phòng ban và phân quyền hệ thống.</p>
          </div>
        </NuxtLink>

        <!-- Layout Kho Card -->
        <NuxtLink to="/admin/layoutkho" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-pink-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-pink-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-pink-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-pink-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zm10 0a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zm10 0a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-pink-600 transition-colors">Layout Kho</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Thiết kế sơ đồ kho và sắp xếp các vị trí local trực quan.</p>
          </div>
        </NuxtLink>

        <!-- Kho Hàng Card -->
        <NuxtLink to="/admin/khohang" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-orange-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-orange-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-orange-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-orange-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-orange-600 transition-colors">Kho Hàng</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý danh sách kho, trạng thái đóng/mở cửa để xe ra vào.</p>
          </div>
        </NuxtLink>

        <!-- Phân Quyền Card -->
        <NuxtLink to="/admin/phanquyen" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-indigo-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-indigo-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-indigo-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-indigo-600 transition-colors">Phân Quyền</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Cấp quyền truy cập các màn hình chức năng cho Nhân viên.</p>
          </div>
        </NuxtLink>

        <!-- Thông số Pre_TripChecklist Card -->
        <NuxtLink to="/admin/pre-trip-checklist" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-purple-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-purple-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-purple-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-purple-600 transition-colors">Thông số Pre_Trip</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý các tiêu chí đánh giá, kiểm tra an toàn trước mỗi chuyến đi.</p>
          </div>
        </NuxtLink>

        <!-- Thông số Vệ Sinh Xe Card -->
        <NuxtLink to="/admin/kiem-tra-ve-sinh" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-teal-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-teal-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-teal-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-teal-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-teal-600 transition-colors">Vệ Sinh Xe Tải</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Cấu hình thông số kiểm tra vệ sinh xe tải hàng ngày.</p>
          </div>
        </NuxtLink>

        <!-- Tồn Kho Đầu Kỳ Card -->
        <NuxtLink to="/admin/tonkhodauky" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-orange-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-orange-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-orange-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-orange-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-orange-600 transition-colors">Tồn Kho Đầu Kỳ</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý lượng hàng hóa tồn kho hiện tại theo từng kho.</p>
          </div>
        </NuxtLink>

        <!-- Sản Phẩm Card -->
        <NuxtLink to="/admin/sanpham" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-cyan-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-cyan-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-cyan-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-cyan-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-cyan-600 transition-colors">Sản Phẩm</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Quản lý danh mục sản phẩm và định lượng theo từng kho.</p>
          </div>
        </NuxtLink>

        <!-- QR Code Card -->
        <NuxtLink to="/admin/qrcode" class="group bg-white rounded-2xl p-6 shadow-sm border border-slate-100 hover:shadow-xl hover:border-pink-200 transition-all duration-300 transform hover:-translate-y-1 relative overflow-hidden">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-pink-50 rounded-full group-hover:scale-150 transition-transform duration-500 ease-out z-0"></div>
          <div class="relative z-10">
            <div class="w-14 h-14 bg-pink-100 rounded-xl flex items-center justify-center mb-5 group-hover:scale-110 transition-transform duration-300 shadow-inner">
              <svg class="w-7 h-7 text-pink-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm14 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z"></path></svg>
            </div>
            <h2 class="text-xl font-bold text-slate-800 mb-2 group-hover:text-pink-600 transition-colors">Tạo QR Code</h2>
            <p class="text-slate-500 text-sm leading-relaxed">Tạo mã QR cho link đăng nhập app nhanh chóng.</p>
          </div>
        </NuxtLink>

      </div>
    </main>
  </div>
</template>

<script setup>

definePageMeta({
  layout: false,
  title: 'Admin Dashboard'
})

const router = useRouter()

const logout = () => {
  // Can add clear token / state logic here later
  router.push('/')
}
</script>

<style>
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>

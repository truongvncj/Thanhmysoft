<template>
  <div class="min-h-screen bg-slate-50 flex flex-col">
    <!-- Header -->
    <header class="bg-white shadow-sm px-6 py-4 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 sticky top-0 z-10 border-b border-slate-100">
      <div>
        <h1 class="text-xl font-bold text-slate-800 tracking-tight">{{ nhanvienInfo?.vitri }} {{ nhanvienInfo?.chucdanh }}</h1>
        <p class="text-sm text-slate-500 mt-1">Xin chào, <span class="font-semibold text-slate-700">{{ nhanvienInfo?.tnv }}</span> | Kho: <span class="font-semibold text-blue-600">{{ khohangInfo?.name }}</span></p>
      </div>
      <div v-if="isBaoVe" class="flex flex-col items-center bg-white px-4 py-2 rounded-xl border border-slate-200 shadow-sm">
        <span class="text-xs text-slate-500 font-medium mb-1">Tỷ lệ Lấy mẫu kiểm tra</span>
        <div class="text-xl font-bold" :class="checkedPercentage < 5 ? 'text-red-600' : 'text-green-600'">
          {{ checkedPercentage }}% <span class="text-xs text-slate-400 font-normal">({{ checkedVehicles }}/{{ totalVehicles }})</span>
        </div>
      </div>
      <button @click="logout" class="flex items-center gap-2 text-sm font-semibold text-red-600 bg-red-50 hover:bg-red-100 px-5 py-2.5 rounded-xl transition-all duration-200">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
        Đăng xuất
      </button>
    </header>

    <!-- Flex container for Sidebar & Main Content -->
    <div class="flex flex-1 w-full max-w-[1400px] mx-auto overflow-hidden">
      <!-- Sidebar (Desktop) -->
      <aside class="hidden md:flex flex-col w-64 bg-white border-r border-slate-200">
        <nav class="flex-1 overflow-y-auto py-4">
          <ul class="space-y-1 px-3">
            <li>
              <button @click="activeMenu = 'dashboard'" :class="[activeMenu === 'dashboard' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'dashboard' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
                </svg>
                <span class="truncate">Danh sách xe trong sân</span>
              </button>
            </li>
            <li v-if="isThuKho">
              <button @click="activeMenu = 'nhapkho'" :class="[activeMenu === 'nhapkho' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'nhapkho' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" />
                </svg>
                <span class="truncate">Nhập kho</span>
              </button>
            </li>
            <li v-if="isThuKho">
              <button @click="activeMenu = 'xuatkho'" :class="[activeMenu === 'xuatkho' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'xuatkho' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                </svg>
                <span class="truncate">Xuất kho</span>
              </button>
            </li>
            <li v-if="isThuKho">
              <button @click="activeMenu = 'demkho'" :class="[activeMenu === 'demkho' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'demkho' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" />
                </svg>
                <span class="truncate">Đếm kho</span>
              </button>
            </li>
            <li v-if="isThuKho">
              <button @click="activeMenu = 'dieuchuyen'" :class="[activeMenu === 'dieuchuyen' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'dieuchuyen' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
                </svg>
                <span class="truncate">Điều chuyển nội bộ</span>
              </button>
            </li>
            <li v-if="isThuKho" class="pt-4 mt-4 border-t border-slate-100">
              <button @click="activeMenu = 'baocao'" :class="[activeMenu === 'baocao' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900', 'group flex items-center px-3 py-2.5 text-sm rounded-lg w-full text-left transition-colors']">
                <svg class="mr-3 h-5 w-5 flex-shrink-0" :class="activeMenu === 'baocao' ? 'text-blue-600' : 'text-slate-400 group-hover:text-slate-500'" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span class="truncate">Báo cáo</span>
              </button>
            </li>
          </ul>
        </nav>
      </aside>

      <!-- Main Content -->
      <div class="flex-1 flex flex-col w-full overflow-hidden">
        <!-- Mobile Navigation -->
        <div class="md:hidden flex overflow-x-auto bg-white border-b border-slate-200 custom-scrollbar flex-shrink-0">
          <button @click="activeMenu = 'dashboard'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'dashboard' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">DS Xe trong sân</button>
          <button v-if="isThuKho" @click="activeMenu = 'nhapkho'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'nhapkho' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">Nhập kho</button>
          <button v-if="isThuKho" @click="activeMenu = 'xuatkho'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'xuatkho' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">Xuất kho</button>
          <button v-if="isThuKho" @click="activeMenu = 'demkho'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'demkho' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">Đếm kho</button>
          <button v-if="isThuKho" @click="activeMenu = 'dieuchuyen'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'dieuchuyen' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">Điều chuyển nội bộ</button>
          <button v-if="isThuKho" @click="activeMenu = 'baocao'" :class="['px-4 py-3 whitespace-nowrap text-sm font-medium border-b-2 transition-colors', activeMenu === 'baocao' ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent']">Báo cáo</button>
        </div>

        <main class="flex-1 w-full p-4 sm:p-6 lg:p-8 overflow-y-auto">
        <div v-if="activeMenu === 'dashboard'">
      
      <!-- Tabs -->
      <div class="flex overflow-x-auto border-b border-gray-200 mb-6 custom-scrollbar pb-2">
        <button v-for="(tab, index) in tabs" :key="index" @click="activeTab = index" 
          :class="[
            'px-4 py-3 sm:px-6 whitespace-nowrap font-medium text-sm transition-colors border-b-2 outline-none',
            activeTab === index ? 'border-blue-500 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]">
          {{ tab.name }}
          <span class="ml-2 py-0.5 px-2 bg-gray-100 rounded-full text-xs" :class="activeTab === index ? 'bg-blue-100 text-blue-600' : ''">
            {{ filteredVehicles(index).length }}
          </span>
        </button>
      </div>

      <!-- Content -->
      <div v-if="loading" class="text-center py-10">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-slate-500">Đang tải dữ liệu...</p>
      </div>
      
      <div v-else>
        <!-- Mobile & Tablet View: Cards -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6 lg:hidden">
          <div v-for="xe in filteredVehicles(activeTab)" :key="xe.id" class="bg-white rounded-2xl shadow-sm border border-slate-200 p-5 hover:shadow-md transition-shadow flex flex-col">
            <div class="flex justify-between items-start mb-4">
              <div>
                <h3 class="font-bold text-lg text-slate-800">{{ xe.bienSo || 'N/A' }}</h3>
                <p class="text-sm text-slate-500 mt-0.5">{{ xe.tenLaiXe }}</p>
              </div>
              <span class="px-2.5 py-1 bg-blue-50 text-blue-700 text-xs font-bold rounded-lg">{{ xe.lyDoVaoKho }}</span>
            </div>
            
            <div class="space-y-2 mb-6 flex-1">
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500">Nhà vận tải:</span>
                <span class="font-medium text-slate-800 text-right ml-2">{{ xe.tenNhaThauVanTai }}</span>
              </div>
              <div class="flex items-center justify-between text-sm" v-if="activeTab > 0">
                <span class="text-slate-500">TG trong sân:</span>
                <span class="font-bold text-red-600">{{ computeThoiGianTrongSan(xe) }}</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500">Chứng từ:</span>
                <span class="font-medium text-slate-800 text-right ml-2">{{ xe.chungTus && xe.chungTus.length ? xe.chungTus.join(', ') : '-' }}</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500">Pre-Checklist:</span>
                <span v-if="xe.pre_Tripchecklist_Time" class="text-green-600 font-bold">Đạt</span>
                <span v-else class="text-amber-500 font-bold">Chưa</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500">Vệ sinh xe:</span>
                <span v-if="xe.kiemTraVeSinh_Time" class="text-green-600 font-bold">Đạt</span>
                <span v-else class="text-amber-500 font-bold">Chưa</span>
              </div>
              <div v-if="activeTab === 0 && isBaoVe" class="pt-2">
                <button @click="openAuditModal(xe)" class="w-full text-xs bg-indigo-50 text-indigo-700 font-bold py-2 rounded-lg border border-indigo-200 hover:bg-indigo-100 transition-colors">
                  <span v-if="xe.baoVeKiemTra_Time">✅ Bảo vệ đã kiểm tra</span>
                  <span v-else>🔍 Bảo vệ kiểm tra</span>
                </button>
              </div>
            </div>
            
            <!-- Actions -->
            <div class="mt-auto border-t pt-4">
              <template v-if="activeTab === 0 && isBaoVe">
                <button @click="updateStatus(xe.id, 1)" :disabled="!xe.pre_Tripchecklist_Time || !xe.kiemTraVeSinh_Time" class="w-full bg-blue-600 hover:bg-blue-700 disabled:bg-slate-300 disabled:cursor-not-allowed text-white font-bold py-2.5 rounded-xl transition-colors">
                  Đồng ý
                </button>
              </template>
              <template v-else-if="activeTab === 1 && isThuKho">
                <button @click="updateStatus(xe.id, 2, xe)" :class="['w-full text-white font-bold py-2.5 rounded-xl transition-colors', (xe.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'bg-emerald-600 hover:bg-emerald-700' : 'bg-orange-500 hover:bg-orange-600']">
                  {{ (xe.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'Hoàn tất Nhập hàng' : 'Hoàn tất Xuất hàng' }}
                </button>
              </template>
              <template v-else-if="activeTab === 1 && isBaoVe">
                <button v-if="!xe.baoVeKiemTraTrongKho_Time" @click="updateChangBuoc(xe.id)" class="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2.5 rounded-xl transition-colors">
                  Xác nhận chằng buộc
                </button>
                <div v-else class="w-full text-center text-green-600 font-bold py-2.5 bg-green-50 rounded-xl border border-green-200">
                  ✅ Đã chằng buộc
                </div>
              </template>
              <template v-else-if="activeTab === 2 && isBaoVe">
                <button @click="updateStatus(xe.id, 3)" class="w-full bg-green-600 hover:bg-green-700 text-white font-bold py-2.5 rounded-xl transition-colors">
                  Xác nhận ra sân
                </button>
              </template>
              <template v-else>
                <div class="text-center text-sm text-slate-400 italic py-1">Không có quyền thao tác</div>
              </template>
            </div>
          </div>
        </div>

        <!-- Desktop View: Table -->
        <div class="hidden lg:block overflow-x-auto bg-white rounded-xl shadow-sm border border-slate-200">
          <table class="w-full text-left border-collapse min-w-[800px]">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-sm text-slate-600">
                <th class="p-4 font-semibold whitespace-nowrap">Biển số / Lái xe</th>
                <th class="p-4 font-semibold whitespace-nowrap">Lý do</th>
                <th class="p-4 font-semibold whitespace-nowrap">Chứng từ</th>
                <th class="p-4 font-semibold whitespace-nowrap">Nhà vận tải</th>
                <th class="p-4 font-semibold whitespace-nowrap" v-if="activeTab > 0">TG trong sân</th>
                <th class="p-4 font-semibold whitespace-nowrap">Trạng thái (Pre / VS)</th>
                <th class="p-4 font-semibold text-right whitespace-nowrap">{{ activeTab === 0 ? 'Cho vào kho' : 'Thao tác' }}</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="xe in filteredVehicles(activeTab)" :key="'table-'+xe.id" class="border-b border-slate-100 hover:bg-slate-50 transition-colors">
                <td class="p-4">
                  <div class="font-bold text-slate-800 text-base">{{ xe.bienSo || 'N/A' }}</div>
                  <div class="text-sm text-slate-500">{{ xe.tenLaiXe }}</div>
                </td>
                <td class="p-4">
                  <span class="px-2.5 py-1 bg-blue-50 text-blue-700 text-xs font-bold rounded-lg">{{ xe.lyDoVaoKho }}</span>
                </td>
                <td class="p-4 text-sm text-slate-800">
                  {{ xe.chungTus && xe.chungTus.length ? xe.chungTus.join(', ') : '-' }}
                </td>
                <td class="p-4 text-sm font-medium text-slate-800">{{ xe.tenNhaThauVanTai }}</td>
                <td class="p-4 text-sm font-bold text-red-600" v-if="activeTab > 0">{{ computeThoiGianTrongSan(xe) }}</td>
                <td class="p-4 text-sm">
                  <div class="flex items-center gap-2 mb-1.5">
                    <span class="text-slate-500 w-8 text-xs font-medium">Pre:</span>
                    <span v-if="xe.pre_Tripchecklist_Time" class="text-green-600 font-bold text-xs bg-green-50 px-2 py-0.5 rounded border border-green-100">Đạt</span>
                    <span v-else class="text-amber-500 font-bold text-xs bg-amber-50 px-2 py-0.5 rounded border border-amber-100">Chưa</span>
                  </div>
                  <div class="flex items-center gap-2 mb-2">
                    <span class="text-slate-500 w-8 text-xs font-medium">VS:</span>
                    <span v-if="xe.kiemTraVeSinh_Time" class="text-green-600 font-bold text-xs bg-green-50 px-2 py-0.5 rounded border border-green-100">Đạt</span>
                    <span v-else class="text-amber-500 font-bold text-xs bg-amber-50 px-2 py-0.5 rounded border border-amber-100">Chưa</span>
                  </div>
                  <div v-if="activeTab === 0 && isBaoVe">
                    <button @click="openAuditModal(xe)" class="text-[11px] bg-indigo-50 text-indigo-700 font-bold px-2 py-1 rounded border border-indigo-200 hover:bg-indigo-100 transition-colors w-full text-center">
                      <span v-if="xe.baoVeKiemTra_Time">✅ Bảo vệ đã kiểm tra</span>
                      <span v-else>🔍 Bảo vệ kiểm tra</span>
                    </button>
                  </div>
                </td>
                <td class="p-4 text-right">
                  <template v-if="activeTab === 0 && isBaoVe">
                    <button @click="updateStatus(xe.id, 1)" :disabled="!xe.pre_Tripchecklist_Time || !xe.kiemTraVeSinh_Time" class="inline-block bg-blue-600 hover:bg-blue-700 disabled:bg-slate-300 disabled:cursor-not-allowed text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm">
                      Đồng ý
                    </button>
                  </template>
                  <template v-else-if="activeTab === 1 && isThuKho">
                    <button @click="updateStatus(xe.id, 2, xe)" :class="['inline-block text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm', (xe.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'bg-emerald-600 hover:bg-emerald-700' : 'bg-orange-500 hover:bg-orange-600']">
                      {{ (xe.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'Hoàn tất Nhập hàng' : 'Hoàn tất Xuất hàng' }}
                    </button>
                  </template>
                  <template v-else-if="activeTab === 1 && isBaoVe">
                    <button v-if="!xe.baoVeKiemTraTrongKho_Time" @click="updateChangBuoc(xe.id)" class="inline-block bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm">
                      Xác nhận chằng buộc
                    </button>
                    <span v-else class="inline-block text-green-600 font-bold px-3 py-2 bg-green-50 rounded-lg border border-green-200 text-sm">
                      ✅ Đã chằng buộc
                    </span>
                  </template>
                  <template v-else-if="activeTab === 2 && isBaoVe">
                    <button @click="updateStatus(xe.id, 3)" class="inline-block bg-green-600 hover:bg-green-700 text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm">
                      Xác nhận ra sân
                    </button>
                  </template>
                  <template v-else>
                    <span class="text-sm text-slate-400 italic">Không có quyền thao tác</span>
                  </template>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        
        <!-- Empty State -->
        <div v-if="filteredVehicles(activeTab).length === 0" class="py-16 text-center text-slate-500 bg-white rounded-xl border border-slate-200 mt-6 lg:mt-0 lg:border-t-0 lg:rounded-t-none">
          <svg class="w-16 h-16 mx-auto text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"></path></svg>
          <p>Không có xe nào ở trạng thái này.</p>
        </div>
        </div> <!-- Close v-else -->
        </div> <!-- Close activeMenu === dashboard -->

        <!-- Các menu khác -->
        <div v-else-if="activeMenu === 'nhapkho'" class="bg-white rounded-xl shadow-sm border border-slate-200 h-full flex flex-col">
          <div class="p-6 border-b border-slate-100 flex justify-between items-center">
            <h2 class="text-xl font-bold text-slate-800">Nhập Kho</h2>
          </div>
          
          <!-- Menu tabs cho Nhập Kho -->
          <div class="px-6 py-2 border-b border-slate-100 flex gap-6 bg-slate-50/50">
            <button 
              v-for="(tab, index) in nhapKhoTabs" 
              :key="index"
              @click="activeNhapKhoTab = index"
              class="pb-3 text-sm font-semibold transition-all relative border-b-2"
              :class="activeNhapKhoTab === index ? 'text-emerald-600 border-emerald-600' : 'text-slate-500 border-transparent hover:text-slate-700'"
            >
              {{ tab.name }}
            </button>
          </div>
          
          <div class="p-4 sm:p-6 overflow-auto flex-1 bg-slate-50">
            <div v-if="filteredNhapKhoVehicles.length === 0" class="text-center py-12 text-slate-500 bg-white rounded-xl border border-slate-200">
              <svg class="w-16 h-16 mx-auto text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"></path></svg>
              <p>Không có dữ liệu.</p>
            </div>
            
            <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              <div v-for="xe in filteredNhapKhoVehicles" :key="xe.id" @click="openNhapKhoModal(xe)" class="bg-white rounded-2xl shadow-sm border border-emerald-100 p-5 hover:shadow-md transition-shadow relative overflow-hidden cursor-pointer">
                <div class="absolute top-0 right-0 w-16 h-16 bg-emerald-50 rounded-bl-full -z-0"></div>
                <div class="relative z-10">
                  <div class="flex justify-between items-start mb-4">
                    <div>
                      <h3 class="font-bold text-lg text-slate-800">{{ xe.bienSo || 'N/A' }}</h3>
                      <p class="text-sm text-slate-500 font-medium">{{ xe.tenLaiXe }}</p>
                    </div>
                    <span class="px-2.5 py-1 bg-emerald-100 text-emerald-800 text-xs font-bold rounded-lg">{{ computeThoiGianTrongSan(xe) }}</span>
                  </div>
                  
                  <div class="space-y-3 pt-3 border-t border-slate-100">
                    <div class="flex flex-col">
                      <span class="text-xs text-slate-500 font-medium uppercase tracking-wider mb-1">STO</span>
                      <span class="text-sm font-semibold text-slate-800 break-words" :class="getStos(xe) === '-' ? 'text-slate-400 italic font-normal' : ''">{{ getStos(xe) }}</span>
                    </div>

                    <div class="flex flex-col">
                      <span class="text-xs text-slate-500 font-medium uppercase tracking-wider mb-1">Shipment</span>
                      <span class="text-sm font-semibold text-slate-800 break-words" :class="getShipments(xe) === '-' ? 'text-slate-400 italic font-normal' : ''">{{ getShipments(xe) }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-else-if="activeMenu === 'xuatkho'" class="bg-white rounded-xl shadow-sm border border-slate-200 h-full flex flex-col">
          <div class="p-6 border-b border-slate-100 flex justify-between items-center">
            <h2 class="text-xl font-bold text-slate-800">Xuất Kho</h2>
          </div>
          
          <!-- Menu tabs cho Xuất Kho -->
          <div class="px-6 py-2 border-b border-slate-100 flex gap-6 bg-slate-50/50">
            <button 
              v-for="(tab, index) in xuatKhoTabs" 
              :key="index"
              @click="activeXuatKhoTab = index"
              class="pb-3 text-sm font-semibold transition-all relative border-b-2"
              :class="activeXuatKhoTab === index ? 'text-blue-600 border-blue-600' : 'text-slate-500 border-transparent hover:text-slate-700'"
            >
              {{ tab.name }}
            </button>
          </div>
          
          <div class="p-4 sm:p-6 overflow-auto flex-1 bg-slate-50">
            <div v-if="filteredXuatKhoVehicles.length === 0" class="text-center py-12 text-slate-500 bg-white rounded-xl border border-slate-200">
              <svg class="w-16 h-16 mx-auto text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"></path></svg>
              <p>Không có dữ liệu.</p>
            </div>
            
            <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              <div v-for="xe in filteredXuatKhoVehicles" :key="xe.id" @click="openXuatKhoModal(xe)" class="bg-white rounded-2xl shadow-sm border border-blue-100 p-5 hover:shadow-md transition-shadow relative overflow-hidden cursor-pointer">
                <div class="absolute top-0 right-0 w-16 h-16 bg-blue-50 rounded-bl-full -z-0"></div>
                <div class="relative z-10">
                  <div class="flex justify-between items-start mb-4">
                    <div>
                      <h3 class="font-bold text-lg text-slate-800">{{ xe.bienSo || 'N/A' }}</h3>
                      <p class="text-sm text-slate-500 font-medium">{{ xe.tenLaiXe }}</p>
                    </div>
                    <span class="px-2.5 py-1 bg-blue-100 text-blue-800 text-xs font-bold rounded-lg">{{ computeThoiGianTrongSan(xe) }}</span>
                  </div>
                  
                  <div class="space-y-3 pt-3 border-t border-slate-100">
                    <div class="flex flex-col">
                      <span class="text-xs text-slate-500 font-medium uppercase tracking-wider mb-1">STO</span>
                      <span class="text-sm font-semibold text-slate-800 break-words" :class="getStos(xe) === '-' ? 'text-slate-400 italic font-normal' : ''">{{ getStos(xe) }}</span>
                    </div>

                    <div class="flex flex-col">
                      <span class="text-xs text-slate-500 font-medium uppercase tracking-wider mb-1">Shipment</span>
                      <span class="text-sm font-semibold text-slate-800 break-words" :class="getShipments(xe) === '-' ? 'text-slate-400 italic font-normal' : ''">{{ getShipments(xe) }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-else-if="activeMenu === 'demkho'" class="bg-white rounded-xl shadow-sm border border-slate-200 p-8 h-full flex flex-col">
          <div class="flex justify-between items-center mb-6">
            <h2 class="text-xl font-bold text-slate-800">Đếm kho: {{ khohangInfo?.name }}</h2>
            <div class="flex bg-slate-100 p-1 rounded-lg">
              <button @click="activeDemKhoTab = 'sodo'" :class="['px-4 py-1.5 rounded-md text-sm font-medium transition-colors', activeDemKhoTab === 'sodo' ? 'bg-white shadow-sm text-blue-600' : 'text-slate-600 hover:text-slate-900']">Sơ đồ kho</button>
              <button @click="activeDemKhoTab = 'sodem'" :class="['px-4 py-1.5 rounded-md text-sm font-medium transition-colors', activeDemKhoTab === 'sodem' ? 'bg-white shadow-sm text-blue-600' : 'text-slate-600 hover:text-slate-900']">Số đếm</button>
            </div>
            <div class="flex items-center gap-2">
              <template v-if="activeDemKhoTab === 'sodo'">
                <div class="relative">
                  <input :value="demKhoViTriSearchQuery"
                         @input="onDemKhoViTriInput"
                         @focus="onDemKhoViTriFocus"
                         @blur="handleDemKhoViTriBlur"
                         @keydown.down.prevent="onDemKhoViTriArrowDown"
                         @keydown.up.prevent="onDemKhoViTriArrowUp"
                         @keydown.enter="onDemKhoViTriEnter"
                         type="text" placeholder="Tìm vị trí..." class="px-3 py-1.5 border border-slate-300 rounded-lg text-sm w-48 focus:ring-2 focus:ring-blue-500 focus:outline-none uppercase font-medium">
                  
                  <!-- Dropdown Vị trí Đếm kho -->
                  <div v-if="showDemKhoViTriDropdown && filteredDemKhoViTri.length > 0" ref="demKhoViTriDropdownList" class="absolute z-50 w-full left-0 mt-1 bg-white border border-slate-200 rounded-lg shadow-lg max-h-60 overflow-y-auto custom-scrollbar">
                    <ul class="py-1 text-sm text-slate-700">
                      <li v-for="(vt, index) in filteredDemKhoViTri" :key="vt.maLocal" @mousedown.prevent="selectDemKhoViTri(vt.maLocal)" :class="['px-4 py-2 cursor-pointer flex justify-between items-center transition-colors', selectedDemKhoViTriIndex === index ? 'bg-blue-100' : 'hover:bg-blue-50']">
                        <span class="font-bold text-blue-700">{{ vt.maLocal }}</span>
                      </li>
                    </ul>
                  </div>
                </div>
                <button @click="onDemKhoManualGo" class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-1.5 rounded-lg text-sm font-bold shadow-sm transition-colors">Go</button>
                <button @click="fetchLayout" class="text-sm bg-blue-50 text-blue-600 px-3 py-1.5 rounded-lg hover:bg-blue-100 font-semibold transition-colors">
                  Làm mới sơ đồ
                </button>
              </template>
            </div>
          </div>
          
          <div v-if="activeDemKhoTab === 'sodo'"
            ref="layoutContainer"
            class="flex-1 bg-slate-200 rounded-2xl shadow-inner border border-slate-300 relative overflow-auto"
            :class="isDraggingLayout ? 'cursor-grabbing' : 'cursor-grab'"
            @mousedown="onLayoutMouseDown"
            @mousemove="onLayoutMouseMove"
            @mouseup="onLayoutMouseUp"
            @mouseleave="onLayoutMouseLeave"
          >
            <div v-if="loadingLayout" class="absolute inset-0 flex items-center justify-center bg-white/50 z-50">
              <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
            </div>
            
            <div v-else-if="layoutItems.length === 0" class="absolute inset-0 flex items-center justify-center text-slate-400 font-medium">
              Chưa có sơ đồ Layout nào được thiết lập cho kho này
            </div>
            
            <div v-else class="relative w-[3000px] h-[3000px] bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAiIGhlaWdodD0iMjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGNpcmNsZSBjeD0iMiIgY3k9IjIiIHI9IjEiIGZpbGw9IiNjYmQ1ZTEiLz48L3N2Zz4=')]">
              <!-- Layout Items Read-Only -->
              <div 
                v-for="item in layoutItems" 
                :key="item.id"
                @dblclick="onDemKhoLayoutElementDblClick(item)"
                :class="[
                  'absolute flex items-center justify-center select-none transition-shadow group',
                  (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? 'bg-slate-800 shadow-sm border-0' : 
                  item.elementType === 'text' ? `bg-transparent text-slate-800 shadow-none border-0 ring-0 outline-none leading-none ${item.hang === 'bold' ? 'font-bold' : 'font-normal'}` : 
                  'bg-white border-2 border-blue-500 shadow-md rounded hover:shadow-lg cursor-pointer'
                ]"
                :style="{ 
                  left: item.positionX + 'px', 
                  top: item.positionY + 'px',
                  width: item.width + 'px',
                  height: item.height + 'px',
                  borderRadius: (item.borderRadius || 0) + 'px',
                  transform: `rotate(${item.rotation || 0}deg)`,
                  transformOrigin: (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? '0% 50%' : '50% 50%',
                  fontSize: item.elementType === 'text' ? (item.height * 0.6) + 'px' : undefined,
                  zIndex: 1
                }"
              >
                <!-- Content -->
                <div class="text-center w-full px-2 overflow-hidden" v-if="(!item.elementType && !item.maLocal.startsWith('LINE_')) || item.elementType === 'local'">
                  <div class="font-bold text-slate-800 truncate" :title="item.maLocal">{{ item.maLocal }}</div>
                  <div class="text-[10px] text-slate-500 truncate" v-if="item.hang || item.cot">H:{{item.hang}} - C:{{item.cot}}</div>
                </div>
                <div class="w-full h-full flex items-center justify-center overflow-hidden" v-else-if="item.elementType === 'text'">
                  {{ item.maLocal }}
                </div>

                <!-- Tooltip -->
                <div class="absolute bottom-full left-1/2 -translate-x-1/2 mb-2 w-48 bg-gray-900 text-white text-xs rounded py-1 px-2 opacity-0 group-hover:opacity-100 pointer-events-none z-20 shadow-lg text-center transition-opacity" v-if="(!item.elementType && !item.maLocal.startsWith('LINE_')) || item.elementType === 'local'">
                  <p class="font-bold">{{ item.maLocal }}</p>
                  <p v-if="item.hang">Dãy hàng: {{ item.hang }}</p>
                  <p v-if="item.cot">STT Dãy Hàng: {{ item.cot }}</p>
                  <p v-if="item.ghiChu" class="mt-1 border-t border-gray-700 pt-1">{{ item.ghiChu }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Tab Số đếm (Bảng tạm) -->
          <div v-else class="flex-1 bg-white border border-slate-200 rounded-lg overflow-hidden flex flex-col">
            <div class="flex-1 overflow-auto flex flex-col">
              <div v-if="currentCountSession.length === 0" class="p-8 text-center text-slate-500 flex-1 flex items-center justify-center">
                Chưa có vị trí nào được đếm trong phiên này. Hãy sang tab Sơ đồ kho và đúp chuột vào một vị trí để đếm.
              </div>
              <div v-else class="overflow-auto flex-1 p-2">
                <table class="w-full text-sm border-collapse border border-slate-200">
                  <thead class="bg-slate-100 border-b border-slate-300 text-xs text-slate-600 uppercase sticky top-0 z-10 shadow-sm">
                    <tr>
                      <th class="px-4 py-3 text-left font-semibold border-r border-slate-200">Sản phẩm</th>
                      <th class="px-4 py-3 text-center font-semibold border-r border-slate-200">NSX / HSD</th>
                      <th class="px-4 py-3 text-center font-semibold border-r border-slate-200 w-32">Số Đếm<br>(Chẵn / Lẻ)</th>
                      <th class="px-4 py-3 text-center font-semibold border-r border-slate-200 w-32">Chênh lệch<br>(Chẵn / Lẻ)</th>
                      <th class="px-4 py-3 text-left font-semibold">Ghi chú</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-200">
                    <template v-for="(phieu, idx) in currentCountSession" :key="phieu.id">
                      <!-- Dòng Vị Trí -->
                      <tr class="bg-blue-50/80 border-t-2 border-slate-300">
                        <td colspan="5" class="px-4 py-2">
                          <div class="flex justify-between items-center">
                            <div class="flex items-center gap-2">
                              <span class="text-blue-800 font-bold text-base">Vị trí: {{ phieu.viTri }}</span>
                              <span class="text-slate-500 text-xs font-medium bg-white px-2 py-0.5 rounded-full border border-blue-100">{{ phieu.chiTiets.length }} mã hàng</span>
                              <span v-if="phieu.ghiChu" class="text-slate-600 text-xs italic ml-2">- {{ phieu.ghiChu }}</span>
                            </div>
                            <button @click="viewPhieuDemKho(phieu)" class="text-blue-600 hover:text-blue-800 text-xs font-bold bg-white px-3 py-1 rounded shadow-sm border border-blue-200 hover:bg-blue-50 transition-colors">Sửa vị trí này</button>
                          </div>
                        </td>
                      </tr>
                      <!-- Các dòng Chi Tiết -->
                      <tr v-for="(ct, cIdx) in phieu.chiTiets" :key="cIdx" class="hover:bg-slate-50">
                        <td class="px-4 py-2 border-r border-slate-100">
                          <div class="font-bold text-slate-800">{{ ct.maSanPham }}</div>
                          <div class="text-slate-600 text-xs">{{ ct.tenSanPham }}</div>
                        </td>
                        <td class="px-4 py-2 text-center text-slate-600 border-r border-slate-100 text-xs">
                          <div v-if="ct.ngaySanXuat">{{ formatDate(ct.ngaySanXuat) }}</div>
                          <div v-if="ct.hanSuDung">{{ formatDate(ct.hanSuDung) }}</div>
                        </td>
                        <td class="px-4 py-2 text-center border-r border-slate-100">
                          <span class="text-blue-700 font-bold">{{ ct.soDemChan }}</span>
                          <span class="text-slate-400 mx-1">/</span>
                          <span class="text-blue-600 font-medium">{{ ct.soDemLe }}</span>
                        </td>
                        <td class="px-4 py-2 text-center border-r border-slate-100">
                          <span :class="getChenhLechColor(ct.chenhLechChan)">{{ ct.chenhLechChan }}</span>
                          <span class="text-slate-400 mx-1">/</span>
                          <span :class="getChenhLechColor(ct.chenhLechLe)">{{ ct.chenhLechLe }}</span>
                        </td>
                        <td class="px-4 py-2 text-slate-600 text-xs italic">{{ ct.ghiChu }}</td>
                      </tr>
                    </template>
                  </tbody>
                </table>
              </div>
            </div>
            
            <div class="p-4 border-t border-slate-200 bg-slate-50 flex flex-wrap justify-between gap-3">
              <div class="flex gap-2">
                <button @click="startNewCountSession" class="px-4 py-2.5 bg-red-50 border border-red-200 text-red-600 hover:bg-red-100 rounded-lg font-bold transition-colors shadow-sm flex items-center gap-2">
                  Bắt đầu đếm kho (Xóa bản tạm)
                </button>
                <button @click="fetchDemKhoTam" class="px-4 py-2.5 bg-slate-200 hover:bg-slate-300 text-slate-700 rounded-lg font-medium transition-colors shadow-sm flex items-center gap-2">
                  Tải lại
                </button>
              </div>
              <div class="flex gap-2" v-if="currentCountSession.length > 0">
                <button @click="exportDemKhoExcel" class="px-6 py-2.5 bg-white border border-emerald-500 text-emerald-600 hover:bg-emerald-50 rounded-lg font-bold transition-colors shadow-sm flex items-center gap-2">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path></svg>
                  Xuất Excel
                </button>
                <button @click="submitCountSession" :disabled="isSubmittingDemKho" class="px-8 py-2.5 bg-emerald-600 hover:bg-emerald-700 text-white rounded-lg font-bold transition-colors disabled:opacity-50 shadow-sm flex items-center gap-2">
                  <span v-if="isSubmittingDemKho" class="animate-spin h-4 w-4 border-2 border-white border-t-transparent rounded-full"></span>
                  Hoàn thành phiên đếm kho
                </button>
              </div>
            </div>
          </div>
        </div>

        <div v-else-if="activeMenu === 'dieuchuyen'" class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 text-slate-500 h-full flex flex-col text-left">
          <div class="flex items-center justify-between mb-6">
            <h2 class="text-2xl font-bold text-slate-800">Điều chuyển nội bộ</h2>
            <div class="flex gap-2">
              <button @click="activeDieuChuyenTab = 'taolenh'" :class="['px-4 py-2 rounded-lg font-medium text-sm transition-colors', activeDieuChuyenTab === 'taolenh' ? 'bg-blue-100 text-blue-700' : 'bg-slate-100 text-slate-600 hover:bg-slate-200']">Tạo lệnh</button>
              <button @click="activeDieuChuyenTab = 'dangdieuchuyen'; fetchPendingLenhDcnb()" :class="['px-4 py-2 rounded-lg font-medium text-sm transition-colors', activeDieuChuyenTab === 'dangdieuchuyen' ? 'bg-blue-100 text-blue-700' : 'bg-slate-100 text-slate-600 hover:bg-slate-200']">Đang điều chuyển</button>
            </div>
          </div>

          <div v-if="activeDieuChuyenTab === 'taolenh'" class="flex-1 overflow-y-auto">
            <!-- Chọn Vị trí đi -->
            <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm mb-6">
              <div class="flex items-end gap-4 mb-4">
                <div class="flex-1">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Vị trí chuyển đi</label>
                  <input v-model="dcnbViTriDiInput" type="text" class="w-full px-4 py-2.5 rounded-lg border border-slate-300 focus:ring-2 focus:ring-blue-500 uppercase font-medium text-slate-900" placeholder="VD: A12" @keyup.enter="fetchDcnbTonKhoList" />
                </div>
                <button @click="openLayoutModalFor('dcnb_di')" :disabled="dcnbLoadingTonKho" class="px-6 py-2.5 bg-blue-100 hover:bg-blue-200 text-blue-700 font-bold rounded-lg shadow-sm disabled:opacity-50 transition-colors flex items-center justify-center gap-2 h-[46px]">
                  <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 20l-5.447-2.724A1 1 0 013 16.382V5.618a1 1 0 011.447-.894L9 7m0 13l6-3m-6 3V7m6 10l4.553 2.276A1 1 0 0021 18.382V7.618a1 1 0 00-.553-.894L15 4m0 13V4m0 0L9 7"></path></svg>
                  Chọn vị trí
                </button>
              </div>

              <!-- Bảng tồn kho vị trí đi -->
              <div v-if="dcnbTonKhoList.length > 0" class="border border-slate-200 rounded-lg overflow-hidden mt-4">
                <div class="overflow-x-auto">
                  <table class="w-full whitespace-nowrap">
                    <thead class="bg-slate-50">
                      <tr>
                        <th class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase">Mã Hàng</th>
                        <th class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase">Tên Sản Phẩm</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">NSX</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">HSD</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Tồn Chẵn</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Tồn Lẻ</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-blue-600 uppercase">Chuyển Chẵn</th>
                        <th class="px-4 py-3 text-center text-xs font-semibold text-blue-600 uppercase">Chuyển Lẻ</th>
                      </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-200 bg-white">
                      <tr v-for="tk in dcnbTonKhoList" :key="tk.id" class="hover:bg-slate-50">
                        <td class="px-4 py-3 text-sm font-medium text-slate-900">{{ tk.maHang }}</td>
                        <td class="px-4 py-3 text-sm text-slate-700">{{ tk.tenSanPham }}</td>
                        <td class="px-4 py-3 text-sm text-center text-slate-600">{{ tk.ngaySanXuat ? new Date(tk.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                        <td class="px-4 py-3 text-sm text-center text-slate-600">{{ tk.hanSuDung ? new Date(tk.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                        <td class="px-4 py-3 text-sm text-center font-medium">{{ tk.soLuongPalletChan }}</td>
                        <td class="px-4 py-3 text-sm text-center font-medium">{{ tk.soThungLe }}</td>
                        <td class="px-4 py-3">
                          <input v-model.number="tk.xuatChan" type="number" min="0" :max="tk.soLuongPalletChan" class="w-20 px-2 py-1.5 text-sm text-center border border-blue-300 rounded focus:ring-1 focus:ring-blue-500" />
                        </td>
                        <td class="px-4 py-3">
                          <input v-model.number="tk.xuatLe" type="number" min="0" :max="tk.soThungLe" class="w-20 px-2 py-1.5 text-sm text-center border border-blue-300 rounded focus:ring-1 focus:ring-blue-500" />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <div class="p-4 bg-slate-50 border-t border-slate-200 flex justify-end">
                  <button @click="addDcnbToDraft" class="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg shadow-sm">
                    Thêm vào lệnh chuyển
                  </button>
                </div>
              </div>
              <div v-else-if="dcnbViTriDi && !dcnbLoadingTonKho" class="text-center py-6 text-slate-500 bg-slate-50 rounded-lg mt-4 border border-dashed border-slate-300">
                Không có hàng hoá tại vị trí này
              </div>
            </div>

            <!-- Vị trí đến & Danh sách lệnh -->
            <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm">
              <div class="flex items-end gap-4 mb-6">
                <div class="flex-1">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Vị trí chuyển đến (Đích)</label>
                  <input v-model="dcnbViTriDenInput" type="text" class="w-full px-4 py-2.5 rounded-lg border border-slate-300 focus:ring-2 focus:ring-blue-500 uppercase font-medium text-slate-900" placeholder="VD: B05" />
                </div>
                <button @click="openLayoutModalFor('dcnb_den')" class="px-6 py-2.5 bg-blue-100 hover:bg-blue-200 text-blue-700 font-bold rounded-lg shadow-sm transition-colors flex items-center justify-center gap-2 h-[46px]">
                  <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 20l-5.447-2.724A1 1 0 013 16.382V5.618a1 1 0 011.447-.894L9 7m0 13l6-3m-6 3V7m6 10l4.553 2.276A1 1 0 0021 18.382V7.618a1 1 0 00-.553-.894L15 4m0 13V4m0 0L9 7"></path></svg>
                  Chọn vị trí
                </button>
              </div>

              <h3 class="text-lg font-bold text-slate-800 mb-4">Chi tiết lệnh điều chuyển trong kho</h3>

              <div class="border border-slate-200 rounded-lg overflow-hidden mb-6">
                <table class="w-full whitespace-nowrap">
                  <thead class="bg-slate-50">
                    <tr>
                      <th class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase">Mã SP</th>
                      <th class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase">Tên SP</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">NSX</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">HSD</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Vị trí đi</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Chuyển Chẵn</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Chuyển Lẻ</th>
                      <th class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-100 bg-white">
                    <tr v-for="(item, index) in dcnbDraftItems" :key="index">
                      <td class="px-4 py-3 text-sm font-medium">{{ item.maSanPham }}</td>
                      <td class="px-4 py-3 text-sm truncate max-w-[200px]" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                      <td class="px-4 py-3 text-sm text-center text-slate-600">{{ formatDate(item.ngaySanXuat) }}</td>
                      <td class="px-4 py-3 text-sm text-center text-slate-600">{{ formatDate(item.hanSuDung) }}</td>
                      <td class="px-4 py-3 text-sm text-center font-bold text-slate-700">{{ item.viTriDi }}</td>
                      <td class="px-4 py-3 text-sm text-center text-blue-600 font-semibold">{{ item.soLuongChan }}</td>
                      <td class="px-4 py-3 text-sm text-center text-blue-600 font-semibold">{{ item.soLuongLe }}</td>
                      <td class="px-4 py-3 text-center">
                        <button @click="removeDcnbDraftItem(index)" class="text-red-500 hover:text-red-700">
                          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                        </button>
                      </td>
                    </tr>
                    <tr v-if="dcnbDraftItems.length === 0">
                      <td colspan="8" class="px-4 py-8 text-center text-slate-500 italic">Chưa có hàng hoá nào được thêm vào lệnh</td>
                    </tr>
                  </tbody>
                </table>
              </div>
              
              <div class="flex justify-end">
                <button @click="createLenhDcnb" :disabled="dcnbDraftItems.length === 0 || isCreatingLenh" class="px-8 py-3 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-lg shadow-md disabled:opacity-50 transition-colors text-lg flex items-center gap-2">
                  Tạo lệnh chuyển
                  <svg v-if="isCreatingLenh" class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
                </button>
              </div>
            </div>
          </div>
          
          <!-- Tab Đang điều chuyển -->
          <div v-else-if="activeDieuChuyenTab === 'dangdieuchuyen'" class="flex-1 overflow-y-auto">
            <div class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden">
              <div v-if="loadingPendingLenhDCNB" class="p-8 text-center">
                <div class="inline-block animate-spin rounded-full h-8 w-8 border-4 border-slate-200 border-t-blue-600"></div>
                <p class="mt-2 text-slate-500">Đang tải...</p>
              </div>
              <div v-else-if="pendingLenhDCNBList.length === 0" class="p-12 text-center">
                <div class="w-16 h-16 bg-slate-50 text-slate-300 rounded-full flex items-center justify-center mx-auto mb-4">
                  <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path></svg>
                </div>
                <h3 class="text-lg font-medium text-slate-800 mb-1">Không có lệnh đang điều chuyển</h3>
                <p class="text-slate-500 text-sm">Các lệnh sau khi tạo sẽ hiển thị ở đây.</p>
              </div>
              <div v-else class="divide-y divide-slate-200">
                <div v-for="lenh in pendingLenhDCNBList" :key="lenh.id" class="p-5 hover:bg-slate-50 transition-colors">
                  <div class="flex justify-between items-start mb-4">
                    <div>
                      <h4 class="font-bold text-lg text-slate-800 flex items-center gap-2">
                        {{ lenh.maLenh }}
                        <span v-if="!lenh.trangThai || lenh.trangThai === 'Pending'" class="px-2 py-0.5 text-xs rounded-full bg-amber-100 text-amber-700 font-medium">Pending</span>
                        <span v-else class="px-2 py-0.5 text-xs rounded-full bg-emerald-100 text-emerald-700 font-medium capitalize">{{ lenh.trangThai }}</span>
                      </h4>
                      <p class="text-sm text-slate-500 mt-1">Người tạo: <span class="font-medium text-slate-700">{{ lenh.nguoiTao }}</span> - {{ new Date(lenh.thoiGianTao).toLocaleString('vi-VN') }}</p>
                    </div>
                    <div class="flex gap-2">
                      <button @click="printLenhDcnb(lenh)" class="px-4 py-2 border border-blue-600 text-blue-600 rounded-lg hover:bg-blue-50 font-medium text-sm flex items-center gap-1 transition-colors">
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 17h2a2 2 0 002-2v-4a2 2 0 00-2-2H5a2 2 0 00-2 2v4a2 2 0 002 2h2m2 4h6a2 2 0 002-2v-4a2 2 0 00-2-2H9a2 2 0 00-2 2v4a2 2 0 002 2zm8-12V5a2 2 0 00-2-2H9a2 2 0 00-2 2v4h10z"></path></svg>
                        In lệnh
                      </button>
                      <button @click="completeLenhDcnb(lenh)" :disabled="isCompletingLenh || lenh.trangThai === 'updated'" :class="['px-4 py-2 text-white rounded-lg font-medium text-sm transition-colors shadow-sm disabled:opacity-50', lenh.trangThai === 'updated' ? 'bg-slate-400 cursor-not-allowed' : 'bg-emerald-600 hover:bg-emerald-700']">
                        Hoàn thành lệnh
                      </button>
                    </div>
                  </div>
                  
                  <div class="bg-white border border-slate-200 rounded-lg overflow-hidden">
                    <table class="w-full text-sm">
                      <thead class="bg-slate-50 border-b border-slate-200 text-xs text-slate-500 uppercase">
                        <tr>
                          <th class="px-4 py-2 text-left font-semibold">Mã Hàng</th>
                          <th class="px-4 py-2 text-left font-semibold">Tên Sản Phẩm</th>
                          <th class="px-4 py-2 text-center font-semibold">NSX</th>
                          <th class="px-4 py-2 text-center font-semibold">HSD</th>
                          <th class="px-4 py-2 text-center font-semibold">Từ Vị trí</th>
                          <th class="px-4 py-2 text-center font-semibold">Đến Vị trí</th>
                          <th class="px-4 py-2 text-right font-semibold">Số Chẵn</th>
                          <th class="px-4 py-2 text-right font-semibold">Số Lẻ</th>
                        </tr>
                      </thead>
                      <tbody class="divide-y divide-slate-100">
                        <tr v-for="ct in lenh.chiTiets" :key="ct.id">
                          <td class="px-4 py-2 font-medium">{{ ct.maSanPham }}</td>
                          <td class="px-4 py-2 text-slate-600 truncate max-w-[200px]" :title="ct.tenSanPham">{{ ct.tenSanPham }}</td>
                          <td class="px-4 py-2 text-center text-slate-600">{{ formatDate(ct.ngaySanXuat) }}</td>
                          <td class="px-4 py-2 text-center text-slate-600">{{ formatDate(ct.hanSuDung) }}</td>
                          <td class="px-4 py-2 text-center font-bold text-amber-600">{{ ct.viTriDi }}</td>
                          <td class="px-4 py-2 text-center font-bold text-emerald-600">{{ ct.viTriDen }}</td>
                          <td class="px-4 py-2 text-right font-semibold">{{ ct.soLuongChan }}</td>
                          <td class="px-4 py-2 text-right font-semibold">{{ ct.soLuongLe }}</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-else-if="activeMenu === 'baocao'" class="bg-white rounded-xl shadow-sm border border-slate-200 h-full flex flex-col overflow-hidden">
          
          <!-- Report Menu (Grid of Cards) -->
          <div v-if="!activeReport" class="p-8 h-full bg-slate-50 overflow-y-auto">
            <h2 class="text-2xl font-bold text-slate-800 mb-6">Danh mục Báo cáo</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
              <!-- Báo cáo tồn kho Card -->
              <div @click="openReport('tonkho')" class="bg-white p-6 rounded-xl shadow-sm border border-slate-200 hover:shadow-md hover:border-blue-300 transition-all cursor-pointer group flex flex-col items-center text-center">
                <div class="w-16 h-16 bg-blue-50 text-blue-600 rounded-full flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                  <svg class="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                </div>
                <h3 class="font-bold text-lg text-slate-800 mb-2">Báo cáo tồn kho</h3>
                <p class="text-sm text-slate-500">Xem danh sách hàng hoá hiện đang tồn trong kho theo thời gian thực.</p>
              </div>

              <!-- Báo cáo nhập xuất Card -->
              <div @click="openReport('nhapxuat')" class="bg-white p-6 rounded-xl shadow-sm border border-slate-200 hover:shadow-md hover:border-emerald-300 transition-all cursor-pointer group flex flex-col items-center text-center">
                <div class="w-16 h-16 bg-emerald-50 text-emerald-600 rounded-full flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                  <svg class="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4"></path></svg>
                </div>
                <h3 class="font-bold text-lg text-slate-800 mb-2">Báo cáo nhập xuất</h3>
                <p class="text-sm text-slate-500">Xem danh sách phát sinh nhập xuất hàng hoá theo ngày.</p>
              </div>
              
              <!-- Báo cáo đếm kho Card -->
              <div @click="openReport('demkho')" class="bg-white p-6 rounded-xl shadow-sm border border-slate-200 hover:shadow-md hover:border-amber-300 transition-all cursor-pointer group flex flex-col items-center text-center">
                <div class="w-16 h-16 bg-amber-50 text-amber-600 rounded-full flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                  <svg class="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path></svg>
                </div>
                <h3 class="font-bold text-lg text-slate-800 mb-2">Báo cáo đếm kho</h3>
                <p class="text-sm text-slate-500">Kiểm duyệt kết quả đếm kho thực tế và xem lịch sử đếm kho.</p>
              </div>

              <!-- Báo cáo điều chuyển Card -->
              <div @click="openReport('dieuchuyen')" class="bg-white p-6 rounded-xl shadow-sm border border-slate-200 hover:shadow-md hover:border-indigo-300 transition-all cursor-pointer group flex flex-col items-center text-center">
                <div class="w-16 h-16 bg-indigo-50 text-indigo-600 rounded-full flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                  <svg class="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4"></path></svg>
                </div>
                <h3 class="font-bold text-lg text-slate-800 mb-2">Báo cáo điều chuyển</h3>
                <p class="text-sm text-slate-500">Xem lịch sử các lệnh điều chuyển nội bộ giữa các vị trí.</p>
              </div>
            </div>
          </div>

          <!-- Báo Cáo Tồn Kho View -->
          <div v-else-if="activeReport === 'tonkho'" class="flex flex-col h-full overflow-hidden">
            <div class="px-6 py-5 border-b border-slate-200 bg-slate-50 flex justify-between items-center">
              <div class="flex items-center gap-4">
                <button @click="closeReport" class="text-slate-400 hover:text-slate-600 p-2 hover:bg-slate-200 rounded-full transition-colors" title="Quay lại danh mục báo cáo">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
                </button>
                <div>
                  <h2 class="text-xl font-bold text-slate-800">Báo cáo tồn kho</h2>
                  <p class="text-sm text-slate-500 mt-1">
                    Danh sách tồn kho hàng hiện tại 
                    <span class="font-bold text-blue-600 ml-2" v-if="khohangInfo">Mã Kho: {{ khohangInfo.name }}</span>
                  </p>
                </div>
              </div>
              <button @click="exportBaocaoTonKhoExcel" :disabled="filteredBaocaoTonKhoData.length === 0" class="px-4 py-2 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-emerald-200 rounded-lg shadow-sm flex items-center gap-2 font-medium">
                <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path></svg>
                Kết xuất Excel
              </button>
            </div>

            <!-- Tab bar Báo cáo tồn kho -->
            <div class="bg-white px-4 border-b border-slate-200 mt-4 rounded-xl shadow-sm">
              <div class="flex space-x-6">
                <button @click="baocaoTonKhoActiveTab = 'Thành phẩm'" :class="baocaoTonKhoActiveTab === 'Thành phẩm' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="py-3 px-2 flex items-center gap-2">
                  Thành phẩm
                  <span class="bg-blue-100 text-blue-700 text-xs py-0.5 px-2 rounded-full font-bold">{{ baocaoTonKhoThanhPhamCount }}</span>
                </button>
                <button @click="baocaoTonKhoActiveTab = 'Vỏ'" :class="baocaoTonKhoActiveTab === 'Vỏ' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="py-3 px-2 flex items-center gap-2">
                  Vỏ
                  <span class="bg-amber-100 text-amber-700 text-xs py-0.5 px-2 rounded-full font-bold">{{ baocaoTonKhoVoCount }}</span>
                </button>
              </div>
            </div>
            
            <div class="flex-1 overflow-auto custom-scrollbar p-0">
              <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                  <tr v-if="baocaoTonKhoActiveTab === 'Thành phẩm'">
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN SẢN PHẨM</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">HẠN SỬ DỤNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-center">% HSD CÒN LẠI</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">SL PALLET CHẴN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">SỐ THÙNG LẺ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">ĐỊNH LƯỢNG (CS/PL)</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">TỔNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">GHI CHÚ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TRẠNG THÁI D.O.D</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGÀY SẢN XUẤT</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">DÃY HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">STT DÃY HÀNG</th>
                  </tr>
                  <tr v-else>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">CHẴN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">LẺ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">ĐỊNH LƯỢNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">TỔNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">GHI CHÚ</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="baocaoTonKhoLoading" class="bg-white">
                    <td :colspan="baocaoTonKhoActiveTab === 'Thành phẩm' ? 14 : 8" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="filteredBaocaoTonKhoData.length === 0" class="bg-white">
                    <td :colspan="baocaoTonKhoActiveTab === 'Thành phẩm' ? 14 : 8" class="py-8 text-center text-slate-500">Không có dữ liệu tồn kho</td>
                  </tr>
                  <template v-else>
                    <tr v-for="item in filteredBaocaoTonKhoData" :key="item.id" class="border-b border-slate-100 hover:bg-blue-50/50 transition-colors">
                      <template v-if="baocaoTonKhoActiveTab === 'Thành phẩm'">
                        <td class="py-2 px-4 border-r border-slate-100">{{ item.viTri }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 font-semibold">{{ item.maHang }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 min-w-[200px] truncate max-w-xs" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-slate-600">{{ item.hanSuDung ? new Date(item.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-center font-medium text-slate-600">{{ getRemainingShelfLifePercentage(item.ngaySanXuat, item.hanSuDung) }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soLuongPalletChan !== null && item.soLuongPalletChan !== 0 ? item.soLuongPalletChan : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soThungLe !== null && item.soThungLe !== 0 ? item.soThungLe.toLocaleString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.dinhLuong !== null && item.dinhLuong !== 0 ? item.dinhLuong : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right font-bold text-slate-900">{{ item.tong !== null && item.tong !== 0 ? item.tong.toLocaleString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100"></td>
                        <td class="py-2 px-4 border-r border-slate-100 text-slate-700">{{ item.trangThaiDOD || 'NORMAL-BÌNH THƯỜNG' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-slate-600">{{ item.ngaySanXuat ? new Date(item.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-slate-600">{{ parseViTri(item.viTri).day }}</td>
                        <td class="py-2 px-4 text-slate-600">{{ parseViTri(item.viTri).stt }}</td>
                      </template>
                      <template v-else>
                        <td class="py-2 px-4 border-r border-slate-100 font-medium text-slate-800">{{ item.viTri }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 font-semibold text-slate-800">{{ item.maHang }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 min-w-[200px] truncate max-w-xs text-slate-800" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soLuongPalletChan !== null && item.soLuongPalletChan !== 0 ? item.soLuongPalletChan : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soThungLe !== null && item.soThungLe !== 0 ? item.soThungLe.toLocaleString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.dinhLuong !== null && item.dinhLuong !== 0 ? item.dinhLuong : '' }}</td>
                        <td class="py-2 px-4 border-r border-slate-100 text-right font-bold text-slate-900">{{ item.tong !== null && item.tong !== 0 ? item.tong.toLocaleString('vi-VN') : '' }}</td>
                        <td class="py-2 px-4"></td>
                      </template>
                    </tr>
                  </template>
                </tbody>
              </table>
            </div>
          </div>

          <!-- Báo Cáo Nhập Xuất View -->
          <div v-else-if="activeReport === 'nhapxuat'" class="flex flex-col h-full overflow-hidden">
            <div class="px-6 py-5 border-b border-slate-200 bg-slate-50 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div class="flex items-center gap-4">
                <button @click="closeReport" class="text-slate-400 hover:text-slate-600 p-2 hover:bg-slate-200 rounded-full transition-colors" title="Quay lại danh mục báo cáo">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
                </button>
                <div>
                  <h2 class="text-xl font-bold text-slate-800">Báo cáo nhập xuất</h2>
                  <p class="text-sm text-slate-500 mt-1">
                    Danh sách nhập/xuất kho theo ngày
                    <span class="font-bold text-emerald-600 ml-2" v-if="khohangInfo">Mã Kho: {{ khohangInfo.name }}</span>
                  </p>
                </div>
              </div>
              
              <div class="flex items-center gap-3">
                <input type="date" v-model="nhapxuatDate" @change="fetchBaocaoNhapXuat" class="border border-slate-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500">
                <button @click="exportBaocaoNhapXuatExcel" :disabled="baocaoNhapXuatData.length === 0" class="px-4 py-2 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-emerald-200 rounded-lg shadow-sm flex items-center gap-2 font-medium">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path></svg>
                  Kết xuất Excel
                </button>
              </div>
            </div>
            
            <div class="flex-1 overflow-auto custom-scrollbar p-0">
              <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                  <tr>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">THỜI GIAN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">LOẠI</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN SẢN PHẨM</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NSX</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">HSD</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SỐ LƯỢNG CHẴN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SỐ LƯỢNG LẺ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SỐ XE</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">CHỨNG TỪ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">NGƯỜI THAO TÁC</th>
                  </tr>
                </thead>
                <tbody class="divide-y divide-slate-100 bg-white">
                  <tr v-if="baocaoNhapXuatLoading" class="bg-white">
                    <td colspan="12" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="baocaoNhapXuatData.length === 0" class="bg-white">
                    <td colspan="12" class="py-8 text-center text-slate-500">Không có dữ liệu phát sinh trong ngày này.</td>
                  </tr>
                  <tr v-for="item in baocaoNhapXuatData" :key="item.id" class="hover:bg-blue-50/50 transition-colors">
                    <td class="py-3 px-4 border-r border-slate-100">{{ formatDate(item.thoiGian, true) }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 font-bold" :class="item.loaiPhatSinh === 'Nhập' ? 'text-blue-600' : 'text-red-600'">{{ item.loaiPhatSinh }}</td>
                    <td class="py-3 px-4 border-r border-slate-100">{{ item.maSanPham }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 whitespace-normal min-w-[200px]">{{ item.tenSanPham }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 text-slate-600">{{ item.ngaySanXuat ? new Date(item.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 text-slate-600">{{ item.hanSuDung ? new Date(item.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 font-medium">{{ item.viTri }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 text-right">{{ item.soLuongChan }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 text-right">{{ item.soLuongLe }}</td>
                    <td class="py-3 px-4 border-r border-slate-100">{{ item.sothe }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 text-xs">{{ item.soSTO || item.soShipment || '-' }}</td>
                    <td class="py-3 px-4">{{ item.tenNhanVien || item.user }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- Báo Cáo Đếm Kho View -->
          <div v-else-if="activeReport === 'demkho'" class="flex flex-col h-full overflow-hidden">
            <div class="px-6 py-5 border-b border-slate-200 bg-slate-50 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div class="flex items-center gap-4">
                <button @click="closeReport" class="text-slate-400 hover:text-slate-600 p-2 hover:bg-slate-200 rounded-full transition-colors" title="Quay lại danh mục báo cáo">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
                </button>
                <div>
                  <h2 class="text-xl font-bold text-slate-800">Báo cáo đếm kho</h2>
                  <p class="text-sm text-slate-500 mt-1">
                    Danh sách các phiên đếm kho
                    <span class="font-bold text-amber-600 ml-2" v-if="khohangInfo">Mã Kho: {{ khohangInfo.name }}</span>
                  </p>
                </div>
              </div>
            </div>
            
            <div class="px-6 pt-4 border-b border-slate-200 bg-white shrink-0">
              <div class="flex space-x-6">
                <button @click="activeBaocaoDemKhoTab = 'choduyet'" :class="activeBaocaoDemKhoTab === 'choduyet' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2 flex items-center gap-2 transition-colors">
                  Chờ duyệt
                  <span v-if="pendingDemKhoReports.length > 0" class="bg-amber-100 text-amber-700 text-xs py-0.5 px-2 rounded-full font-bold">{{ pendingDemKhoReports.length }}</span>
                </button>
                <button @click="activeBaocaoDemKhoTab = 'lichsu'" :class="activeBaocaoDemKhoTab === 'lichsu' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2 transition-colors">
                  Lịch sử đếm kho
                </button>
              </div>
            </div>

            <div class="flex-1 overflow-auto custom-scrollbar p-0 bg-slate-50/50">
              <!-- Tab Chờ duyệt -->
              <div v-if="activeBaocaoDemKhoTab === 'choduyet'">
                <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                  <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                    <tr>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">ID</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">THỜI GIAN TẠO</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGƯỜI ĐẾM</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">GHI CHÚ</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200 text-center">THAO TÁC</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-100 bg-white">
                    <tr v-if="baocaoDemKhoLoading" class="bg-white">
                      <td colspan="6" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                    </tr>
                    <tr v-else-if="pendingDemKhoReports.length === 0" class="bg-white">
                      <td colspan="6" class="py-8 text-center text-slate-500">Không có phiếu đếm kho nào cần duyệt.</td>
                    </tr>
                    <tr v-for="item in pendingDemKhoReports" :key="item.id" @dblclick="openDemKhoReportDetail(item)" class="hover:bg-blue-50/50 transition-colors cursor-pointer group">
                      <td class="py-3 px-4 border-r border-slate-100 font-bold text-slate-700">#{{ item.id }}</td>
                      <td class="py-3 px-4 border-r border-slate-100 font-bold text-blue-700">{{ item.viTri }}</td>
                      <td class="py-3 px-4 border-r border-slate-100">{{ formatDate(item.thoiGianTao, true) }}</td>
                      <td class="py-3 px-4 border-r border-slate-100">{{ item.nguoiDem }}</td>
                      <td class="py-3 px-4 border-r border-slate-100 truncate max-w-[200px]" :title="item.ghiChu">{{ item.ghiChu }}</td>
                      <td class="py-2 px-4 text-center">
                        <button @click.stop="openDemKhoReportDetail(item)" class="px-3 py-1.5 bg-white border border-slate-300 text-slate-700 rounded-lg hover:bg-slate-50 text-xs font-semibold mr-2 transition-colors">Chi tiết</button>
                        <button @click.stop="duyetDemKho(item.id)" class="px-3 py-1.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 text-xs font-bold mr-2 transition-colors shadow-sm">Duyệt</button>
                        <button @click.stop="khongDuyetDemKho(item.id)" class="px-3 py-1.5 bg-red-50 text-red-600 border border-red-200 rounded-lg hover:bg-red-100 text-xs font-semibold transition-colors">Không Duyệt</button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              
              <!-- Tab Lịch sử -->
              <div v-else-if="activeBaocaoDemKhoTab === 'lichsu'">
                <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                  <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                    <tr>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">ID</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">THỜI GIAN TẠO</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGƯỜI ĐẾM</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-center">TRẠNG THÁI</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGƯỜI DUYỆT</th>
                      <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">THỜI GIAN DUYỆT</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-100 bg-white">
                    <tr v-if="baocaoDemKhoLoading" class="bg-white">
                      <td colspan="7" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                    </tr>
                    <tr v-else-if="historyDemKhoReports.length === 0" class="bg-white">
                      <td colspan="7" class="py-8 text-center text-slate-500">Chưa có lịch sử đếm kho.</td>
                    </tr>
                    <tr v-for="item in historyDemKhoReports" :key="item.id" @dblclick="openDemKhoReportDetail(item)" class="hover:bg-slate-50 transition-colors cursor-pointer group" title="Nháy đúp để xem chi tiết">
                      <td class="py-3 px-4 border-r border-slate-100 font-bold text-slate-700">#{{ item.id }}</td>
                      <td class="py-3 px-4 border-r border-slate-100 font-bold text-slate-800">{{ item.viTri }}</td>
                      <td class="py-3 px-4 border-r border-slate-100">{{ formatDate(item.thoiGianTao, true) }}</td>
                      <td class="py-3 px-4 border-r border-slate-100">{{ item.nguoiDem }}</td>
                      <td class="py-3 px-4 border-r border-slate-100 text-center">
                        <span :class="item.trangThai === 'Duyệt' ? 'bg-emerald-100 text-emerald-700' : 'bg-red-100 text-red-700'" class="px-2.5 py-1 rounded-full text-xs font-bold">
                          {{ item.trangThai }}
                        </span>
                      </td>
                      <td class="py-3 px-4 border-r border-slate-100">{{ item.nguoiDuyet }}</td>
                      <td class="py-3 px-4">{{ item.thoiGianDuyet ? formatDate(item.thoiGianDuyet, true) : '-' }}</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>

          <!-- Báo Cáo Điều Chuyển View -->
          <div v-else-if="activeReport === 'dieuchuyen'" class="flex flex-col h-full overflow-hidden">
            <div class="px-6 py-5 border-b border-slate-200 bg-slate-50 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div class="flex items-center gap-4">
                <button @click="closeReport" class="text-slate-400 hover:text-slate-600 p-2 hover:bg-slate-200 rounded-full transition-colors" title="Quay lại danh mục báo cáo">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
                </button>
                <div>
                  <h2 class="text-xl font-bold text-slate-800">Báo cáo điều chuyển hàng nội bộ</h2>
                  <p class="text-sm text-slate-500 mt-1">
                    Danh sách các lệnh điều chuyển nội bộ đã thực hiện
                    <span class="font-bold text-indigo-600 ml-2" v-if="khohangInfo">Mã Kho: {{ khohangInfo.name }}</span>
                  </p>
                </div>
              </div>
              
              <div class="flex flex-wrap items-center gap-3">
                <div class="flex flex-wrap items-center gap-3 bg-slate-100 p-1.5 rounded-lg border border-slate-200">
                  <div class="flex items-center gap-2">
                    <span class="text-xs font-semibold text-slate-500">Từ ngày:</span>
                    <input type="date" v-model="dieuChuyenFromDate" class="border border-slate-300 rounded px-2 py-1 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 bg-white">
                  </div>
                  <div class="flex items-center gap-2">
                    <span class="text-xs font-semibold text-slate-500">Đến ngày:</span>
                    <input type="date" v-model="dieuChuyenToDate" class="border border-slate-300 rounded px-2 py-1 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 bg-white">
                  </div>
                  <button @click="fetchBaocaoDieuChuyen" class="px-4 py-1.5 bg-blue-600 hover:bg-blue-700 text-white rounded text-sm font-bold transition-colors shadow-sm">
                    Thực hiện
                  </button>
                </div>
                <button @click="exportBaocaoDieuChuyenExcel" :disabled="baocaoDieuChuyenData.length === 0" class="px-4 py-2 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-emerald-200 rounded-lg shadow-sm flex items-center gap-2 font-medium">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path></svg>
                  Kết xuất Excel
                </button>
              </div>
            </div>
            
            <div class="flex-1 overflow-auto custom-scrollbar p-0">
              <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                  <tr>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ LỆNH</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">THỜI GIAN HT</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGƯỜI THỰC HIỆN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ SP</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN SẢN PHẨM</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TỪ VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">ĐẾN VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200 text-right">SL CHẴN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200 text-right">SL LẺ</th>
                  </tr>
                </thead>
                <tbody class="divide-y divide-slate-100 bg-white">
                  <tr v-if="baocaoDieuChuyenLoading" class="bg-white">
                    <td colspan="9" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="baocaoDieuChuyenData.length === 0" class="bg-white">
                    <td colspan="9" class="py-8 text-center text-slate-500">Không có dữ liệu điều chuyển nào trong khoảng thời gian này.</td>
                  </tr>
                  <template v-else v-for="lenh in baocaoDieuChuyenData" :key="lenh.id">
                    <tr v-for="(ct, idx) in lenh.chiTiets" :key="ct.id" class="hover:bg-blue-50/50 transition-colors">
                      <td v-if="idx === 0" :rowspan="lenh.chiTiets.length" class="py-3 px-4 border-r border-slate-200 font-bold text-slate-800 align-top">
                        {{ lenh.maLenh }}
                      </td>
                      <td v-if="idx === 0" :rowspan="lenh.chiTiets.length" class="py-3 px-4 border-r border-slate-200 text-slate-600 align-top">
                        {{ formatDate(lenh.thoiGianHoanThanh, true) }}
                      </td>
                      <td v-if="idx === 0" :rowspan="lenh.chiTiets.length" class="py-3 px-4 border-r border-slate-200 text-slate-600 align-top">
                        {{ lenh.nguoiHoanThanh || '-' }}
                      </td>
                      <td class="py-2 px-4 border-r border-slate-100">{{ ct.maSanPham }}</td>
                      <td class="py-2 px-4 border-r border-slate-100 whitespace-normal min-w-[200px]">{{ ct.tenSanPham }}</td>
                      <td class="py-2 px-4 border-r border-slate-100 font-bold text-amber-600 text-center">{{ ct.viTriDi }}</td>
                      <td class="py-2 px-4 border-r border-slate-100 font-bold text-emerald-600 text-center">{{ ct.viTriDen }}</td>
                      <td class="py-2 px-4 border-r border-slate-100 text-right">{{ ct.soLuongChan }}</td>
                      <td class="py-2 px-4 text-right">{{ ct.soLuongLe }}</td>
                    </tr>
                  </template>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        </main>
      </div>
    </div>

    <!-- Modal Chi Tiết Phiếu Đếm Kho -->
    <div v-if="selectedDemKhoReport" class="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-5xl shadow-xl flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50 rounded-t-xl">
          <div>
            <h3 class="text-lg font-bold text-slate-800">Chi tiết phiếu đếm kho #{{ selectedDemKhoReport.id }}</h3>
            <div class="flex items-center gap-3 text-sm text-slate-500 mt-1">
              <span>Vị trí: <strong class="text-slate-700">{{ selectedDemKhoReport.viTri }}</strong></span>
              <span>•</span>
              <span>Người đếm: <strong>{{ selectedDemKhoReport.nguoiDem }}</strong></span>
              <span>•</span>
              <span>Thời gian: {{ formatDate(selectedDemKhoReport.thoiGianTao, true) }}</span>
            </div>
          </div>
          <button @click="closeDemKhoReportDetail" class="text-slate-400 hover:text-slate-600 p-2 hover:bg-slate-200 rounded-full transition-colors">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1 custom-scrollbar">
          <table class="w-full text-left border-collapse text-sm">
            <thead class="bg-slate-100 sticky top-0 shadow-sm">
              <tr>
                <th class="py-2 px-3 font-semibold text-slate-700 border-b border-r border-slate-200">Mã SP</th>
                <th class="py-2 px-3 font-semibold text-slate-700 border-b border-r border-slate-200">Tên SP</th>
                <th class="py-2 px-3 font-semibold text-slate-700 border-b border-r border-slate-200">NSX</th>
                <th class="py-2 px-3 font-semibold text-slate-700 border-b border-r border-slate-200">HSD</th>
                <th class="py-2 px-3 font-semibold text-blue-700 border-b border-r border-blue-200 bg-blue-50 text-center" colspan="2">HIỆN TẠI</th>
                <th class="py-2 px-3 font-semibold text-amber-700 border-b border-r border-amber-200 bg-amber-50 text-center" colspan="2">SỐ ĐẾM</th>
                <th class="py-2 px-3 font-semibold text-red-700 border-b border-slate-200 bg-red-50 text-center" colspan="2">CHÊNH LỆCH</th>
              </tr>
              <tr>
                <th class="py-2 px-3 border-b border-r border-slate-200"></th>
                <th class="py-2 px-3 border-b border-r border-slate-200"></th>
                <th class="py-2 px-3 border-b border-r border-slate-200"></th>
                <th class="py-2 px-3 border-b border-r border-slate-200"></th>
                <th class="py-2 px-2 text-xs text-center border-b border-r border-blue-200 bg-blue-50/50">Pallet</th>
                <th class="py-2 px-2 text-xs text-center border-b border-r border-blue-200 bg-blue-50/50">Lẻ</th>
                <th class="py-2 px-2 text-xs text-center border-b border-r border-amber-200 bg-amber-50/50">Pallet</th>
                <th class="py-2 px-2 text-xs text-center border-b border-r border-amber-200 bg-amber-50/50">Lẻ</th>
                <th class="py-2 px-2 text-xs text-center border-b border-r border-red-200 bg-red-50/50">Pallet</th>
                <th class="py-2 px-2 text-xs text-center border-b border-slate-200 bg-red-50/50">Lẻ</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="ct in selectedDemKhoReport.chiTiets" :key="ct.id" class="border-b border-slate-100 hover:bg-slate-50 transition-colors">
                <td class="py-2 px-3 border-r border-slate-100 font-medium">{{ ct.maSanPham }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-xs">{{ ct.tenSanPham }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-xs">{{ ct.ngaySanXuat ? new Date(ct.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-xs">{{ ct.hanSuDung ? new Date(ct.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-center bg-blue-50/10">{{ ct.tonHienTaiChan }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-center bg-blue-50/10">{{ ct.tonHienTaiLe }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-center bg-amber-50/20 font-bold">{{ ct.soDemChan }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-center bg-amber-50/20 font-bold">{{ ct.soDemLe }}</td>
                <td class="py-2 px-3 border-r border-slate-100 text-center bg-red-50/30" :class="ct.chenhLechChan !== 0 ? 'text-red-600 font-bold' : 'text-slate-400'">{{ ct.chenhLechChan }}</td>
                <td class="py-2 px-3 text-center bg-red-50/30" :class="ct.chenhLechLe !== 0 ? 'text-red-600 font-bold' : 'text-slate-400'">{{ ct.chenhLechLe }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        
        <div class="px-6 py-4 border-t border-slate-200 bg-slate-50 flex justify-end gap-3 rounded-b-xl">
          <button @click="closeDemKhoReportDetail" class="px-5 py-2.5 bg-white border border-slate-300 text-slate-700 font-bold rounded-lg hover:bg-slate-50 transition-colors shadow-sm">Đóng</button>
          <template v-if="selectedDemKhoReport.trangThai === 'Chờ duyệt'">
            <button @click="khongDuyetDemKho(selectedDemKhoReport.id)" class="px-5 py-2.5 bg-red-50 text-red-600 font-bold rounded-lg hover:bg-red-100 border border-red-200 transition-colors shadow-sm">Không Duyệt</button>
            <button @click="duyetDemKho(selectedDemKhoReport.id)" class="px-5 py-2.5 bg-blue-600 text-white font-bold rounded-lg hover:bg-blue-700 transition-colors shadow-sm">Duyệt Phiếu</button>
          </template>
        </div>
      </div>
    </div>

    <!-- Modal 1: Chi tiết xuất kho (Shipment & STO) -->
    <div v-if="showXuatKhoModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-lg shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Chi tiết xe xuất: {{ selectedXuatKhoXe?.bienSo }}</h3>
          <button @click="closeXuatKhoModal" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="p-6 overflow-y-auto">
          <div class="mb-6">
            <h4 class="font-semibold text-slate-700 mb-3 border-b pb-2">Số Shipment <span class="text-xs text-slate-400 font-normal">(Nhấn để chọn xuất kho)</span></h4>
            <div v-if="selectedXuatKhoXe?.shipments?.length" class="space-y-4">
              <div v-for="shipment in selectedXuatKhoXe.shipments" :key="shipment" class="flex flex-col gap-2">
                <div @click="openXuatKhoShipmentModal(shipment)" class="p-3 bg-blue-50 text-blue-800 rounded-lg font-medium border border-blue-100 cursor-pointer hover:bg-blue-100 transition-colors flex justify-between items-center group shadow-sm">
                  <span>{{ shipment }}</span>
                  <div class="flex items-center gap-2">
                    <span class="text-xs bg-white px-2 py-1 rounded text-blue-600 border border-blue-200 shadow-sm" v-if="draftLoadTickets[shipment]?.length">{{ draftLoadTickets[shipment].length }} mục đã chọn</span>
                    <svg class="w-5 h-5 text-blue-400 group-hover:text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                  </div>
                </div>
                <!-- Draft items list -->
                <div v-if="draftLoadTickets[shipment]?.length" class="pl-4 border-l-2 border-blue-200 space-y-1">
                  <div v-for="item in draftLoadTickets[shipment]" :key="item.id" class="bg-slate-50 text-xs px-3 py-2 rounded border border-slate-200 text-slate-700">
                    <span class="font-bold text-blue-700">{{ item.maHang }}</span> - {{ item.tenSanPham }}
                    <span class="text-slate-500 ml-2">(Vị trí: <span class="font-bold text-slate-700">{{ item.viTri }}</span>)</span>
                    <span class="ml-2 font-medium text-emerald-600">SL: {{ item.xuatChan }} chẵn / {{ item.xuatLe }} lẻ</span>
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="text-slate-500 italic text-sm">Không có Shipment</div>
          </div>
          

        </div>
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-slate-50 gap-3">
          <button @click="closeXuatKhoModal" class="px-4 py-2 bg-white border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-50 font-medium">Đóng</button>
          <button @click="rejectXuatKho" class="px-4 py-2 bg-red-600 text-white border border-transparent rounded-lg hover:bg-red-700 font-bold shadow-sm flex items-center gap-2">
            Hủy yêu cầu
          </button>
          <button @click="submitFinalXuatKho" class="px-4 py-2 bg-blue-600 text-white border border-transparent rounded-lg hover:bg-blue-700 font-bold shadow-sm flex items-center gap-2">
             <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
             Hoàn tất Xuất hàng
          </button>
        </div>
      </div>
    </div>

    <!-- Modal 2: Chọn sản phẩm và vị trí xuất kho -->
    <div v-if="showXuatKhoSTOModal" class="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-4xl shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Xuất kho {{ selectedXuatKhoType }}: {{ selectedXuatKhoDoc }}</h3>
          <button @click="closeXuatKhoSTOModal" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="px-6 pt-4 border-b border-slate-200 bg-white">
          <div class="flex space-x-6">
            <button @click="xuatKhoActiveTab = 'xuatKho'" :class="xuatKhoActiveTab === 'xuatKho' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2">Xuất kho</button>
            <button @click="xuatKhoActiveTab = 'danhSach'" :class="xuatKhoActiveTab === 'danhSach' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2 flex items-center gap-2">
              Chi tiết phiếu xuất
              <span v-if="draftLoadTickets[selectedXuatKhoDoc]?.length" class="bg-blue-100 text-blue-700 text-xs py-0.5 px-2 rounded-full font-bold">{{ draftLoadTickets[selectedXuatKhoDoc].length }}</span>
            </button>
          </div>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1 flex flex-col bg-slate-50/50">
          <!-- Tab 1: Xuất kho -->
          <div v-show="xuatKhoActiveTab === 'xuatKho'" class="flex flex-col gap-6 flex-1">
            <!-- Chọn sản phẩm -->
            <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm flex flex-col md:flex-row gap-4 items-start relative z-10">
              <div class="w-full md:w-1/3 relative">
                <label class="block text-sm font-semibold text-slate-700 mb-2">Mã Sản Phẩm</label>
                <input :value="xuatKhoSearchQuery" 
                       @input="onXuatKhoProductInput"
                       @focus="showXuatKhoProductDropdown = true" 
                       @blur="handleXuatKhoProductBlur" 
                       @keydown.down.prevent="onXuatKhoArrowDown"
                       @keydown.up.prevent="onXuatKhoArrowUp"
                       @keydown.enter="onXuatKhoEnter"
                       type="text" placeholder="Nhập mã..." class="w-full border border-slate-300 rounded-lg px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 bg-slate-50 outline-none font-bold" autocomplete="off" />
                
                <!-- Dropdown -->
                <div v-if="showXuatKhoProductDropdown && filteredXuatKhoProducts.length > 0" ref="xuatKhoDropdownList" class="absolute z-50 w-full left-0 mt-1 bg-white border border-slate-200 rounded-lg shadow-lg max-h-60 overflow-y-auto custom-scrollbar min-w-[300px]">
                  <ul class="py-1 text-sm text-slate-700">
                    <li v-for="(sp, index) in filteredXuatKhoProducts" :key="sp.id" @mousedown.prevent="selectXuatKhoProduct(sp)" :class="['px-4 py-2.5 cursor-pointer flex justify-between items-center transition-colors', selectedXuatKhoProductIndex === index ? 'bg-blue-100' : 'hover:bg-blue-50']">
                      <span class="font-bold text-blue-700 shrink-0">{{ sp.maSanPham }}</span>
                      <span class="text-xs text-slate-500 truncate ml-2 text-right">{{ sp.tenSanPham }}</span>
                    </li>
                  </ul>
                </div>
              </div>
              
              <div class="w-full md:w-2/3 md:pt-7">
                 <div class="flex items-center w-full min-h-[42px] border border-slate-200 rounded-lg px-4 bg-slate-50 text-slate-800 font-bold overflow-hidden">
                   <span class="truncate">{{ productsList.find(p => p.maSanPham === selectedProductIdForXuatKho)?.tenSanPham || 'Chưa chọn sản phẩm...' }}</span>
                 </div>
              </div>
            </div>

            <!-- Kết quả Tồn Kho -->
            <div v-if="selectedProductIdForXuatKho" class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden flex flex-col z-0">
              <div class="p-4 border-b border-slate-100 bg-slate-50 flex justify-between items-center">
                <h4 class="font-semibold text-slate-700">Chọn vị trí và nhập số lượng xuất</h4>
                <button @click="fetchTonKhoLocations" class="text-sm text-blue-600 hover:text-blue-800 flex items-center font-medium">
                  <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15"></path></svg>
                  Làm mới
                </button>
              </div>
              
              <div class="overflow-x-auto">
                <table class="min-w-full divide-y divide-slate-200">
                  <thead class="bg-slate-50">
                    <tr>
                      <th scope="col" class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Vị trí</th>
                      <th @click="handleSortTonKho('ngaySanXuat')" scope="col" class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider cursor-pointer hover:bg-slate-100 select-none">
                        NSX
                        <span v-if="sortTonKhoKey === 'ngaySanXuat'" class="ml-1">{{ sortTonKhoAsc ? '↑' : '↓' }}</span>
                      </th>
                      <th @click="handleSortTonKho('hanSuDung')" scope="col" class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider cursor-pointer hover:bg-slate-100 select-none">
                        HSD
                        <span v-if="sortTonKhoKey === 'hanSuDung'" class="ml-1">{{ sortTonKhoAsc ? '↑' : '↓' }}</span>
                      </th>
                      <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Chẵn</th>
                      <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Lẻ</th>
                      <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Xuất Chẵn</th>
                      <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Xuất Lẻ</th>
                    </tr>
                  </thead>
                  <tbody class="bg-white divide-y divide-slate-100">
                    <tr v-if="loadingTonKho" class="bg-slate-50/50">
                      <td colspan="7" class="px-4 py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                    </tr>
                    <tr v-else-if="!tonKhoList.length" class="bg-slate-50/50">
                      <td colspan="7" class="px-4 py-8 text-center text-slate-500">Không có tồn kho khả dụng cho sản phẩm này</td>
                    </tr>
                    <tr v-for="tk in tonKhoList" :key="tk.id" class="hover:bg-slate-50 transition-colors">
                      <td class="px-4 py-3 text-sm font-medium text-slate-900">{{ tk.viTri }}</td>
                      <td class="px-4 py-3 text-sm text-slate-600">{{ tk.ngaySanXuat ? new Date(tk.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                      <td class="px-4 py-3 text-sm text-slate-600">{{ tk.hanSuDung ? new Date(tk.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                      <td class="px-4 py-3 text-sm text-center text-slate-600 font-medium">{{ tk.soLuongPalletChan }}</td>
                      <td class="px-4 py-3 text-sm text-center text-slate-600 font-medium">{{ tk.soThungLe }}</td>
                      <td class="px-4 py-3 text-sm text-center">
                        <input type="number" v-model.number="tk.xuatChan" min="0" :max="tk.soLuongPalletChan" class="w-full text-center border-slate-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm py-1.5" />
                      </td>
                      <td class="px-4 py-3 text-sm text-center">
                        <input type="number" v-model.number="tk.xuatLe" min="0" :max="tk.soThungLe" class="w-full text-center border-slate-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm py-1.5" />
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              
              <div v-if="tonKhoList.length > 0" class="p-4 bg-slate-50 border-t border-slate-100 flex justify-end">
                 <button @click="addDraftTicket" class="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-sm transition-colors flex items-center gap-2">
                   <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                   Thêm vào phiếu Load
                 </button>
              </div>
            </div>
          </div>

          <!-- Tab 2: Chi tiết phiếu xuất -->
          <div v-show="xuatKhoActiveTab === 'danhSach'" class="flex flex-col gap-6 flex-1">
            <!-- Bảng Phiếu Load Nháp -->
            <div class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden flex-1 flex flex-col z-0">
               <div class="p-4 border-b border-slate-100 bg-slate-50">
                 <h4 class="font-bold text-slate-700">Các mặt hàng đã thêm vào phiếu Load</h4>
               </div>
               <div class="overflow-x-auto flex-1 p-0">
                  <table class="min-w-full divide-y divide-slate-200">
                    <thead class="bg-slate-50 sticky top-0">
                      <tr>
                        <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Mã SP</th>
                        <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Tên Sản Phẩm</th>
                        <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Vị trí</th>
                        <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Chẵn</th>
                        <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Lẻ</th>
                        <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase w-16">Thao tác</th>
                      </tr>
                    </thead>
                    <tbody class="bg-white divide-y divide-slate-100">
                      <tr v-if="!draftLoadTickets[selectedXuatKhoDoc] || draftLoadTickets[selectedXuatKhoDoc].length === 0">
                         <td colspan="6" class="px-4 py-6 text-center text-slate-500 italic">Chưa có mặt hàng nào được chọn</td>
                      </tr>
                      <tr v-for="item in draftLoadTickets[selectedXuatKhoDoc]" :key="item.id" class="hover:bg-slate-50">
                         <td class="px-4 py-2 text-sm font-bold text-blue-700">{{ item.maHang }}</td>
                         <td class="px-4 py-2 text-sm text-slate-700 max-w-[200px] truncate">{{ item.tenSanPham }}</td>
                         <td class="px-4 py-2 text-sm font-medium text-slate-800">{{ item.viTri }}</td>
                         <td class="px-4 py-2 text-sm text-center text-slate-700 font-bold">{{ item.xuatChan }}</td>
                         <td class="px-4 py-2 text-sm text-center text-slate-700 font-bold">{{ item.xuatLe }}</td>
                         <td class="px-4 py-2 text-sm text-center">
                           <button @click="removeDraftItem(selectedXuatKhoDoc, item.id)" class="text-red-500 hover:text-red-700 p-1" title="Xóa">
                             <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                           </button>
                         </td>
                      </tr>
                    </tbody>
                  </table>
               </div>
            </div>
          </div>
        </div>
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-white">
          <button @click="closeXuatKhoSTOModal" class="px-4 py-2 bg-slate-100 border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-200 font-medium">Đóng</button>
        </div>
      </div>
    </div>

    <!-- Nhập Kho Modal -->
    <div v-if="showNhapKhoModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-lg shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Chi tiết xe nhập: {{ selectedNhapKhoXe?.bienSo }}</h3>
          <button @click="showNhapKhoModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="p-6 overflow-y-auto">

          
          <div>
            <h4 class="font-semibold text-slate-700 mb-3 border-b pb-2">Số STO <span class="text-xs text-slate-400 font-normal">(Nhấn để chọn nhập kho)</span></h4>
            <div v-if="selectedNhapKhoXe?.stOs?.length || selectedNhapKhoXe?.stos?.length" class="space-y-4">
              <div v-for="sto in (selectedNhapKhoXe.stOs || selectedNhapKhoXe.stos)" :key="sto" class="flex flex-col gap-2">
                <div @click="openNhapKhoSTOModal(sto)" class="p-3 bg-emerald-50 text-emerald-800 rounded-lg font-medium border border-emerald-100 cursor-pointer hover:bg-emerald-100 transition-colors flex justify-between items-center group shadow-sm">
                  <span>{{ sto }}</span>
                  <div class="flex items-center gap-2">
                    <span class="text-xs bg-white px-2 py-1 rounded text-emerald-600 border border-emerald-200 shadow-sm" v-if="draftNhapKhoTickets[sto]?.length">{{ draftNhapKhoTickets[sto].length }} mục đã chọn</span>
                    <svg class="w-5 h-5 text-emerald-400 group-hover:text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                  </div>
                </div>
                <!-- Draft items list -->
                <div v-if="draftNhapKhoTickets[sto]?.length" class="pl-4 border-l-2 border-emerald-200 space-y-1">
                  <div v-for="item in draftNhapKhoTickets[sto]" :key="item.id" class="bg-slate-50 text-xs px-3 py-2 rounded border border-slate-200 text-slate-700">
                    <span class="font-bold text-emerald-700">{{ item.maHang }}</span> - {{ item.tenSanPham }}
                    <span class="text-slate-500 ml-2">(Vị trí: <span class="font-bold text-slate-700">{{ item.viTri }}</span>)</span>
                    <span class="ml-2 font-medium text-emerald-600">SL: {{ item.nhapChan }} chẵn / {{ item.nhapLe }} lẻ</span>
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="text-slate-500 italic text-sm">Không có STO</div>
          </div>
        </div>
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-slate-50 gap-3">
          <button @click="showNhapKhoModal = false" class="px-4 py-2 bg-white border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-50 font-medium">Đóng</button>
          <button @click="rejectNhapKho" class="px-4 py-2 bg-red-600 text-white border border-transparent rounded-lg hover:bg-red-700 font-bold shadow-sm flex items-center gap-2">
            Hủy yêu cầu
          </button>
          <button @click="submitFinalNhapKho" class="px-4 py-2 bg-emerald-600 text-white border border-transparent rounded-lg hover:bg-emerald-700 font-bold shadow-sm flex items-center gap-2">
             <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
             Hoàn tất Nhập hàng
          </button>
        </div>
      </div>
    </div>

    <!-- Modal 2: Chọn sản phẩm và vị trí nhập kho -->
    <div v-if="showNhapKhoSTOModal" class="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-4xl shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Nhập kho {{ selectedNhapKhoType }}: {{ selectedNhapKhoDoc }}</h3>
          <button @click="showNhapKhoSTOModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        <div class="px-6 pt-4 border-b border-slate-200 bg-white">
          <div class="flex space-x-6">
            <button @click="nhapKhoActiveTab = 'nhapKho'" :class="nhapKhoActiveTab === 'nhapKho' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2">Nhập kho</button>
            <button @click="nhapKhoActiveTab = 'danhSach'" :class="nhapKhoActiveTab === 'danhSach' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 font-medium hover:text-slate-700'" class="pb-3 px-2 flex items-center gap-2">
              Danh sách hàng nhập
              <span v-if="draftNhapKhoTickets[selectedNhapKhoDoc]?.length" class="bg-blue-100 text-blue-700 text-xs py-0.5 px-2 rounded-full font-bold">{{ draftNhapKhoTickets[selectedNhapKhoDoc].length }}</span>
            </button>
          </div>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1 flex flex-col bg-slate-50/50">
          <!-- Tab 1: Nhập kho -->
          <div v-show="nhapKhoActiveTab === 'nhapKho'" class="flex flex-col gap-6">
            <!-- Chọn sản phẩm -->
            <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm flex flex-col md:flex-row gap-4 items-start relative z-10">
              <div class="w-full md:w-1/3 relative">
              <label class="block text-sm font-semibold text-slate-700 mb-2">Mã Sản Phẩm</label>
              <input :value="nhapKhoSearchQuery" 
                     @input="onNhapKhoProductInput"
                     @focus="showNhapKhoProductDropdown = true" 
                     @blur="handleNhapKhoProductBlur" 
                     @keydown.down.prevent="onNhapKhoArrowDown"
                     @keydown.up.prevent="onNhapKhoArrowUp"
                     @keydown.enter="onNhapKhoEnter"
                     type="text" placeholder="Nhập mã..." class="w-full border border-slate-300 rounded-lg px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 bg-slate-50 outline-none font-bold" autocomplete="off" />
              
              <!-- Dropdown -->
              <div v-if="showNhapKhoProductDropdown && filteredNhapKhoProducts.length > 0" ref="nhapKhoDropdownList" class="absolute z-50 w-full left-0 mt-1 bg-white border border-slate-200 rounded-lg shadow-lg max-h-60 overflow-y-auto custom-scrollbar min-w-[300px]">
                <ul class="py-1 text-sm text-slate-700">
                  <li v-for="(sp, index) in filteredNhapKhoProducts" :key="sp.id" @mousedown.prevent="selectNhapKhoProduct(sp)" :class="['px-4 py-2.5 cursor-pointer flex justify-between items-center transition-colors', selectedNhapKhoProductIndex === index ? 'bg-blue-100' : 'hover:bg-blue-50']">
                    <span class="font-bold text-blue-700 shrink-0">{{ sp.maSanPham }}</span>
                    <span class="text-xs text-slate-500 truncate ml-2 text-right">{{ sp.tenSanPham }}</span>
                  </li>
                </ul>
              </div>
            </div>
            
            <div class="w-full md:w-2/3 md:pt-7">
               <div class="flex items-center w-full min-h-[42px] border border-slate-200 rounded-lg px-4 bg-slate-50 text-slate-800 font-bold overflow-hidden">
                 <span class="truncate">{{ productsList.find(p => p.maSanPham === selectedProductIdForNhapKho)?.tenSanPham || 'Chưa chọn sản phẩm...' }}</span>
               </div>
            </div>
          </div>

          <!-- Chọn vị trí Nhập Kho -->
          <div v-if="selectedProductIdForNhapKho" class="bg-white rounded-xl border border-slate-200 shadow-sm flex flex-col z-0">
            <div class="p-4 border-b border-slate-100 bg-slate-50 flex justify-between items-center">
              <h4 class="font-semibold text-slate-700">Chọn vị trí và nhập số lượng</h4>
            </div>
            
            <div class="p-6 flex flex-col gap-4">
              <div class="flex flex-col md:flex-row gap-4 items-end relative z-20">
                <div class="w-full md:w-1/2 relative">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Vị trí</label>
                  <input :value="nhapKhoViTriSearchQuery"
                         @input="onNhapKhoViTriInput"
                         @focus="onNhapKhoViTriFocus"
                         @blur="handleNhapKhoViTriBlur"
                         @keydown.down.prevent="onNhapKhoViTriArrowDown"
                         @keydown.up.prevent="onNhapKhoViTriArrowUp"
                         @keydown.enter="onNhapKhoViTriEnter"
                         type="text" placeholder="Nhập mã vị trí..." class="w-full border border-slate-300 rounded-lg px-3 py-2 bg-white text-slate-800 focus:border-blue-500 focus:ring-blue-500 outline-none font-bold shadow-sm" autocomplete="off" />
                  
                  <!-- Dropdown Vị trí -->
                  <div v-if="showNhapKhoViTriDropdown && filteredNhapKhoViTri.length > 0" ref="nhapKhoViTriDropdownList" class="absolute z-50 w-full left-0 mt-1 bg-white border border-slate-200 rounded-lg shadow-lg max-h-60 overflow-y-auto custom-scrollbar">
                    <ul class="py-1 text-sm text-slate-700">
                      <li v-for="(vt, index) in filteredNhapKhoViTri" :key="vt.maLocal" @mousedown.prevent="selectNhapKhoViTri(vt.maLocal)" :class="['px-4 py-2.5 cursor-pointer flex justify-between items-center transition-colors', selectedNhapKhoViTriIndex === index ? 'bg-blue-100' : 'hover:bg-blue-50']">
                        <span class="font-bold text-blue-700">{{ vt.maLocal }}</span>
                        <span class="text-xs text-slate-500 truncate ml-2">{{ vt.ghiChu || (vt.hang ? `H:${vt.hang} - C:${vt.cot}` : '') }}</span>
                      </li>
                    </ul>
                  </div>
                </div>
                <div class="w-full md:w-1/2">
                  <button @click="openLayoutModalFor('nhapkho')" class="w-full px-4 py-2 bg-blue-100 text-blue-700 hover:bg-blue-200 font-bold rounded-lg shadow-sm transition-colors flex items-center justify-center gap-2 h-[42px]">
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 20l-5.447-2.724A1 1 0 013 16.382V5.618a1 1 0 011.447-.894L9 7m0 13l6-3m-6 3V7m6 10l4.553 2.276A1 1 0 0021 18.382V7.618a1 1 0 00-.553-.894L15 4m0 13V4m0 0L9 7"></path></svg>
                    Mở sơ đồ kho để chọn vị trí
                  </button>
                </div>
              </div>
              <div v-if="selectedProductForNhapKho?.loaiSanPham === 'Thành phẩm'" class="flex flex-col md:flex-row gap-4 items-end mt-2 relative z-10">
                <div class="w-full md:w-1/2">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Ngày sản xuất <span class="text-red-500">*</span></label>
                  <input type="date" v-model="nhapKhoNgaySanXuat" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-slate-800 font-bold focus:border-blue-500 focus:ring-blue-500 outline-none shadow-sm" />
                </div>
                <div class="w-full md:w-1/2">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Hạn sử dụng <span class="text-red-500">*</span></label>
                  <input type="date" v-model="nhapKhoHanSuDung" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-slate-800 font-bold focus:border-blue-500 focus:ring-blue-500 outline-none shadow-sm" />
                </div>
              </div>
              <div class="flex flex-col md:flex-row gap-4 items-end mt-2 relative z-10">
                <div class="w-full md:w-1/3">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Nhập chẵn (Pallet)</label>
                  <input ref="nhapKhoQuantityChanInput" type="number" min="0" v-model.number="nhapKhoQuantityChan" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-slate-800 font-bold focus:border-blue-500 focus:ring-blue-500 outline-none shadow-sm" />
                </div>
                <div class="w-full md:w-1/3">
                  <label class="block text-sm font-semibold text-slate-700 mb-2">Nhập lẻ (Thùng)</label>
                  <input type="number" min="0" v-model.number="nhapKhoQuantityLe" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-slate-800 font-bold focus:border-blue-500 focus:ring-blue-500 outline-none shadow-sm" />
                </div>
                <div class="w-full md:w-1/3">
                  <button @click="addNhapKhoDraftTicketFromMain" class="w-full px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-sm transition-colors flex items-center justify-center gap-2 h-[42px]">
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                    Thêm vào danh sách
                  </button>
                </div>
              </div>
            </div>
          </div>
          </div>
          
          <!-- Tab 2: Danh sách hàng nhập -->
          <div v-show="nhapKhoActiveTab === 'danhSach'" class="flex flex-col h-full gap-4">
            <!-- Bảng Phiếu cẩu hàng Nháp -->
            <div class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden flex-1 flex flex-col z-0">
             <div class="p-4 border-b border-slate-100 bg-slate-50">
               <h4 class="font-bold text-slate-700">Các mặt hàng đã thêm vào phiếu cẩu hàng</h4>
             </div>
             <div class="overflow-x-auto flex-1 p-0">
                <table class="min-w-full divide-y divide-slate-200">
                  <thead class="bg-slate-50 sticky top-0">
                    <tr>
                      <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Mã SP</th>
                      <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Tên Sản Phẩm</th>
                      <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">Vị trí</th>
                      <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">NSX</th>
                      <th class="px-4 py-2 text-left text-xs font-semibold text-slate-500 uppercase">HSD</th>
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Chẵn</th>
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Lẻ</th>
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase w-16">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody class="bg-white divide-y divide-slate-100">
                    <tr v-if="!draftNhapKhoTickets[selectedNhapKhoDoc] || draftNhapKhoTickets[selectedNhapKhoDoc].length === 0">
                       <td colspan="8" class="px-4 py-6 text-center text-slate-500 italic">Chưa có mặt hàng nào được chọn</td>
                    </tr>
                    <tr v-for="item in draftNhapKhoTickets[selectedNhapKhoDoc]" :key="item.id" class="hover:bg-slate-50">
                       <td class="px-4 py-2 text-sm font-bold text-blue-700">{{ item.maHang }}</td>
                       <td class="px-4 py-2 text-sm text-slate-700 max-w-[200px] truncate">{{ item.tenSanPham }}</td>
                       <td class="px-4 py-2 text-sm font-medium text-slate-800">{{ item.viTri }}</td>
                       <td class="px-4 py-2 text-sm text-slate-600">{{ item.ngaySanXuat ? new Date(item.ngaySanXuat).toLocaleDateString('vi-VN') : '' }}</td>
                       <td class="px-4 py-2 text-sm text-slate-600">{{ item.hanSuDung ? new Date(item.hanSuDung).toLocaleDateString('vi-VN') : '' }}</td>
                       <td class="px-4 py-2 text-sm text-center text-slate-700 font-bold">{{ item.nhapChan }}</td>
                       <td class="px-4 py-2 text-sm text-center text-slate-700 font-bold">{{ item.nhapLe }}</td>
                       <td class="px-4 py-2 text-sm text-center">
                         <button @click="removeNhapKhoDraftItem(selectedNhapKhoDoc, item.id)" class="text-red-500 hover:text-red-700 p-1" title="Xóa">
                           <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                         </button>
                       </td>
                    </tr>
                  </tbody>
                </table>
             </div>
          </div>
        </div>
        </div>
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-white">
          <button @click="showNhapKhoSTOModal = false" class="px-4 py-2 bg-slate-100 border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-200 font-medium">Đóng</button>
        </div>
      </div>
    </div>

    <!-- Modal Layout dành cho Nhập Kho -->
    <div v-if="showNhapKhoLayoutModal" class="fixed inset-0 z-[70] flex items-center justify-center bg-black bg-opacity-70">
      <div class="bg-white rounded-xl w-[95vw] h-[95vh] shadow-2xl flex flex-col overflow-hidden relative">
        <div class="px-6 py-4 border-b border-slate-200 flex justify-between items-center bg-slate-50">
          <h3 class="text-xl font-bold text-slate-800">Chọn vị trí từ Sơ đồ kho</h3>
          <button @click="showNhapKhoLayoutModal = false" class="text-slate-400 hover:text-slate-600 bg-slate-200 hover:bg-slate-300 rounded-full p-1.5 transition-colors">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="flex-1 bg-slate-200 relative overflow-hidden flex flex-col">
          <!-- Layout container -->
          <div 
            ref="layoutContainer"
            class="flex-1 w-full h-full relative overflow-auto"
            :class="isDraggingLayout ? 'cursor-grabbing' : 'cursor-grab'"
            @mousedown="onLayoutMouseDown"
            @mousemove="onLayoutMouseMove"
            @mouseup="onLayoutMouseUp"
            @mouseleave="onLayoutMouseLeave"
          >
            <div class="relative w-[3000px] h-[3000px] bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAiIGhlaWdodD0iMjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGNpcmNsZSBjeD0iMiIgY3k9IjIiIHI9IjEiIGZpbGw9IiNjYmQ1ZTEiLz48L3N2Zz4=')]">
              <div 
                v-for="item in layoutItems" 
                :key="item.id"
                @dblclick.prevent="onNhapKhoLayoutElementDblClick(item)"
                :class="[
                  'absolute flex items-center justify-center select-none transition-shadow group',
                  (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? 'bg-slate-800 shadow-sm border-0' : 
                  item.elementType === 'text' ? `bg-transparent text-slate-800 shadow-none border-0 ring-0 outline-none leading-none ${item.hang === 'bold' ? 'font-bold' : 'font-normal'}` : 
                  selectedLayoutViTri === item.maLocal ? 'bg-blue-100 border-4 border-blue-600 shadow-xl z-10' : 'bg-white border-2 border-slate-400 hover:border-blue-400 cursor-pointer shadow-md rounded hover:shadow-lg'
                ]"
                :style="{ 
                  left: item.positionX + 'px', 
                  top: item.positionY + 'px',
                  width: item.width + 'px',
                  height: item.height + 'px',
                  borderRadius: (item.borderRadius || 0) + 'px',
                  transform: `rotate(${item.rotation || 0}deg)`,
                  transformOrigin: (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? '0% 50%' : '50% 50%',
                  fontSize: item.elementType === 'text' ? (item.height * 0.6) + 'px' : undefined,
                  zIndex: selectedLayoutViTri === item.maLocal ? 10 : 1
                }"
              >
                <!-- Content -->
                <div class="text-center w-full px-2 overflow-hidden" v-if="(!item.elementType && !item.maLocal.startsWith('LINE_')) || item.elementType === 'local'">
                  <div class="font-bold truncate" :class="selectedLayoutViTri === item.maLocal ? 'text-blue-800' : 'text-slate-800'" :title="item.maLocal">{{ item.maLocal }}</div>
                  <div class="text-[10px] truncate" :class="selectedLayoutViTri === item.maLocal ? 'text-blue-600' : 'text-slate-500'" v-if="item.hang || item.cot">H:{{item.hang}} - C:{{item.cot}}</div>
                </div>
                <div class="w-full h-full flex items-center justify-center overflow-hidden" v-else-if="item.elementType === 'text'">
                  {{ item.maLocal }}
                </div>
              </div>
            </div>
          </div>
          
          <!-- Panel Nhập liệu dưới cùng -->
          <div class="absolute bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.1)] p-4 z-20 flex flex-col md:flex-row items-center justify-between gap-4">
             <div class="flex items-center gap-4">
               <div class="text-slate-700">
                 <span class="block text-xs font-semibold text-slate-500 uppercase">Vị trí đang chọn</span>
                 <span class="text-2xl font-black text-blue-700">{{ selectedLayoutViTri || 'Chưa chọn' }}</span>
               </div>
             </div>
             
             <div>
               <button @click="confirmNhapKhoLayoutSelection" class="px-8 py-3 bg-blue-600 text-white rounded-xl hover:bg-blue-700 font-bold shadow-lg transition-transform hover:scale-105 flex items-center gap-2 text-lg">
                 <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
                 Chọn vị trí này
               </button>
             </div>
          </div>
        </div>
      </div>
    </div>

    
    <GuardAuditModal 
      :show="showAuditModal" 
      :xe="selectedAuditXe" 
      @close="closeAuditModal"
      @success="handleAuditSuccess"
    />
    <!-- Modal 3: View Load Ticket (Phiếu cẩu hàng) -->
    <div v-if="showViewLoadTicketModal" class="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
      <div class="bg-white rounded-2xl shadow-xl w-full max-w-4xl max-h-[90vh] flex flex-col overflow-hidden">
        <!-- Content to Print -->
        <div id="print-load-ticket" class="overflow-y-auto flex-1 flex flex-col">
          <!-- Header -->
          <div class="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
            <div>
              <h2 class="text-2xl font-bold text-slate-800">{{ (viewLoadTicketData.xe?.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'Phiếu cất hàng (Nhập kho)' : 'Phiếu cẩu hàng (Xuất kho)' }}</h2>
              <p class="text-slate-500 mt-1">Biển số xe: <span class="font-bold text-blue-600">{{ viewLoadTicketData.xe?.bienSo || 'N/A' }}</span> - Tài xế: {{ viewLoadTicketData.xe?.tenLaiXe || 'N/A' }}</p>
              <p class="text-slate-500 mt-0.5 text-sm">
                Thủ kho: <span class="font-semibold text-slate-700">{{ viewLoadTicketData.items?.[0]?.tenNhanVien || 'N/A' }}</span>
                | Thời gian: <span class="font-semibold text-slate-700">{{ viewLoadTicketData.items?.[0]?.thoiGian ? formatDate(viewLoadTicketData.items[0].thoiGian, true) : 'N/A' }}</span>
              </p>
            </div>
            <button @click="showViewLoadTicketModal = false" class="text-slate-400 hover:text-slate-600 p-2 rounded-full hover:bg-slate-100 transition-colors print:hidden">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
            </button>
          </div>

          <!-- Body -->
          <div class="p-6 bg-slate-50/50 space-y-6">
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
            <h3 class="font-bold text-slate-800 p-4 border-b border-slate-100 bg-slate-50">{{ (viewLoadTicketData.xe?.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'Chi tiết hàng cất vào các vị trí' : 'Chi tiết hàng lấy tại các vị trí' }}</h3>
            <div class="overflow-x-auto">
              <table class="w-full text-left border-collapse min-w-max">
                <thead>
                  <tr class="bg-slate-50/50 text-slate-500 text-xs uppercase tracking-wider">
                    <th class="px-4 py-3 font-semibold border-b border-slate-100">Mã hàng</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100">Tên hàng</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100 text-center">Vị trí</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100 text-right">Lượng chẵn (Pallet)</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100 text-right">Lượng lẻ (Thùng)</th>
                  </tr>
                </thead>
                <tbody class="text-sm divide-y divide-slate-50">
                  <tr v-if="!computedViewLoadTicketItemsGrouped || computedViewLoadTicketItemsGrouped.length === 0">
                    <td colspan="5" class="px-4 py-6 text-center text-slate-500">{{ (viewLoadTicketData.xe?.lyDoVaoKho || '').toLowerCase().includes('nhập') ? 'Không có dữ liệu nhập kho.' : 'Không có dữ liệu xuất kho.' }}</td>
                  </tr>
                  <tr v-for="item in computedViewLoadTicketItemsGrouped" :key="item.id" class="hover:bg-slate-50/80 transition-colors">
                    <td class="px-4 py-3 font-medium text-slate-800">{{ item.maSanPham }}</td>
                    <td class="px-4 py-3 text-slate-600">{{ item.tenSanPham }}</td>
                    <td class="px-4 py-3 text-center">
                      <span class="px-2 py-1 bg-amber-100 text-amber-800 rounded-md font-bold text-xs">{{ item.viTri }}</span>
                    </td>
                    <td class="px-4 py-3 text-right font-semibold text-blue-600">{{ item.soLuongChan || 0 }}</td>
                    <td class="px-4 py-3 text-right font-semibold text-green-600">{{ item.soLuongLe || 0 }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- Summary -->
          <div v-if="viewLoadTicketData.items && viewLoadTicketData.items.length > 0" class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
            <h3 class="font-bold text-slate-800 p-4 border-b border-slate-100 bg-slate-50">Tổng hợp theo mặt hàng</h3>
            <div class="overflow-x-auto">
              <table class="w-full text-left border-collapse min-w-max">
                <thead>
                  <tr class="bg-slate-50/50 text-slate-500 text-xs uppercase tracking-wider">
                    <th class="px-4 py-3 font-semibold border-b border-slate-100">Mã hàng</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100">Tên hàng</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100 text-right">Tổng chẵn (Pallet)</th>
                    <th class="px-4 py-3 font-semibold border-b border-slate-100 text-right">Tổng lẻ (Thùng)</th>
                  </tr>
                </thead>
                <tbody class="text-sm divide-y divide-slate-50">
                  <tr v-for="(summary, ma) in computedViewLoadTicketSummary" :key="ma" class="hover:bg-slate-50/80 transition-colors">
                    <td class="px-4 py-3 font-medium text-slate-800">{{ ma }}</td>
                    <td class="px-4 py-3 text-slate-600">{{ summary.tenSanPham }}</td>
                    <td class="px-4 py-3 text-right font-bold text-blue-600">{{ summary.tongChan }}</td>
                    <td class="px-4 py-3 text-right font-bold text-green-600">{{ summary.tongLe }}</td>
                  </tr>
                  <!-- Grand Total -->
                  <tr class="bg-slate-100">
                    <td colspan="2" class="px-4 py-3 font-bold text-slate-800 text-right">TỔNG CỘNG:</td>
                    <td class="px-4 py-3 text-right font-black text-blue-700 text-base">
                      {{ Object.values(computedViewLoadTicketSummary).reduce((sum, item) => sum + item.tongChan, 0) }}
                    </td>
                    <td class="px-4 py-3 text-right font-black text-green-700 text-base">
                      {{ Object.values(computedViewLoadTicketSummary).reduce((sum, item) => sum + item.tongLe, 0) }}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        </div>

        <!-- Footer -->
        <div class="p-4 border-t border-slate-100 bg-slate-50 flex justify-end gap-3">
          <button @click="printTicket" class="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 font-medium transition-colors shadow-sm flex items-center gap-2">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 17h2a2 2 0 002-2v-4a2 2 0 00-2-2H5a2 2 0 00-2 2v4a2 2 0 002 2h2m2 4h6a2 2 0 002-2v-4a2 2 0 00-2-2H9a2 2 0 00-2 2v4a2 2 0 002 2zm8-12V5a2 2 0 00-2-2H9a2 2 0 00-2 2v4h10z"></path></svg>
            In Phiếu
          </button>
          <button @click="showViewLoadTicketModal = false" class="px-6 py-2 bg-white border border-slate-300 text-slate-700 rounded-lg hover:bg-slate-50 font-medium transition-colors shadow-sm">
            Đóng
          </button>
        </div>
      </div>
    </div>
    <!-- Modal Đếm kho (Bảng đếm) -->
    <div v-if="showDemKhoModal" class="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-4">
      <div class="bg-white rounded-2xl w-[95%] max-w-7xl max-h-[90vh] flex flex-col shadow-2xl overflow-hidden">
        <div class="flex justify-between items-center p-6 border-b border-slate-200 bg-slate-50">
          <div>
            <h3 class="text-2xl font-bold text-slate-800">Bảng đếm kho</h3>
            <p class="text-blue-600 font-medium text-lg mt-1">Vị trí: {{ demKhoData?.viTri }}</p>
          </div>
          <div class="flex items-center gap-4">
            <button @click="showAddProductModal = true" class="px-4 py-2 bg-indigo-50 text-indigo-700 hover:bg-indigo-100 font-semibold rounded-lg text-sm border border-indigo-200 transition-colors flex items-center gap-2">
              <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
              Thêm sản phẩm
            </button>
            <button @click="showDemKhoModal = false" class="text-slate-400 hover:text-slate-600 transition-colors p-2 bg-slate-200 rounded-full hover:bg-slate-300">
              <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
            </button>
          </div>
        </div>
        
        <div class="flex-1 overflow-auto p-6 bg-slate-50/50">
          <table class="w-full text-sm border-collapse bg-white shadow-sm rounded-lg overflow-hidden border border-slate-200">
            <thead>
              <tr class="bg-slate-100 border-b border-slate-200">
                <th rowspan="2" class="px-3 py-2 text-left font-bold text-slate-700 border-r border-slate-200 bg-slate-100">Mã hàng</th>
                <th colspan="2" class="px-3 py-2 text-center font-bold text-blue-700 border-r border-slate-200 bg-blue-50">SỐ HIỆN TẠI</th>
                <th colspan="2" class="px-3 py-2 text-center font-bold text-amber-700 border-r border-slate-200 bg-amber-50">SỐ ĐẾM</th>
                <th colspan="2" class="px-3 py-2 text-center font-bold text-emerald-700 border-r border-slate-200 bg-emerald-50">CHÊNH LỆCH</th>
                <th rowspan="2" class="px-3 py-2 text-left font-bold text-slate-700 border-r border-slate-200 bg-slate-100">Tên sản phẩm</th>
                <th rowspan="2" class="px-3 py-2 text-center font-bold text-slate-700 border-r border-slate-200 bg-slate-100">Ngày SX</th>
                <th rowspan="2" class="px-3 py-2 text-center font-bold text-slate-700 border-r border-slate-200 bg-slate-100">Hạn SD</th>
                <th rowspan="2" class="px-3 py-2 text-left font-bold text-slate-700 bg-slate-100 w-48">Ghi chú</th>
              </tr>
              <tr class="bg-slate-50 border-b border-slate-200">
                <!-- SỐ HIỆN TẠI -->
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Chẵn</th>
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Lẻ</th>
                <!-- SỐ ĐẾM -->
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Chẵn</th>
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Lẻ</th>
                <!-- CHÊNH LỆCH -->
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Chẵn</th>
                <th class="px-3 py-2 text-center font-semibold text-slate-600 border-r border-slate-200">Lẻ</th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="!demKhoData?.chiTiets || demKhoData.chiTiets.length === 0">
                <td colspan="11" class="px-4 py-8 text-center text-slate-500 font-medium">Chưa có sản phẩm nào ghi nhận tại vị trí này.</td>
              </tr>
              <tr v-for="(ct, index) in demKhoData?.chiTiets" :key="index" class="border-b border-slate-100 hover:bg-slate-50 transition-colors">
                <td class="px-3 py-2 font-bold text-slate-800 border-r border-slate-100">{{ ct.maSanPham }}</td>
                <td class="px-3 py-2 text-center font-medium text-blue-700 border-r border-slate-100 bg-blue-50/30">{{ ct.tonChanHienTai }}</td>
                <td class="px-3 py-2 text-center font-medium text-blue-700 border-r border-slate-100 bg-blue-50/30">{{ ct.tonLeHienTai }}</td>
                
                <!-- Input số đếm -->
                <td class="px-2 py-1 text-center border-r border-slate-100 bg-amber-50/30">
                  <input type="number" v-model.number="ct.soDemChan" :disabled="demKhoData?.id > 0" min="0" class="w-full text-center border border-slate-300 rounded px-2 py-1 text-slate-800 focus:ring-2 focus:ring-amber-500 focus:outline-none bg-white font-medium disabled:bg-slate-100 disabled:text-slate-500">
                </td>
                <td class="px-2 py-1 text-center border-r border-slate-100 bg-amber-50/30">
                  <input type="number" v-model.number="ct.soDemLe" :disabled="demKhoData?.id > 0" min="0" class="w-full text-center border border-slate-300 rounded px-2 py-1 text-slate-800 focus:ring-2 focus:ring-amber-500 focus:outline-none bg-white font-medium disabled:bg-slate-100 disabled:text-slate-500">
                </td>
                
                <!-- Chênh lệch = Số đếm - Hiện tại -->
                <td class="px-3 py-2 text-center font-bold border-r border-slate-100" :class="getChenhLechColor(ct.soDemChan - ct.tonChanHienTai)">{{ ct.soDemChan - ct.tonChanHienTai }}</td>
                <td class="px-3 py-2 text-center font-bold border-r border-slate-100" :class="getChenhLechColor(ct.soDemLe - ct.tonLeHienTai)">{{ ct.soDemLe - ct.tonLeHienTai }}</td>
                
                <td class="px-3 py-2 text-slate-700 border-r border-slate-100 max-w-[200px] truncate" :title="ct.tenSanPham">{{ ct.tenSanPham }}</td>
                <td class="px-3 py-2 text-center text-slate-600 border-r border-slate-100">{{ formatDate(ct.ngaySanXuat) }}</td>
                <td class="px-3 py-2 text-center text-slate-600 border-r border-slate-100">{{ formatDate(ct.hanSuDung) }}</td>
                <td class="px-2 py-1 bg-white">
                  <input type="text" v-model="ct.ghiChu" :disabled="demKhoData?.id > 0" class="w-full border border-slate-300 rounded px-2 py-1 text-slate-800 focus:ring-2 focus:ring-blue-500 focus:outline-none text-sm placeholder:text-slate-400 disabled:bg-slate-100 disabled:text-slate-500" placeholder="Nhập ghi chú...">
                </td>
              </tr>
            </tbody>
          </table>
          <div class="mt-4 px-1">
            <label class="block text-sm font-medium text-slate-700 mb-1">Ghi chú chung cho phiếu đếm (Tuỳ chọn)</label>
            <input type="text" v-model="demKhoData.ghiChu" :disabled="demKhoData?.id > 0" class="w-full border border-slate-300 rounded-lg px-4 py-2 text-slate-800 focus:ring-2 focus:ring-blue-500 focus:outline-none disabled:bg-slate-100 disabled:text-slate-500" placeholder="Nhập ghi chú nếu cần...">
          </div>
        </div>
        
        <div class="p-6 border-t border-slate-200 flex justify-end gap-3 bg-white rounded-b-2xl">
          <button @click="showDemKhoModal = false" class="px-6 py-2.5 border border-slate-300 text-slate-700 rounded-lg hover:bg-slate-50 font-medium transition-colors">Đóng</button>
          <button @click="submitDemKho" class="px-8 py-2.5 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-bold transition-colors shadow-sm flex items-center gap-2">
            Lưu vào bảng đếm
          </button>
        </div>
      </div>
    </div>
    
    <!-- Modal Thêm sản phẩm ngoài danh mục -->
    <div v-if="showAddProductModal" class="fixed inset-0 bg-black/60 z-[60] flex items-center justify-center p-4">
      <div class="bg-white rounded-xl w-[95%] max-w-md shadow-2xl overflow-hidden">
        <div class="p-5 border-b border-slate-200 bg-slate-50 flex justify-between items-center">
          <h3 class="text-lg font-bold text-slate-800">Thêm sản phẩm đếm</h3>
          <button @click="showAddProductModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        <div class="p-5 space-y-4">
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Mã sản phẩm *</label>
            <input list="productListDatalist" v-model="addProductForm.maSanPham" class="w-full border border-slate-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none" placeholder="Nhập mã SP...">
            <datalist id="productListDatalist">
              <option v-for="sp in productsList" :key="sp.maSanPham" :value="sp.maSanPham">{{ sp.tenSanPham }}</option>
            </datalist>
          </div>
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Ngày sản xuất *</label>
            <input type="date" v-model="addProductForm.ngaySanXuat" class="w-full border border-slate-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Hạn sử dụng *</label>
            <input type="date" v-model="addProductForm.hanSuDung" class="w-full border border-slate-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Ghi chú (Tuỳ chọn)</label>
            <input type="text" v-model="addProductForm.ghiChu" class="w-full border border-slate-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none" placeholder="Nhập ghi chú...">
          </div>
        </div>
        <div class="p-5 border-t border-slate-200 flex justify-end gap-3 bg-slate-50">
          <button @click="showAddProductModal = false" class="px-4 py-2 border border-slate-300 rounded-lg font-medium hover:bg-slate-100">Hủy</button>
          <button @click="handleAddProductToDemKho" class="px-6 py-2 bg-indigo-600 text-white font-bold rounded-lg hover:bg-indigo-700 shadow-sm">Thêm vào</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import * as XLSX from 'xlsx'

definePageMeta({
  layout: false,
  title: 'Dashboard Nhân viên'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl
const router = useRouter()

const nhanvienInfo = ref(null)
const khohangInfo = ref(null)

const vehicles = ref([])
const loading = ref(true)
const layoutItems = ref([])

// Layout panning state
const layoutContainer = ref(null)
const isDraggingLayout = ref(false)
const startDragX = ref(0)
const startDragY = ref(0)
const startScrollLeft = ref(0)
const startScrollTop = ref(0)

const onLayoutMouseDown = async (e) => {
  isDraggingLayout.value = true
  startDragX.value = e.clientX
  startDragY.value = e.clientY
  startScrollLeft.value = layoutContainer.value.scrollLeft
  startScrollTop.value = layoutContainer.value.scrollTop
}

const onLayoutMouseMove = async (e) => {
  if (!isDraggingLayout.value || !layoutContainer.value) return
  e.preventDefault()
  const dx = e.clientX - startDragX.value
  const dy = e.clientY - startDragY.value
  layoutContainer.value.scrollLeft = startScrollLeft.value - dx
  layoutContainer.value.scrollTop = startScrollTop.value - dy
}

const onLayoutMouseUp = async () => {
  isDraggingLayout.value = false
}

const onLayoutMouseLeave = async () => {
  isDraggingLayout.value = false
}
const loadingLayout = ref(false)

const getStos = (xe) => {
  if (xe.sTOs && xe.sTOs.length) return xe.sTOs.join(' | ')
  if (xe.stOs && xe.stOs.length) return xe.stOs.join(' | ')
  if (xe.stos && xe.stos.length) return xe.stos.join(' | ')
  return '-'
}

const getShipments = (xe) => {
  if (xe.shipments && xe.shipments.length) return xe.shipments.join(' | ')
  return '-'
}

const nhapKhoVehicles = computed(() => {
  return vehicles.value.filter(v => 
    v.trangThai === 1 && 
    (v.lyDoVaoKho === 'Nhập hàng' || v.lyDoVaoKho === 'Nhập kho' || (v.lyDoVaoKho && v.lyDoVaoKho.toLowerCase().includes('nhập')))
  )
})

const activeNhapKhoTab = ref(0)
const nhapKhoTabs = [
  { name: 'Đang lập phiếu' },
  { name: 'Đang cẩu hàng' }
]

const filteredNhapKhoVehicles = computed(() => {
  if (activeNhapKhoTab.value === 0) {
    return nhapKhoVehicles.value.filter(v => !v.nhapKho_Time)
  }
  return nhapKhoVehicles.value.filter(v => !!v.nhapKho_Time)
})

const activeXuatKhoTab = ref(0)
const xuatKhoTabs = [
  { name: 'Đang lập phiếu' },
  { name: 'Đang cẩu hàng' }
]

const xuatKhoVehicles = computed(() => {
  return vehicles.value.filter(v => 
    v.trangThai === 1 && 
    (v.lyDoVaoKho === 'Xuất hàng' || v.lyDoVaoKho === 'Xuất kho' || (v.lyDoVaoKho && v.lyDoVaoKho.toLowerCase().includes('xuất')))
  )
})

const filteredXuatKhoVehicles = computed(() => {
  if (activeXuatKhoTab.value === 0) {
    return xuatKhoVehicles.value.filter(v => !v.xuatKho_Time)
  }
  return xuatKhoVehicles.value.filter(v => !!v.xuatKho_Time)
})

const tabs = [
  { name: 'Đã đăng ký', status: 0 },
  { name: 'Trong sân', status: 1 },
  { name: 'Chuẩn bị ra sân', status: 2 },
  { name: 'Đã hoàn thành', status: 3 }
]
const activeTab = ref(0)
const activeMenu = ref('dashboard')

// --- ĐIỀU CHUYỂN NỘI BỘ ---
const activeDieuChuyenTab = ref('taolenh')

const dcnbViTriDiInput = ref('')
const dcnbViTriDi = ref('')
const dcnbTonKhoList = ref([])
const dcnbLoadingTonKho = ref(false)
const dcnbDraftItems = ref([])
const dcnbViTriDenInput = ref('')
const isCreatingLenh = ref(false)

const pendingLenhDCNBList = ref([])
const loadingPendingLenhDCNB = ref(false)
const isCompletingLenh = ref(false)

const fetchDcnbTonKhoList = async () => {
  if (!dcnbViTriDiInput.value.trim() || !khohangInfo.value?.id) return
  dcnbLoadingTonKho.value = true
  try {
    const viTri = dcnbViTriDiInput.value.trim().toUpperCase()
    dcnbViTriDi.value = viTri
    const res = await $fetch(`${apiBase}/DieuChuyenNoiBo/Kho/${khohangInfo.value.id}/ViTri/${viTri}`)
    dcnbTonKhoList.value = res.map(item => ({
      ...item,
      xuatChan: 0,
      xuatLe: 0
    }))
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải tồn kho vị trí')
  } finally {
    dcnbLoadingTonKho.value = false
  }
}

const addDcnbToDraft = async () => {
  const itemsToAdd = dcnbTonKhoList.value.filter(tk => tk.xuatChan > 0 || tk.xuatLe > 0)
  if (itemsToAdd.length === 0) return

  itemsToAdd.forEach(item => {
    // Check if already in draft
    const existing = dcnbDraftItems.value.find(d => 
      d.maSanPham === item.maHang && 
      d.ngaySanXuat === item.ngaySanXuat && 
      d.hanSuDung === item.hanSuDung
    )

    if (existing) {
      existing.soLuongChan += item.xuatChan
      existing.soLuongLe += item.xuatLe
    } else {
      dcnbDraftItems.value.push({
        maSanPham: item.maHang,
        tenSanPham: item.tenSanPham,
        ngaySanXuat: item.ngaySanXuat ? item.ngaySanXuat : null,
        hanSuDung: item.hanSuDung ? item.hanSuDung : null,
        viTriDi: item.viTri,
        viTriDen: '', // To be filled before creating
        soLuongChan: item.xuatChan,
        soLuongLe: item.xuatLe,
        tonChan: item.soLuongPalletChan,
        tonLe: item.soThungLe
      })
    }
    
    // Reset inputs
    item.xuatChan = 0
    item.xuatLe = 0
  })
}

const removeDcnbDraftItem = async (index) => {
  dcnbDraftItems.value.splice(index, 1)
}

const createLenhDcnb = async () => {
  if (dcnbDraftItems.value.length === 0) return
  if (!dcnbViTriDenInput.value.trim()) {
    alert("Vui lòng nhập Vị trí chuyển đến")
    return
  }

  // Update viTriDen for all items
  const viTriDen = dcnbViTriDenInput.value.trim().toUpperCase()
  dcnbDraftItems.value.forEach(item => {
    item.viTriDen = viTriDen
  })

  isCreatingLenh.value = true
  try {
    const payload = {
      maLenh: "TEMP",
      khohangId: khohangInfo.value.id,
      nguoiTao: nhanvienInfo.value?.tnv || 'Thủ kho',
      chiTiets: dcnbDraftItems.value
    }
    await $fetch(`${apiBase}/DieuChuyenNoiBo/Create`, {
      method: 'POST',
      body: payload
    })
    
    alert('Tạo Lệnh điều chuyển nội bộ thành công!')
    // Reset form
    dcnbDraftItems.value = []
    dcnbViTriDiInput.value = ''
    dcnbViTriDi.value = ''
    dcnbTonKhoList.value = []
    dcnbViTriDenInput.value = ''
    
    // Switch to Pending tab
    activeDieuChuyenTab.value = 'dangdieuchuyen'
    fetchPendingLenhDcnb()
  } catch (err) {
    console.error(err)
    alert('Có lỗi khi tạo lệnh điều chuyển')
  } finally {
    isCreatingLenh.value = false
  }
}

const fetchPendingLenhDcnb = async () => {
  if (!khohangInfo.value?.id) return
  loadingPendingLenhDCNB.value = true
  try {
    const res = await $fetch(`${apiBase}/DieuChuyenNoiBo/Pending/${khohangInfo.value.id}`)
    pendingLenhDCNBList.value = res || []
  } catch (err) {
    console.error(err)
  } finally {
    loadingPendingLenhDCNB.value = false
  }
}

const completeLenhDcnb = async (lenh) => {
  if (!await confirm('Xác nhận hoàn thành lệnh điều chuyển nội bộ này? (Hệ thống sẽ cập nhật tồn kho)')) return
  
  isCompletingLenh.value = true
  try {
    await $fetch(`${apiBase}/DieuChuyenNoiBo/Complete/${lenh.id}?nguoiHoanThanh=${nhanvienInfo.value?.tnv}`, {
      method: 'POST'
    })
    alert('Đã hoàn thành lệnh điều chuyển nội bộ!')
    // Cập nhật trạng thái trực tiếp trên UI thay vì gọi fetchPendingLenhDcnb để giữ lệnh hiển thị
    lenh.trangThai = 'updated'
    // fetchPendingLenhDcnb()
  } catch (err) {
    console.error(err)
    alert(err.data || 'Có lỗi khi hoàn thành lệnh')
  } finally {
    isCompletingLenh.value = false
  }
}

const printLenhDcnb = async (lenh) => {
  const printWindow = window.open('', '_blank')
  if (!printWindow) return

  let rowsHtml = ''
  lenh.chiTiets.forEach((item, index) => {
    rowsHtml += `
      <tr>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${index + 1}</td>
        <td style="border: 1px solid #ddd; padding: 8px;">${item.maSanPham}</td>
        <td style="border: 1px solid #ddd; padding: 8px;">${item.tenSanPham}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${item.ngaySanXuat ? new Date(item.ngaySanXuat).toLocaleDateString('vi-VN') : ''}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${item.hanSuDung ? new Date(item.hanSuDung).toLocaleDateString('vi-VN') : ''}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${item.viTriDi}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${item.viTriDen}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: right;">${item.soLuongChan}</td>
        <td style="border: 1px solid #ddd; padding: 8px; text-align: right;">${item.soLuongLe}</td>
      </tr>
    `
  })

  const html = `
    <html>
      <head>
        <title>Phiếu lệnh điều chuyển nội bộ</title>
        <style>
          body { font-family: Arial, sans-serif; padding: 20px; }
          h2 { text-align: center; margin-bottom: 20px; }
          .header-info { margin-bottom: 20px; display: flex; justify-content: space-between; }
          table { width: 100%; border-collapse: collapse; margin-bottom: 30px; }
          th { border: 1px solid #ddd; padding: 8px; background-color: #f5f5f5; text-align: center; }
          .signature { display: flex; justify-content: space-around; margin-top: 50px; }
          .signature div { text-align: center; }
        </style>
      </head>
      <body>
        <h2>PHIẾU LỆNH ĐIỀU CHUYỂN NỘI BỘ</h2>
        <div class="header-info">
          <div>
            <p><strong>Mã lệnh:</strong> ${lenh.maLenh}</p>
            <p><strong>Ngày tạo:</strong> ${new Date(lenh.thoiGianTao).toLocaleString('vi-VN')}</p>
          </div>
          <div>
            <p><strong>Người tạo:</strong> ${lenh.nguoiTao}</p>
            <p><strong>Kho:</strong> ${khohangInfo.value?.tenKho}</p>
          </div>
        </div>
        
        <table>
          <thead>
            <tr>
              <th>STT</th>
              <th>Mã Hàng</th>
              <th>Tên Hàng</th>
              <th>NSX</th>
              <th>HSD</th>
              <th>Vị trí đi</th>
              <th>Vị trí đến</th>
              <th>Chẵn</th>
              <th>Lẻ</th>
            </tr>
          </thead>
          <tbody>
            ${rowsHtml}
          </tbody>
        </table>

        <div class="signature">
          <div>
            <p><strong>Người lập phiếu</strong></p>
            <p>(Ký, ghi rõ họ tên)</p>
          </div>
          <div>
            <p><strong>Thủ kho</strong></p>
            <p>(Ký, ghi rõ họ tên)</p>
          </div>
          <div>
            <p><strong>Người vận chuyển</strong></p>
            <p>(Ký, ghi rõ họ tên)</p>
          </div>
        </div>
      </body>
    </html>
  `

  printWindow.document.write(html)
  printWindow.document.close()
  setTimeout(() => {
    printWindow.print()
  }, 500)
}


const hasPermission = async (code) => {
  if (!nhanvienInfo.value?.permissions) return false
  const perms = nhanvienInfo.value.permissions.split(',').map(p => p.trim())
  return perms.includes(code)
}

const isBaoVe = computed(() => hasPermission('BaoVe'))
const isThuKho = computed(() => hasPermission('ThuKho'))

const totalVehicles = computed(() => vehicles.value.length)
const checkedVehicles = computed(() => vehicles.value.filter(v => v.baoVeKiemTra_Time).length)
const checkedPercentage = computed(() => {
  if (totalVehicles.value === 0) return 0
  return Math.round((checkedVehicles.value / totalVehicles.value) * 100)
})

const showAuditModal = ref(false)
const selectedAuditXe = ref(null)

// Xuat Kho Modals State
const showXuatKhoModal = ref(false)
const selectedXuatKhoXe = ref(null)

const showXuatKhoSTOModal = ref(false)
const selectedXuatKhoType = ref('STO')
const selectedXuatKhoDoc = ref('')
const xuatKhoActiveTab = ref('xuatKho')
const draftLoadTickets = ref({})

const productsList = ref([])
const selectedProductIdForXuatKho = ref('')
const tonKhoList = ref([])
const loadingTonKho = ref(false)

const sortTonKhoKey = ref('')
const sortTonKhoAsc = ref(true)

const handleSortTonKho = async (key) => {
  if (sortTonKhoKey.value === key) {
    sortTonKhoAsc.value = !sortTonKhoAsc.value
  } else {
    sortTonKhoKey.value = key
    sortTonKhoAsc.value = true
  }
  
  tonKhoList.value.sort((a, b) => {
    let valA = a[key]
    let valB = b[key]
    
    if (key === 'ngaySanXuat' || key === 'hanSuDung') {
      valA = valA ? new Date(valA).getTime() : 0
      valB = valB ? new Date(valB).getTime() : 0
    }
    
    if (valA < valB) return sortTonKhoAsc.value ? -1 : 1
    if (valA > valB) return sortTonKhoAsc.value ? 1 : -1
    return 0
  })
}

const showXuatKhoProductDropdown = ref(false)
const selectedXuatKhoProductIndex = ref(-1)
const xuatKhoDropdownList = ref(null)
const xuatKhoSearchQuery = ref('')
// --- VIEW LOAD TICKET ---
const showViewLoadTicketModal = ref(false)
const viewLoadTicketData = ref({
  xe: null,
  items: []
})

const openViewLoadTicketModal = async (xe) => {
  try {
    const loai = (xe.lyDoVaoKho && xe.lyDoVaoKho.toLowerCase().includes('nhập')) ? 'Nhập' : 'Xuất'
    const res = await $fetch(`${apiBase}/PhatSinhSanPhams/BySothe`, {
      params: { sothe: xe.sothe, loai: loai }
    })
    viewLoadTicketData.value.xe = xe
    viewLoadTicketData.value.items = res || []
    showViewLoadTicketModal.value = true
  } catch (err) {
    alert('Không thể tải phiếu cẩu hàng!')
  }
}

const computedViewLoadTicketItemsGrouped = computed(() => {
  const grouped = {}
  viewLoadTicketData.value.items.forEach(item => {
    const key = `${item.maSanPham}_${item.viTri}`
    if (!grouped[key]) {
      grouped[key] = {
        id: item.id || key,
        maSanPham: item.maSanPham,
        tenSanPham: item.tenSanPham,
        viTri: item.viTri,
        soLuongChan: 0,
        soLuongLe: 0
      }
    }
    grouped[key].soLuongChan += (item.soLuongChan || 0)
    grouped[key].soLuongLe += (item.soLuongLe || 0)
  })
  return Object.values(grouped).sort((a, b) => a.maSanPham.localeCompare(b.maSanPham) || a.viTri.localeCompare(b.viTri))
})

const computedViewLoadTicketSummary = computed(() => {
  const summary = {}
  viewLoadTicketData.value.items.forEach(item => {
    if (!summary[item.maSanPham]) {
      summary[item.maSanPham] = {
        tenSanPham: item.tenSanPham,
        tongChan: 0,
        tongLe: 0
      }
    }
    summary[item.maSanPham].tongChan += (item.soLuongChan || 0)
    summary[item.maSanPham].tongLe += (item.soLuongLe || 0)
  })
  return summary
})

const printTicket = async () => {
  const printContents = document.getElementById('print-load-ticket').innerHTML;
  const printWindow = window.open('', '_blank', 'width=800,height=600');
  printWindow.document.write(`
    <html>
      <head>
        <title>In Phiếu Cẩu Hàng</title>
        <style>
          body { font-family: Arial, sans-serif; padding: 20px; color: #333; }
          h2 { margin-bottom: 5px; }
          p { margin-top: 0; color: #555; }
          table { width: 100%; border-collapse: collapse; margin-bottom: 30px; margin-top: 10px; }
          th, td { border: 1px solid #ddd; padding: 10px; text-align: left; font-size: 14px; }
          th { background-color: #f8f9fa; font-weight: bold; }
          h3 { margin-bottom: 0; padding-bottom: 5px; border-bottom: 2px solid #eee; }
          .text-right { text-align: right; }
          .text-center { text-align: center; }
          .font-bold { font-weight: bold; }
          .font-black { font-weight: 900; }
          button { display: none !important; }
        </style>
      </head>
      <body>
        ${printContents}
      </body>
    </html>
  `);
  printWindow.document.close();
  setTimeout(() => {
    printWindow.print();
    printWindow.close();
  }, 250);
}

// --- XUẤT KHO ---
const openXuatKhoModal = async (xe) => {
  if (activeXuatKhoTab.value === 1) {
    openViewLoadTicketModal(xe)
    return
  }
  selectedXuatKhoXe.value = xe
  showXuatKhoModal.value = true
}

const openXuatKhoSTOModal = async (sto) => {
  await initXuatKhoDocModal(sto, 'STO')
}

const closeXuatKhoSTOModal = async () => {
  showXuatKhoSTOModal.value = false
}

const closeXuatKhoModal = async () => {
  try {
    const docs = selectedXuatKhoXe.value?.shipments || selectedXuatKhoXe.value?.stOs || selectedXuatKhoXe.value?.stos || []
    const mnv = nhanvienInfo.value?.mnv
    if (mnv) {
      for (const doc of docs) {
        await $fetch(`${apiBase}/XuatKhoTams/Clear?soChungTu=${doc}&maNhanVien=${mnv}`, {
          method: 'POST'
        })
        draftLoadTickets.value[doc] = []
      }
    }
  } catch (err) {
    console.error(err)
  }
  showXuatKhoModal.value = false
}

const nhapKhoViTriSearchQuery = ref('')
const selectedNhapKhoViTri = ref('')
const showNhapKhoViTriDropdown = ref(false)
const selectedNhapKhoViTriIndex = ref(-1)
const nhapKhoViTriDropdownList = ref(null)
const nhapKhoQuantityChanInput = ref(null)

const filteredNhapKhoViTri = computed(() => {
  if (layoutItems.value.length === 0) return []
  const locations = layoutItems.value.filter(item => {
    const isLocal = !item.elementType || item.elementType === 'local'
    const isNotLine = item.maLocal && !item.maLocal.startsWith('LINE_')
    return isLocal && isNotLine
  })
  
  if (!nhapKhoViTriSearchQuery.value) return locations.slice(0, 50)
  
  const query = nhapKhoViTriSearchQuery.value.trim().toLowerCase()
  return locations.filter(vt => {
    return (vt.maLocal && vt.maLocal.toLowerCase().includes(query)) ||
           (vt.ghiChu && vt.ghiChu.toLowerCase().includes(query))
  }).slice(0, 50)
})

const selectNhapKhoViTri = async (maLocal) => {
  selectedNhapKhoViTri.value = maLocal
  nhapKhoViTriSearchQuery.value = maLocal
  showNhapKhoViTriDropdown.value = false
  selectedNhapKhoViTriIndex.value = -1
}

const onNhapKhoViTriInput = async (e) => {
  nhapKhoViTriSearchQuery.value = e.target.value
  selectedNhapKhoViTri.value = '' 
  showNhapKhoViTriDropdown.value = true
  selectedNhapKhoViTriIndex.value = -1
}

const onNhapKhoViTriFocus = async () => {
  if (layoutItems.value.length === 0) fetchLayout()
  showNhapKhoViTriDropdown.value = true
  selectedNhapKhoViTriIndex.value = -1
}

const handleNhapKhoViTriBlur = async () => {
  setTimeout(() => {
    showNhapKhoViTriDropdown.value = false
  }, 200)
}

const onNhapKhoViTriArrowDown = async () => {
  if (!showNhapKhoViTriDropdown.value || filteredNhapKhoViTri.value.length === 0) return
  if (selectedNhapKhoViTriIndex.value < filteredNhapKhoViTri.value.length - 1) {
    selectedNhapKhoViTriIndex.value++
    scrollToViTriItem()
  }
}

const onNhapKhoViTriArrowUp = async () => {
  if (!showNhapKhoViTriDropdown.value || filteredNhapKhoViTri.value.length === 0) return
  if (selectedNhapKhoViTriIndex.value > 0) {
    selectedNhapKhoViTriIndex.value--
    scrollToViTriItem()
  }
}

const onNhapKhoViTriEnter = async () => {
  if (showNhapKhoViTriDropdown.value && selectedNhapKhoViTriIndex.value >= 0 && selectedNhapKhoViTriIndex.value < filteredNhapKhoViTri.value.length) {
    selectNhapKhoViTri(filteredNhapKhoViTri.value[selectedNhapKhoViTriIndex.value].maLocal)
    setTimeout(() => {
      nhapKhoQuantityChanInput.value?.focus()
    }, 50)
  }
}

const scrollToViTriItem = async () => {
  if (!nhapKhoViTriDropdownList.value) return
  const list = nhapKhoViTriDropdownList.value.querySelector('ul')
  if (!list) return
  const items = list.children
  if (selectedNhapKhoViTriIndex.value >= 0 && selectedNhapKhoViTriIndex.value < items.length) {
    items[selectedNhapKhoViTriIndex.value].scrollIntoView({ block: 'nearest' })
  }
}

// --- ĐẾM KHO STATE & LOGIC ---
const activeDemKhoTab = ref('sodo')
const showDemKhoModal = ref(false)
const isSubmittingDemKho = ref(false)

const currentCountSession = ref([]) // Bảng tạm lưu các vị trí đã đếm trong phiên
const demKhoData = ref({ viTri: '', chiTiets: [], ghiChu: '' })

// Manual vị trí Go
const demKhoViTriSearchQuery = ref('')
const showDemKhoViTriDropdown = ref(false)
const selectedDemKhoViTriIndex = ref(-1)
const demKhoViTriDropdownList = ref(null)

const availableDemKhoViTriList = computed(() => {
  return layoutItems.value
    .filter(i => i.elementType !== 'line' && i.elementType !== 'text' && !i.maLocal.startsWith('LINE_'))
})

const filteredDemKhoViTri = computed(() => {
  const q = demKhoViTriSearchQuery.value.trim().toLowerCase()
  if (!q) return availableDemKhoViTriList.value.slice(0, 50)
  return availableDemKhoViTriList.value.filter(v => v.maLocal.toLowerCase().includes(q)).slice(0, 50)
})

const onDemKhoViTriInput = (e) => {
  demKhoViTriSearchQuery.value = e.target.value.toUpperCase()
  showDemKhoViTriDropdown.value = true
  selectedDemKhoViTriIndex.value = -1
}

const onDemKhoViTriFocus = () => {
  showDemKhoViTriDropdown.value = true
}

const handleDemKhoViTriBlur = () => {
  setTimeout(() => {
    showDemKhoViTriDropdown.value = false
  }, 200)
}

const selectDemKhoViTri = (maLocal) => {
  demKhoViTriSearchQuery.value = maLocal
  showDemKhoViTriDropdown.value = false
  selectedDemKhoViTriIndex.value = -1
}

const onDemKhoViTriArrowDown = () => {
  if (selectedDemKhoViTriIndex.value < filteredDemKhoViTri.value.length - 1) {
    selectedDemKhoViTriIndex.value++
    scrollDemKhoViTriItem()
  }
}

const onDemKhoViTriArrowUp = () => {
  if (selectedDemKhoViTriIndex.value > 0) {
    selectedDemKhoViTriIndex.value--
    scrollDemKhoViTriItem()
  }
}

const onDemKhoViTriEnter = () => {
  if (selectedDemKhoViTriIndex.value > -1 && filteredDemKhoViTri.value[selectedDemKhoViTriIndex.value]) {
    selectDemKhoViTri(filteredDemKhoViTri.value[selectedDemKhoViTriIndex.value].maLocal)
  }
}

const scrollDemKhoViTriItem = () => {
  setTimeout(() => {
    if (demKhoViTriDropdownList.value) {
      const activeItem = demKhoViTriDropdownList.value.querySelector('.bg-blue-100')
      if (activeItem) {
        activeItem.scrollIntoView({ block: 'nearest', behavior: 'smooth' })
      }
    }
  }, 50)
}

const onDemKhoManualGo = () => {
  const viTri = demKhoViTriSearchQuery.value.trim().toUpperCase()
  if (!viTri) {
    alert('Vui lòng nhập hoặc chọn vị trí')
    return
  }
  const exist = availableDemKhoViTriList.value.find(v => v.maLocal === viTri)
  if (!exist) {
    alert('Vị trí này không tồn tại trong sơ đồ kho!')
    return
  }
  onDemKhoLayoutElementDblClick({ maLocal: viTri, elementType: 'rect' })
  demKhoViTriSearchQuery.value = ''
}

// Modal thêm sản phẩm ngoài danh mục
const showAddProductModal = ref(false)
const addProductForm = ref({
  maSanPham: '',
  ngaySanXuat: '',
  hanSuDung: '',
  ghiChu: ''
})

const getChenhLechColor = (val) => {
  if (val === 0) return 'text-slate-600'
  return val > 0 ? 'text-rose-600 font-bold' : 'text-emerald-600 font-bold'
}

const getProductName = (ma) => {
  const p = productsList.value.find(x => x.maSanPham === ma)
  return p ? p.tenSanPham : ''
}

const onDemKhoLayoutElementDblClick = async (item) => {
  if (item.elementType !== 'line' && !item.maLocal.startsWith('LINE_') && item.elementType !== 'text') {
    loadingLayout.value = true
    try {
      if (productsList.value.length === 0) {
        await fetchProducts()
      }
      const viTri = item.maLocal.toUpperCase()
      const res = await $fetch(`${apiBase}/DieuChuyenNoiBo/Kho/${khohangInfo.value.id}/ViTri/${viTri}`)
      
      // Lấy danh sách tồn kho thực tế
      let chiTiets = res.map(tk => ({
        maSanPham: tk.maHang,
        tenSanPham: tk.tenSanPham,
        ngaySanXuat: tk.ngaySanXuat,
        hanSuDung: tk.hanSuDung,
        tonChanHienTai: tk.soLuongPalletChan,
        tonLeHienTai: tk.soThungLe,
        soDemChan: tk.soLuongPalletChan,
        soDemLe: tk.soThungLe,
        ghiChu: ''
      }))

      // Gộp với dữ liệu đã lưu trong bảng tạm (nếu có)
      const existingSession = currentCountSession.value.find(s => s.viTri === viTri)
      let sessionGhiChu = ''
      if (existingSession) {
        sessionGhiChu = existingSession.ghiChu
        // Cập nhật lại số đếm từ session, thêm các sản phẩm ngoài danh mục
        existingSession.chiTiets.forEach(sessionCt => {
          const matchIndex = chiTiets.findIndex(c => c.maSanPham === sessionCt.maSanPham && c.ngaySanXuat === sessionCt.ngaySanXuat && c.hanSuDung === sessionCt.hanSuDung)
          if (matchIndex !== -1) {
            chiTiets[matchIndex].soDemChan = sessionCt.soDemChan
            chiTiets[matchIndex].soDemLe = sessionCt.soDemLe
            chiTiets[matchIndex].ghiChu = sessionCt.ghiChu
          } else {
            // Sản phẩm ngoài danh mục đã thêm trước đó
            chiTiets.push({
              ...sessionCt,
              tonChanHienTai: 0,
              tonLeHienTai: 0
            })
          }
        })
      }

      demKhoData.value = {
        khohangId: khohangInfo.value.id,
        viTri: viTri,
        nguoiDem: nhanvienInfo.value?.tnv || 'Thủ kho',
        ghiChu: sessionGhiChu,
        chiTiets: chiTiets
      }
      showDemKhoModal.value = true
    } catch (err) {
      console.error(err)
      alert('Lỗi khi tải tồn kho của vị trí này')
    } finally {
      loadingLayout.value = false
    }
  }
}

const handleAddProductToDemKho = () => {
  if (!addProductForm.value.maSanPham) {
    alert('Vui lòng chọn Mã sản phẩm')
    return
  }
  if (!addProductForm.value.ngaySanXuat || !addProductForm.value.hanSuDung) {
    alert('Vui lòng nhập Ngày sản xuất và Hạn sử dụng (Bắt buộc)')
    return
  }
  
  const tenSP = getProductName(addProductForm.value.maSanPham) || 'Sản phẩm mới'
  
  demKhoData.value.chiTiets.push({
    maSanPham: addProductForm.value.maSanPham,
    tenSanPham: tenSP,
    ngaySanXuat: addProductForm.value.ngaySanXuat,
    hanSuDung: addProductForm.value.hanSuDung,
    tonChanHienTai: 0,
    tonLeHienTai: 0,
    soDemChan: 0,
    soDemLe: 0,
    ghiChu: addProductForm.value.ghiChu
  })
  
  showAddProductModal.value = false
  addProductForm.value = { maSanPham: '', ngaySanXuat: '', hanSuDung: '', ghiChu: '' }
}

const submitDemKho = async () => {
  if (!demKhoData.value || !demKhoData.value.chiTiets || demKhoData.value.chiTiets.length === 0) return
  
  const dataToSave = JSON.parse(JSON.stringify(demKhoData.value))
  dataToSave.nguoiDem = nhanvienInfo.value?.tnv || 'Thủ kho'
  dataToSave.trangThai = 'Bảng tạm'
  dataToSave.nguoiDuyet = ''
  if (!dataToSave.ghiChu) dataToSave.ghiChu = ''
  
  dataToSave.chiTiets.forEach(ct => {
    if (!ct.ghiChu) ct.ghiChu = ''
    if (ct.ngaySanXuat === '') ct.ngaySanXuat = null
    if (ct.hanSuDung === '') ct.hanSuDung = null
  })
  
  try {
    await $fetch(`${apiBase}/DemKho/Tam/Save`, {
      method: 'POST',
      body: dataToSave
    })
    
    // Tải lại bảng tạm để cập nhật cho người dùng
    await fetchDemKhoTam()
    showDemKhoModal.value = false
    
  } catch (err) {
    console.error(err)
    alert(err.data?.title || (err.data && typeof err.data === 'object' ? JSON.stringify(err.data) : err.data) || 'Có lỗi khi lưu bảng tạm vào Server')
  }
}

const viewPhieuDemKho = (phieu) => {
  demKhoData.value = JSON.parse(JSON.stringify(phieu))
  showDemKhoModal.value = true
}

const submitCountSession = async () => {
  if (currentCountSession.value.length === 0) return
  if (!confirm('Bạn có chắc chắn muốn HOÀN THÀNH TẤT CẢ dữ liệu trong phiên đếm này và gửi duyệt?')) return
  
  isSubmittingDemKho.value = true
  try {
    await $fetch(`${apiBase}/DemKho/Tam/Complete/${khohangInfo.value.id}?nguoiHoanThanh=${encodeURIComponent(nhanvienInfo.value?.tnv || '')}`, {
      method: 'POST'
    })
    
    alert('Đã hoàn thành phiên đếm kho và gửi chờ duyệt!')
    await fetchDemKhoTam() // Clear local view
    activeDemKhoTab.value = 'sodo' // Quay về sơ đồ
  } catch (err) {
    console.error(err)
    alert(err.data?.title || (err.data && typeof err.data === 'object' ? JSON.stringify(err.data) : err.data) || 'Có lỗi khi gửi hoàn thành phiên đếm')
  } finally {
    isSubmittingDemKho.value = false
  }
}

const exportDemKhoExcel = () => {
  if (currentCountSession.value.length === 0) return
  
  const dataToExport = []
  currentCountSession.value.forEach(phieu => {
    phieu.chiTiets.forEach(ct => {
      dataToExport.push({
        'Vị trí': phieu.viTri,
        'Mã Hàng': ct.maSanPham,
        'Tên Sản Phẩm': ct.tenSanPham,
        'Ngày Sản Xuất': ct.ngaySanXuat ? formatDate(ct.ngaySanXuat) : '',
        'Hạn Sử Dụng': ct.hanSuDung ? formatDate(ct.hanSuDung) : '',
        'Tồn Chẵn Hiện Tại': ct.tonChanHienTai,
        'Tồn Lẻ Hiện Tại': ct.tonLeHienTai,
        'Số Đếm Chẵn': ct.soDemChan,
        'Số Đếm Lẻ': ct.soDemLe,
        'Chênh Lệch Chẵn': ct.chenhLechChan,
        'Chênh Lệch Lẻ': ct.chenhLechLe,
        'Ghi Chú': ct.ghiChu || '',
        'Ghi Chú Phiếu': phieu.ghiChu || ''
      })
    })
  })
  
  const ws = XLSX.utils.json_to_sheet(dataToExport)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, "KiemDem")
  XLSX.writeFile(wb, `KiemDem_${khohangInfo.value?.name || 'Kho'}_${new Date().toISOString().slice(0,10)}.xlsx`)
}

// Layout Nhập kho state
const showNhapKhoLayoutModal = ref(false)
const selectedLayoutViTri = ref('')
const layoutModalTarget = ref('nhapkho')
const nhapKhoQuantityChan = ref(0)
const nhapKhoQuantityLe = ref(0)
const nhapKhoNgaySanXuat = ref('')
const nhapKhoHanSuDung = ref('')

const openLayoutModalFor = async (target) => {
  layoutModalTarget.value = target
  if (layoutItems.value.length === 0) fetchLayout()
  showNhapKhoLayoutModal.value = true
  selectedLayoutViTri.value = ''
}

const confirmNhapKhoLayoutSelection = async () => {
  if (!selectedLayoutViTri.value) {
    alert('Vui lòng chọn vị trí')
    return
  }
  
  if (layoutModalTarget.value === 'nhapkho') {
    selectedNhapKhoViTri.value = selectedLayoutViTri.value
    nhapKhoViTriSearchQuery.value = selectedLayoutViTri.value
  } else if (layoutModalTarget.value === 'dcnb_di') {
    dcnbViTriDiInput.value = selectedLayoutViTri.value
    fetchDcnbTonKhoList()
  } else if (layoutModalTarget.value === 'dcnb_den') {
    dcnbViTriDenInput.value = selectedLayoutViTri.value
  }
  showNhapKhoLayoutModal.value = false
}

const onNhapKhoLayoutElementDblClick = async (item) => {
  if (item.elementType !== 'line' && !item.maLocal.startsWith('LINE_') && item.elementType !== 'text') {
    selectedLayoutViTri.value = item.maLocal
    confirmNhapKhoLayoutSelection()
  }
}

const addNhapKhoDraftTicketFromMain = async () => {
  if (!selectedProductIdForNhapKho.value) {
    alert('Chưa chọn sản phẩm')
    return
  }
  
  if (!selectedNhapKhoViTri.value) {
    alert('Vui lòng chọn một mã vị trí có sẵn từ danh sách thả xuống hoặc sơ đồ kho.')
    return
  }
  
  const viTri = selectedNhapKhoViTri.value

  if (!nhapKhoQuantityChan.value && !nhapKhoQuantityLe.value) {
    alert('Vui lòng nhập số lượng nhập (chẵn hoặc lẻ)')
    return
  }

  const productInfo = selectedProductForNhapKho.value
  const isThanhPham = productInfo?.loaiSanPham === 'Thành phẩm'

  if (isThanhPham && (!nhapKhoNgaySanXuat.value || !nhapKhoHanSuDung.value)) {
    alert('Vui lòng chọn Ngày sản xuất và Hạn sử dụng')
    return
  }
  
  const nsx = isThanhPham ? nhapKhoNgaySanXuat.value : null
  const hsd = isThanhPham ? nhapKhoHanSuDung.value : null
  const maHangStr = productInfo?.maSanPham || selectedProductIdForNhapKho.value
  
  if (!draftNhapKhoTickets.value[selectedNhapKhoDoc.value]) {
    draftNhapKhoTickets.value[selectedNhapKhoDoc.value] = []
  }
  
  const docTickets = draftNhapKhoTickets.value[selectedNhapKhoDoc.value]
  const existingIndex = docTickets.findIndex(d => 
    d.maHang === maHangStr && 
    d.viTri === viTri &&
    d.ngaySanXuat === nsx &&
    d.hanSuDung === hsd
  )
  
  if (existingIndex > -1) {
    docTickets[existingIndex].nhapChan += nhapKhoQuantityChan.value || 0
    docTickets[existingIndex].nhapLe += nhapKhoQuantityLe.value || 0
  } else {
    docTickets.push({
      id: Date.now().toString(),
      maHang: maHangStr,
      tenSanPham: productInfo?.tenSanPham || '',
      viTri: viTri,
      nhapChan: nhapKhoQuantityChan.value || 0,
      nhapLe: nhapKhoQuantityLe.value || 0,
      ngaySanXuat: nsx,
      hanSuDung: hsd
    })
  }
  
  nhapKhoQuantityChan.value = 0
  nhapKhoQuantityLe.value = 0
}


const initXuatKhoDocModal = async (doc, type) => {
  selectedXuatKhoType.value = type
  selectedXuatKhoDoc.value = doc
  xuatKhoActiveTab.value = 'xuatKho'
  showXuatKhoSTOModal.value = true
  fetchProducts()
  xuatKhoSearchQuery.value = ''
  selectedProductIdForXuatKho.value = ''
  tonKhoList.value = []
  
  try {
    const mnv = nhanvienInfo.value?.mnv
    if (doc && mnv) {
      const res = await $fetch(`${apiBase}/XuatKhoTams?soChungTu=${doc}&maNhanVien=${mnv}`)
      draftLoadTickets.value[doc] = (res || []).map(x => ({
        id: x.id,
        maHang: x.maSanPham,
        tenSanPham: x.tenSanPham,
        viTri: x.viTri,
        hanSuDung: x.hanSuDung,
        ngaySanXuat: x.ngaySanXuat,
        xuatChan: x.soLuongChan,
        xuatLe: x.soLuongLe,
        khohangId: x.khohangId
      }))
    }
  } catch (err) {
    console.error(err)
  }
}

const openXuatKhoShipmentModal = async (shipment) => {
  await initXuatKhoDocModal(shipment, 'Shipment')
}

const filteredXuatKhoProducts = computed(() => {
  if (!xuatKhoSearchQuery.value) return productsList.value.slice(0, 50)
  const query = String(xuatKhoSearchQuery.value).trim().toLowerCase()
  
  const queryTokens = query.split(/\s+/)
  const isNumeric = /^\d+$/.test(query.replace(/\s+/g, ''))
  
  return productsList.value.filter(s => {
    if (isNumeric) {
      const ma = String(s.maSanPham || '').toLowerCase()
      return queryTokens.every(token => ma.includes(token))
    } else {
      const ten = String(s.tenSanPham || '').toLowerCase()
      return queryTokens.every(token => ten.includes(token))
    }
  }).slice(0, 50)
})

const selectXuatKhoProduct = async (sp) => {
  xuatKhoSearchQuery.value = sp.maSanPham
  selectedProductIdForXuatKho.value = sp.maSanPham
  showXuatKhoProductDropdown.value = false
  fetchTonKhoLocations()
}

const handleXuatKhoProductBlur = async () => {
  showXuatKhoProductDropdown.value = false
}

const onXuatKhoProductInput = async (e) => {
  xuatKhoSearchQuery.value = e.target.value
  selectedProductIdForXuatKho.value = '' 
  tonKhoList.value = []
  showXuatKhoProductDropdown.value = true
  selectedXuatKhoProductIndex.value = -1
}

const onXuatKhoArrowDown = async () => {
  if (!showXuatKhoProductDropdown.value) {
    showXuatKhoProductDropdown.value = true
    return
  }
  if (selectedXuatKhoProductIndex.value < filteredXuatKhoProducts.value.length - 1) {
    selectedXuatKhoProductIndex.value++
    scrollXuatKhoItem()
  }
}

const onXuatKhoArrowUp = async () => {
  if (selectedXuatKhoProductIndex.value > 0) {
    selectedXuatKhoProductIndex.value--
    scrollXuatKhoItem()
  }
}

const onXuatKhoEnter = async (e) => {
  e.preventDefault()
  if (showXuatKhoProductDropdown.value && selectedXuatKhoProductIndex.value >= 0 && selectedXuatKhoProductIndex.value < filteredXuatKhoProducts.value.length) {
    selectXuatKhoProduct(filteredXuatKhoProducts.value[selectedXuatKhoProductIndex.value])
  } else {
    showXuatKhoProductDropdown.value = false
  }
}

const scrollXuatKhoItem = async () => {
  setTimeout(() => {
    if (xuatKhoDropdownList.value) {
      const activeItem = xuatKhoDropdownList.value.querySelector('.bg-blue-100')
      if (activeItem) {
        activeItem.scrollIntoView({ block: 'nearest', behavior: 'smooth' })
      }
    }
  }, 50)
}

const fetchProducts = async () => {
  if (productsList.value.length > 0 || !khohangInfo.value?.id) return
  try {
    const res = await $fetch(`${apiBase}/Sanphams/ByKhohang/${khohangInfo.value.id}`)
    productsList.value = res || []
  } catch (err) {
    console.error(err)
  }
}

const fetchTonKhoLocations = async () => {
  if (!selectedProductIdForXuatKho.value) {
    tonKhoList.value = []
    return
  }
  loadingTonKho.value = true
  try {
    const res = await $fetch(`${apiBase}/TonKhoHienTais/BySanPham/${selectedProductIdForXuatKho.value}`)
    tonKhoList.value = (res || []).map(t => ({
      ...t,
      xuatChan: 0,
      xuatLe: 0,
      isSubmitting: false
    }))
  } catch (err) {
    console.error(err)
    alert('Có lỗi tải tồn kho')
  } finally {
    loadingTonKho.value = false
  }
}

const addDraftTicket = async () => {
  const itemsToAdd = tonKhoList.value.filter(tk => tk.xuatChan > 0 || tk.xuatLe > 0)
  if (itemsToAdd.length === 0) {
    alert('Vui lòng nhập số lượng xuất cho ít nhất 1 vị trí!')
    return
  }
  
  const productInfo = productsList.value.find(p => p.maSanPham === selectedProductIdForXuatKho.value)
  const doc = selectedXuatKhoDoc.value
  const mnv = nhanvienInfo.value?.mnv
  
  if (!mnv) {
    alert('Không tìm thấy thông tin nhân viên!')
    return
  }

  try {
    for (const tk of itemsToAdd) {
      await $fetch(`${apiBase}/XuatKhoTams`, {
        method: 'POST',
        body: {
          maNhanVien: mnv,
          soChungTu: doc,
          maSanPham: tk.maHang,
          tenSanPham: productInfo?.tenSanPham || tk.tenSanPham,
          viTri: tk.viTri,
          soLuongChan: tk.xuatChan || 0,
          soLuongLe: tk.xuatLe || 0,
          ngaySanXuat: tk.ngaySanXuat,
          hanSuDung: tk.hanSuDung,
          khohangId: khohangInfo.value?.id
        }
      })
      tk.xuatChan = 0
      tk.xuatLe = 0
    }

    // Refresh draft ticket list from server
    const res = await $fetch(`${apiBase}/XuatKhoTams?soChungTu=${doc}&maNhanVien=${mnv}`)
    draftLoadTickets.value[doc] = (res || []).map(x => ({
      id: x.id,
      maHang: x.maSanPham,
      tenSanPham: x.tenSanPham,
      viTri: x.viTri,
      hanSuDung: x.hanSuDung,
      ngaySanXuat: x.ngaySanXuat,
      xuatChan: x.soLuongChan,
      xuatLe: x.soLuongLe,
      khohangId: x.khohangId
    }))

    // Reload available locations
    await fetchTonKhoLocations()
  } catch (err) {
    console.error(err)
    alert(err.response?._data?.message || 'Có lỗi xảy ra khi giữ chỗ tồn kho.')
  }
}

const removeDraftItem = async (doc, itemId) => {
  try {
    await $fetch(`${apiBase}/XuatKhoTams/${itemId}`, {
      method: 'DELETE'
    })
    
    // Refresh draft ticket list from server
    const mnv = nhanvienInfo.value?.mnv
    if (doc && mnv) {
      const res = await $fetch(`${apiBase}/XuatKhoTams?soChungTu=${doc}&maNhanVien=${mnv}`)
      draftLoadTickets.value[doc] = (res || []).map(x => ({
        id: x.id,
        maHang: x.maSanPham,
        tenSanPham: x.tenSanPham,
        viTri: x.viTri,
        hanSuDung: x.hanSuDung,
        ngaySanXuat: x.ngaySanXuat,
        xuatChan: x.soLuongChan,
        xuatLe: x.soLuongLe,
        khohangId: x.khohangId
      }))
    }

    // Reload available locations
    await fetchTonKhoLocations()
  } catch (err) {
    console.error(err)
    alert('Có lỗi khi xóa mặt hàng tạm tính.')
  }
}

const rejectXuatKho = async () => {
  const reason = await prompt('Vui lòng nhập lý do hủy yêu cầu xuất kho (bắt buộc):')
  if (!reason || reason.trim() === '') return
  if (!await confirm(`Xác nhận hủy yêu cầu xuất kho với lý do: "${reason}" và cho xe ra cổng luôn?`)) return
  try {
    // Clear temporary picks
    const docs = selectedXuatKhoXe.value?.shipments || selectedXuatKhoXe.value?.stOs || selectedXuatKhoXe.value?.stos || []
    for (const doc of docs) {
      await $fetch(`${apiBase}/XuatKhoTams/Clear?soChungTu=${doc}&maNhanVien=${nhanvienInfo.value?.mnv}`, {
        method: 'POST'
      })
      draftLoadTickets.value[doc] = []
    }

    await $fetch(`${apiBase}/Danhsachxetrongkho/reject-xuatkho/${selectedXuatKhoXe.value.id}`, {
      method: 'POST',
      body: { lyDoHuy: reason }
    })
    showXuatKhoModal.value = false
    fetchData()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra')
  }
}

const submitFinalXuatKho = async () => {
  const docs = Object.keys(draftLoadTickets.value)
  const payload = []
  
  docs.forEach(doc => {
    const isSTO = selectedXuatKhoXe.value?.stOs?.includes(doc) || selectedXuatKhoXe.value?.stos?.includes(doc)
    const isShipment = selectedXuatKhoXe.value?.shipments?.includes(doc)
    
    draftLoadTickets.value[doc].forEach(tk => {
      payload.push({
        maSanPham: tk.maHang,
        tenSanPham: tk.tenSanPham,
        viTri: tk.viTri,
        ngaySanXuat: tk.ngaySanXuat,
        hanSuDung: tk.hanSuDung,
        soLuongChan: tk.xuatChan || 0,
        soLuongLe: tk.xuatLe || 0,
        user: nhanvienInfo.value?.mnv || '',
        tenNhanVien: nhanvienInfo.value?.tnv || '',
        soSTO: isSTO ? doc : null,
        soShipment: isShipment ? doc : null,
        sothe: selectedXuatKhoXe.value?.sothe,
        khohangId: khohangInfo.value?.id
      })
    })
  })
  
  if (payload.length === 0) {
    alert('Phiếu load trống, vui lòng chọn ít nhất 1 sản phẩm!')
    return
  }
  
  if (!await confirm(`Xác nhận ghi nhận xuất kho cho ${payload.length} mặt hàng?`)) return
  
  try {
    loading.value = true
    for (const item of payload) {
      await $fetch(`${apiBase}/PhatSinhSanPhams/XuatKho`, {
        method: 'POST',
        body: item
      })
    }
    
    // Dọn dẹp các bản ghi tạm giữ
    for (const doc of docs) {
      await $fetch(`${apiBase}/XuatKhoTams/Clear?soChungTu=${doc}&maNhanVien=${nhanvienInfo.value?.mnv}`, {
        method: 'POST'
      })
    }

    // Đánh dấu là đã hoàn tất xuất kho cho xe
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-xuatkho/${selectedXuatKhoXe.value.id}`, {
      method: 'POST'
    })
    
    alert('Đã hoàn tất lập phiếu cẩu hàng xuất kho!')
    draftLoadTickets.value = {}
    showXuatKhoModal.value = false
    activeXuatKhoTab.value = 1
    fetchData(false)
  } catch (err) {
    console.error(err)
    alert(err.response?._data?.message || 'Có lỗi khi xuất kho')
  } finally {
    loading.value = false
  }
}


// --- NHAP KHO SCRIPT ---
const showNhapKhoModal = ref(false)
const selectedNhapKhoXe = ref(null)

const showNhapKhoSTOModal = ref(false)
const nhapKhoActiveTab = ref('nhapKho')
const selectedNhapKhoType = ref('STO')
const selectedNhapKhoDoc = ref('')
const draftNhapKhoTickets = ref({})

const nhapKhoSearchQuery = ref('')
const selectedProductIdForNhapKho = ref('')
const showNhapKhoProductDropdown = ref(false)
const selectedNhapKhoProductIndex = ref(-1)
const nhapKhoDropdownList = ref(null)

// --- XUẤT KHO ---
const openNhapKhoModal = async (xe) => {
  if (activeNhapKhoTab.value === 1) {
    openViewLoadTicketModal(xe)
    return
  }
  selectedNhapKhoXe.value = xe
  showNhapKhoModal.value = true
}

const openNhapKhoSTOModal = async (sto) => {
  selectedNhapKhoType.value = 'STO'
  selectedNhapKhoDoc.value = sto
  showNhapKhoSTOModal.value = true
  fetchProducts()
  if (layoutItems.value.length === 0) fetchLayout()
  nhapKhoSearchQuery.value = ''
  selectedProductIdForNhapKho.value = ''
  tonKhoList.value = []
  nhapKhoViTriSearchQuery.value = ''
  nhapKhoQuantityChan.value = 0
  nhapKhoQuantityLe.value = 0
}

const openNhapKhoShipmentModal = async (shipment) => {
  selectedNhapKhoType.value = 'Shipment'
  selectedNhapKhoDoc.value = shipment
  showNhapKhoSTOModal.value = true
  fetchProducts()
  if (layoutItems.value.length === 0) fetchLayout()
  nhapKhoSearchQuery.value = ''
  selectedProductIdForNhapKho.value = ''
  tonKhoList.value = []
  nhapKhoViTriSearchQuery.value = ''
  nhapKhoQuantityChan.value = 0
  nhapKhoQuantityLe.value = 0
}

const filteredNhapKhoProducts = computed(() => {
  if (!nhapKhoSearchQuery.value) return productsList.value.slice(0, 50)
  const query = String(nhapKhoSearchQuery.value).trim().toLowerCase()
  
  const queryTokens = query.split(/\s+/)
  const isNumeric = /^\d+$/.test(query.replace(/\s+/g, ''))
  
  return productsList.value.filter(s => {
    if (isNumeric) {
      const ma = String(s.maSanPham || '').toLowerCase()
      return queryTokens.every(token => ma.includes(token))
    } else {
      const ten = String(s.tenSanPham || '').toLowerCase()
      return queryTokens.every(token => ten.includes(token))
    }
  }).slice(0, 50)
})

const selectedProductForNhapKho = computed(() => {
  return productsList.value.find(p => p.maSanPham === selectedProductIdForNhapKho.value)
})

const selectNhapKhoProduct = async (sp) => {
  nhapKhoSearchQuery.value = sp.maSanPham
  selectedProductIdForNhapKho.value = sp.maSanPham
  showNhapKhoProductDropdown.value = false
  fetchTonKhoLocationsForNhapKho()
}

const handleNhapKhoProductBlur = async () => {
  showNhapKhoProductDropdown.value = false
}

const onNhapKhoProductInput = async (e) => {
  nhapKhoSearchQuery.value = e.target.value
  selectedProductIdForNhapKho.value = '' 
  tonKhoList.value = []
  showNhapKhoProductDropdown.value = true
  selectedNhapKhoProductIndex.value = -1
}

const onNhapKhoArrowDown = async () => {
  if (!showNhapKhoProductDropdown.value) {
    showNhapKhoProductDropdown.value = true
    return
  }
  if (selectedNhapKhoProductIndex.value < filteredNhapKhoProducts.value.length - 1) {
    selectedNhapKhoProductIndex.value++
    scrollNhapKhoItem()
  }
}

const onNhapKhoArrowUp = async () => {
  if (selectedNhapKhoProductIndex.value > 0) {
    selectedNhapKhoProductIndex.value--
    scrollNhapKhoItem()
  }
}

const onNhapKhoEnter = async (e) => {
  e.preventDefault()
  if (showNhapKhoProductDropdown.value && selectedNhapKhoProductIndex.value >= 0 && selectedNhapKhoProductIndex.value < filteredNhapKhoProducts.value.length) {
    selectNhapKhoProduct(filteredNhapKhoProducts.value[selectedNhapKhoProductIndex.value])
  } else {
    showNhapKhoProductDropdown.value = false
  }
}

const scrollNhapKhoItem = async () => {
  setTimeout(() => {
    if (nhapKhoDropdownList.value) {
      const activeItem = nhapKhoDropdownList.value.querySelector('.bg-blue-100')
      if (activeItem) {
        activeItem.scrollIntoView({ block: 'nearest', behavior: 'smooth' })
      }
    }
  }, 50)
}

const fetchTonKhoLocationsForNhapKho = async () => {
  if (!selectedProductIdForNhapKho.value) {
    tonKhoList.value = []
    return
  }
  loadingTonKho.value = true
  try {
    const res = await $fetch(`${apiBase}/TonKhoHienTais/BySanPham/${selectedProductIdForNhapKho.value}`)
    tonKhoList.value = (res || []).map(t => ({
      ...t,
      nhapChan: 0,
      nhapLe: 0,
      isSubmitting: false
    }))
  } catch (err) {
    console.error(err)
    alert('Có lỗi tải tồn kho')
  } finally {
    loadingTonKho.value = false
  }
}

const addNewLocationForNhapKho = async () => {
  if (!selectedProductIdForNhapKho.value) return
  tonKhoList.value.push({
    id: 'new_' + Date.now(),
    viTri: '',
    maHang: selectedProductIdForNhapKho.value,
    soLuongPalletChan: 0,
    soThungLe: 0,
    nhapChan: 0,
    nhapLe: 0,
    isNew: true
  })
}

const addNhapKhoDraftTicket = async () => {
  const itemsToAdd = tonKhoList.value.filter(tk => tk.nhapChan > 0 || tk.nhapLe > 0)
  if (itemsToAdd.length === 0) {
    alert('Vui lòng nhập số lượng nhập cho ít nhất 1 vị trí!')
    return
  }

  const missingLocation = itemsToAdd.find(tk => !tk.viTri || tk.viTri.trim() === '')
  if (missingLocation) {
    alert('Vui lòng nhập đầy đủ tên vị trí cho các hàng cần nhập!')
    return
  }
  
  const productInfo = productsList.value.find(p => p.maSanPham === selectedProductIdForNhapKho.value)
  const doc = selectedNhapKhoDoc.value
  
  if (!draftNhapKhoTickets.value[doc]) {
    draftNhapKhoTickets.value[doc] = []
  }
  
  itemsToAdd.forEach(tk => {
    const existing = draftNhapKhoTickets.value[doc].find(d => d.maHang === tk.maHang && d.viTri === tk.viTri)
    if (existing) {
      existing.nhapChan = (existing.nhapChan || 0) + (tk.nhapChan || 0)
      existing.nhapLe = (existing.nhapLe || 0) + (tk.nhapLe || 0)
    } else {
      draftNhapKhoTickets.value[doc].push({
        id: Date.now() + Math.random(),
        maHang: tk.maHang,
        tenSanPham: productInfo?.tenSanPham || tk.tenSanPham,
        viTri: tk.viTri,
        hanSuDung: tk.hanSuDung,
        ngaySanXuat: tk.ngaySanXuat,
        nhapChan: tk.nhapChan || 0,
        nhapLe: tk.nhapLe || 0
      })
    }
    tk.nhapChan = 0
    tk.nhapLe = 0
  })
}

const removeNhapKhoDraftItem = async (doc, itemId) => {
  if (draftNhapKhoTickets.value[doc]) {
    draftNhapKhoTickets.value[doc] = draftNhapKhoTickets.value[doc].filter(i => i.id !== itemId)
  }
}
const rejectNhapKho = async () => {
  const reason = await prompt('Vui lòng nhập lý do hủy yêu cầu nhập kho (bắt buộc):')
  if (!reason || reason.trim() === '') return
  if (!await confirm(`Xác nhận hủy yêu cầu nhập kho với lý do: "${reason}" và cho xe ra cổng luôn?`)) return
  try {
    await $fetch(`${apiBase}/Danhsachxetrongkho/reject-nhapkho/${selectedNhapKhoXe.value.id}`, {
      method: 'POST',
      body: { lyDoHuy: reason }
    })
    showNhapKhoModal.value = false
    fetchData()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra')
  }
}

const submitFinalNhapKho = async () => {
  const docs = Object.keys(draftNhapKhoTickets.value)
  const payload = []
  
  docs.forEach(doc => {
    const isSTO = selectedNhapKhoXe.value?.stOs?.includes(doc) || selectedNhapKhoXe.value?.stos?.includes(doc)
    const isShipment = selectedNhapKhoXe.value?.shipments?.includes(doc)
    
    draftNhapKhoTickets.value[doc].forEach(tk => {
      payload.push({
        maSanPham: tk.maHang,
        tenSanPham: tk.tenSanPham,
        viTri: tk.viTri,
        hanSuDung: tk.hanSuDung,
        ngaySanXuat: tk.ngaySanXuat,
        soLuongChan: tk.nhapChan || 0,
        soLuongLe: tk.nhapLe || 0,
        user: nhanvienInfo.value?.mnv || '',
        tenNhanVien: nhanvienInfo.value?.tnv || '',
        soSTO: isSTO ? doc : null,
        soShipment: isShipment ? doc : null,
        sothe: selectedNhapKhoXe.value?.sothe,
        khohangId: khohangInfo.value?.id
      })
    })
  })
  
  if (payload.length === 0) {
    alert('Phiếu load trống, vui lòng chọn ít nhất 1 sản phẩm!')
    return
  }
  
  if (!await confirm(`Xác nhận ghi nhận nhập kho cho ${payload.length} mặt hàng?`)) return
  
  try {
    loading.value = true
    for (const item of payload) {
      await $fetch(`${apiBase}/PhatSinhSanPhams/NhapKho`, {
        method: 'POST',
        body: item
      })
    }
    
    // Đánh dấu là đã hoàn tất nhập kho cho xe
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-nhapkho/${selectedNhapKhoXe.value.id}`, {
      method: 'POST'
    })
    
    alert('Đã hoàn tất nhập kho!')
    draftNhapKhoTickets.value = {}
    showNhapKhoModal.value = false
    activeNhapKhoTab.value = 1
    fetchData(false)
  } catch (err) {
    console.error(err)
    alert(err.response?._data?.message || 'Có lỗi khi nhập kho')
  } finally {
    loading.value = false
  }
}


const openAuditModal = async (xe) => {
  selectedAuditXe.value = xe
  showAuditModal.value = true
}

const closeAuditModal = async () => {
  showAuditModal.value = false
  selectedAuditXe.value = null
}

const handleAuditSuccess = async () => {
  closeAuditModal()
  fetchData(false)
}

const filteredVehicles = (tabIndex) => {
  const status = tabs[tabIndex].status
  return vehicles.value.filter(v => v.trangThai === status)
}

const now = ref(new Date())

const computeThoiGianTrongSan = (xe) => {
  if (!xe.xacNhanVaoCong_Time) return "00:00:00"
  
  // Backend is sending DateTime already in UTC+7 but marked as UTC (Z). 
  // Strip 'Z' so JS parses it as local time correctly.
  const startStr = xe.xacNhanVaoCong_Time.endsWith('Z') ? xe.xacNhanVaoCong_Time.slice(0, -1) : xe.xacNhanVaoCong_Time;
  const start = new Date(startStr)
  
  let end = now.value
  if (xe.xacNhanRaCong_Time) {
    const endStr = xe.xacNhanRaCong_Time.endsWith('Z') ? xe.xacNhanRaCong_Time.slice(0, -1) : xe.xacNhanRaCong_Time;
    end = new Date(endStr)
  }
  
  const diff = Math.max(0, end - start)
  const hours = Math.floor(diff / 3600000)
  const minutes = Math.floor((diff % 3600000) / 60000)
  const seconds = Math.floor((diff % 60000) / 1000)
  
  return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
}

const fetchData = async (showLoading = true) => {
  if (!khohangInfo.value?.id) return
  if (showLoading) loading.value = true
  try {
    const res = await $fetch(`${apiBase}/Danhsachxetrongkho/dashboard?khohangId=${khohangInfo.value.id}`)
    vehicles.value = res || []
  } catch (err) {
    console.error(err)
    if (showLoading) alert('Có lỗi khi tải danh sách xe')
  } finally {
    if (showLoading) loading.value = false
  }
}

const fetchLayout = async () => {
  if (!khohangInfo.value?.id) return
  loadingLayout.value = true
  try {
    const res = await $fetch(`${apiBase}/Layoutkhos?khohangId=${khohangInfo.value.id}`)
    layoutItems.value = res || []
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải layout kho')
  } finally {
    loadingLayout.value = false
  }
}

watch(activeMenu, (newMenu) => {
  if (newMenu === 'demkho' && layoutItems.value.length === 0) {
    fetchLayout()
  }
  // Nếu chuyển khỏi menu báo cáo thì reset report
  if (newMenu !== 'baocao') {
    activeReport.value = null
    baocaoTonKhoLoading.value = false
  }
})

const fetchDemKhoTam = async () => {
  if (!khohangInfo.value?.id) return
  try {
    const res = await $fetch(`${apiBase}/DemKho/Tam/${khohangInfo.value.id}`)
    currentCountSession.value = res || []
  } catch (err) {
    console.error(err)
  }
}

const startNewCountSession = async () => {
  if (!khohangInfo.value?.id) return
  if (!confirm('Bạn có chắc muốn Bắt đầu đếm kho mới? Toàn bộ dữ liệu Bảng tạm đếm kho (của bạn và người khác) hiện tại sẽ bị XÓA.')) return
  
  try {
    await $fetch(`${apiBase}/DemKho/Tam/Clear/${khohangInfo.value.id}`, {
      method: 'DELETE'
    })
    
    await fetchDemKhoTam()
    alert('Đã xóa dữ liệu bảng tạm. Bạn có thể bắt đầu phiên đếm mới.')
  } catch (err) {
    console.error(err)
    alert(err.data?.title || 'Có lỗi khi xóa bảng tạm')
  }
}

watch(activeDemKhoTab, (newVal) => {
  if (newVal === 'sodem') {
    fetchDemKhoTam()
  }
})

watch(khohangInfo, (newVal) => {
  if (newVal) {
    fetchDemKhoTam()
  }
})

const activeReport = ref(null)

const getLocalDateString = () => {
  const d = new Date()
  const offset = d.getTimezoneOffset() * 60000
  return new Date(d - offset).toISOString().split('T')[0]
}
const nhapxuatDate = ref(getLocalDateString())
const baocaoNhapXuatData = ref([])
const baocaoNhapXuatLoading = ref(false)

const fetchBaocaoNhapXuat = async () => {
  if (!khohangInfo.value?.id) return
  baocaoNhapXuatLoading.value = true
  try {
    const res = await $fetch(`${apiBase}/PhatSinhSanPhams/ByDateAndKhohang?khohangId=${khohangInfo.value.id}&date=${nhapxuatDate.value}`)
    baocaoNhapXuatData.value = res || []
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải báo cáo nhập xuất')
  } finally {
    baocaoNhapXuatLoading.value = false
  }
}

const openReport = async (reportType) => {
  activeReport.value = reportType
  if (reportType === 'tonkho') {
    fetchBaocaoTonKho()
  } else if (reportType === 'nhapxuat') {
    fetchBaocaoNhapXuat()
  } else if (reportType === 'demkho') {
    fetchBaocaoDemKho()
  } else if (reportType === 'dieuchuyen') {
    fetchBaocaoDieuChuyen()
  }
}

const closeReport = async () => {
  activeReport.value = null
}

const parseViTri = (viTri) => {
  if (!viTri) return { day: '', stt: '' }
  const match = viTri.match(/^([a-zA-ZÀ-ỹ\s]+)?(\d+)?$/)
  if (match) {
    return {
      day: match[1] || '',
      stt: match[2] || ''
    }
  }
  return { day: viTri, stt: '' }
}

const getRemainingShelfLifePercentage = (nsx, hsd) => {
  if (!nsx || !hsd) return ''
  const nsxDate = new Date(nsx)
  const hsdDate = new Date(hsd)
  const today = new Date()
  
  const totalDuration = hsdDate.getTime() - nsxDate.getTime()
  if (totalDuration <= 0) return '0%'
  
  const remainingDuration = hsdDate.getTime() - today.getTime()
  if (remainingDuration <= 0) return '0%'
  
  const percent = Math.round((remainingDuration / totalDuration) * 100)
  return `${percent}%`
}

const baocaoTonKhoData = ref([])
const baocaoTonKhoActiveTab = ref('Thành phẩm')

const totalThanhPhamChan = computed(() => {
  return baocaoTonKhoData.value
    .filter(item => {
      const sp = productsList.value.find(p => p.maSanPham === item.maHang)
      return (sp?.loaiSanPham || 'Thành phẩm') === 'Thành phẩm'
    })
    .reduce((sum, item) => sum + (item.soLuongPalletChan || 0), 0)
})

const filteredBaocaoTonKhoData = computed(() => {
  if (baocaoTonKhoActiveTab.value === 'Vỏ') {
    const baseVo = baocaoTonKhoData.value.filter(item => {
      const sp = productsList.value.find(p => p.maSanPham === item.maHang)
      return sp?.loaiSanPham === 'Vỏ'
    })
    
    const palletProduct = productsList.value.find(p => p.maSanPham === '0514')
    const palletName = palletProduct?.tenSanPham || 'Pallet Rental'
    const palletDinhLuong = palletProduct?.dinhLuong || 11
    
    const virtualRow = {
      id: 'virtual_pallet_tp',
      viTri: 'Pallet chứa TP',
      maHang: '0514',
      tenSanPham: palletName,
      ngaySanXuat: null,
      hanSuDung: null,
      soLuongPalletChan: null,
      soThungLe: totalThanhPhamChan.value,
      dinhLuong: palletDinhLuong,
      tong: totalThanhPhamChan.value,
      isVirtual: true
    }
    
    return [virtualRow, ...baseVo]
  } else {
    return baocaoTonKhoData.value.filter(item => {
      const sp = productsList.value.find(p => p.maSanPham === item.maHang)
      return (sp?.loaiSanPham || 'Thành phẩm') === 'Thành phẩm'
    })
  }
})

const baocaoTonKhoThanhPhamCount = computed(() => {
  return baocaoTonKhoData.value.filter(item => {
    const sp = productsList.value.find(p => p.maSanPham === item.maHang)
    return (sp?.loaiSanPham || 'Thành phẩm') === 'Thành phẩm'
  }).length
})

const baocaoTonKhoVoCount = computed(() => {
  const baseVoCount = baocaoTonKhoData.value.filter(item => {
    const sp = productsList.value.find(p => p.maSanPham === item.maHang)
    return sp?.loaiSanPham === 'Vỏ'
  }).length
  return baseVoCount + 1
})

// ==== STATE BÁO CÁO ĐẾM KHO ====
const baocaoDemKhoLoading = ref(false)
const baocaoDemKhoData = ref([])
const activeBaocaoDemKhoTab = ref('choduyet')
const selectedDemKhoReport = ref(null)

// ==== STATE BÁO CÁO ĐIỀU CHUYỂN ====
const dieuChuyenFromDate = ref(getLocalDateString())
const dieuChuyenToDate = ref(getLocalDateString())
const baocaoDieuChuyenData = ref([])
const baocaoDieuChuyenLoading = ref(false)

const fetchBaocaoDieuChuyen = async () => {
  if (!khohangInfo.value?.id) return
  baocaoDieuChuyenLoading.value = true
  try {
    const res = await $fetch(`${apiBase}/DieuChuyenNoiBo/Report/${khohangInfo.value.id}?fromDate=${dieuChuyenFromDate.value}&toDate=${dieuChuyenToDate.value}`)
    baocaoDieuChuyenData.value = res || []
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải báo cáo điều chuyển')
  } finally {
    baocaoDieuChuyenLoading.value = false
  }
}

const exportBaocaoDieuChuyenExcel = () => {
  if (!baocaoDieuChuyenData.value.length) {
    alert('Không có dữ liệu để xuất!')
    return
  }

  const exportData = []
  baocaoDieuChuyenData.value.forEach(lenh => {
    lenh.chiTiets.forEach(ct => {
      exportData.push({
        'Mã Lệnh': lenh.maLenh,
        'Thời gian tạo': new Date(lenh.thoiGianTao).toLocaleString('vi-VN'),
        'Thời gian HT': new Date(lenh.thoiGianHoanThanh).toLocaleString('vi-VN'),
        'Người thực hiện': lenh.nguoiHoanThanh,
        'Trạng thái': lenh.trangThai,
        'Mã SP': ct.maSanPham,
        'Tên SP': ct.tenSanPham,
        'Số lượng chẵn': ct.soLuongChan,
        'Số lượng lẻ': ct.soLuongLe,
        'Từ vị trí': ct.viTriDi,
        'Đến vị trí': ct.viTriDen,
        'NSX': ct.ngaySanXuat ? new Date(ct.ngaySanXuat).toLocaleDateString('vi-VN') : '',
        'HSD': ct.hanSuDung ? new Date(ct.hanSuDung).toLocaleDateString('vi-VN') : ''
      })
    })
  })

  const ws = XLSX.utils.json_to_sheet(exportData)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, 'BaoCaoDieuChuyen')
  XLSX.writeFile(wb, `BaoCaoDieuChuyen_${dieuChuyenFromDate.value}_den_${dieuChuyenToDate.value}.xlsx`)
}

const pendingDemKhoReports = computed(() => {
  return baocaoDemKhoData.value.filter(p => p.trangThai === 'Chờ duyệt')
})

const historyDemKhoReports = computed(() => {
  return baocaoDemKhoData.value.filter(p => p.trangThai !== 'Chờ duyệt')
})

const fetchBaocaoDemKho = async () => {
  if (!khohangInfo.value?.id) return
  baocaoDemKhoLoading.value = true
  try {
    const res = await $fetch(`${apiBase}/DemKho/Kho/${khohangInfo.value.id}`)
    baocaoDemKhoData.value = res
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải báo cáo đếm kho')
  } finally {
    baocaoDemKhoLoading.value = false
  }
}

const duyetDemKho = async (phieuId) => {
  if (!confirm('Bạn có chắc chắn muốn Duyệt phiếu đếm kho này? Tồn kho sẽ được điều chỉnh theo số chênh lệch.')) return
  
  try {
    await $fetch(`${apiBase}/DemKho/Duyet/${phieuId}?nguoiDuyet=${encodeURIComponent(nhanvienInfo.value?.tnv || 'Thủ kho')}`, {
      method: 'POST'
    })
    alert('Duyệt thành công!')
    selectedDemKhoReport.value = null
    fetchBaocaoDemKho()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi duyệt phiếu đếm kho: ' + (err.data || err.message))
  }
}

const khongDuyetDemKho = async (phieuId) => {
  if (!confirm('Bạn có chắc chắn muốn KHÔNG DUYỆT phiếu này? Phiếu sẽ chuyển sang trạng thái Hủy.')) return
  
  try {
    await $fetch(`${apiBase}/DemKho/KhongDuyet/${phieuId}?nguoiDuyet=${encodeURIComponent(nhanvienInfo.value?.tnv || 'Thủ kho')}`, {
      method: 'POST'
    })
    alert('Đã hủy phiếu đếm kho!')
    selectedDemKhoReport.value = null
    fetchBaocaoDemKho()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi hủy phiếu: ' + (err.data || err.message))
  }
}

const openDemKhoReportDetail = (phieu) => {
  selectedDemKhoReport.value = phieu
}

const getUniqueViTriList = (chiTiets) => {
  if (!chiTiets) return []
  // In the demkho list view, we already group or display viTri directly on PhieuDemKho.
  // Wait, in demkho table it was `item.viTri`!
  // So getUniqueViTriList is simple.
  const phieu = baocaoDemKhoData.value.find(p => p.chiTiets === chiTiets)
  if (phieu && phieu.viTri) {
    return [phieu.viTri]
  }
  return []
}

const closeDemKhoReportDetail = () => {
  selectedDemKhoReport.value = null
}

const baocaoTonKhoLoading = ref(false)

const fetchBaocaoTonKho = async () => {
  if (!khohangInfo.value?.id) return
  baocaoTonKhoLoading.value = true
  try {
    // 1. Sync TonKhoHienTai first
    await $fetch(`${apiBase}/Sync/TonKho/${khohangInfo.value.id}`, { method: 'POST' })
    
    // 2. Fetch the updated TonKhoHienTai
    const res = await $fetch(`${apiBase}/TonKhoHienTais/ByKhohang/${khohangInfo.value.id}`)
    baocaoTonKhoData.value = res || []

    // 3. Load products if not loaded yet
    if (productsList.value.length === 0) {
      const sps = await $fetch(`${apiBase}/Sanphams/ByKhohang/${khohangInfo.value.id}`)
      productsList.value = sps || []
    }
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải báo cáo tồn kho')
  } finally {
    baocaoTonKhoLoading.value = false
  }
}

const exportBaocaoTonKhoExcel = async () => {
  if (filteredBaocaoTonKhoData.value.length === 0) return
  
  let dataToExport = []
  if (baocaoTonKhoActiveTab.value === 'Thành phẩm') {
    dataToExport = filteredBaocaoTonKhoData.value.map(item => {
      const { day, stt } = parseViTri(item.viTri)
      return {
        'VỊ TRÍ': item.viTri,
        'MÃ HÀNG': item.maHang,
        'TÊN SẢN PHẨM': item.tenSanPham,
        'HẠN SỬ DỤNG': item.hanSuDung ? new Date(item.hanSuDung).toLocaleDateString('vi-VN') : '',
        '% HẠN SỬ DỤNG CÒN LẠI': getRemainingShelfLifePercentage(item.ngaySanXuat, item.hanSuDung),
        'SỐ LƯỢNG PALLET CHẴN': item.soLuongPalletChan !== null && item.soLuongPalletChan !== 0 ? item.soLuongPalletChan : '',
        'SỐ THÙNG LẺ': item.soThungLe !== null && item.soThungLe !== 0 ? item.soThungLe : '',
        'ĐỊNH LƯỢNG (CS/PL)': item.dinhLuong !== null && item.dinhLuong !== 0 ? item.dinhLuong : '',
        'TỔNG': item.tong !== null && item.tong !== 0 ? item.tong : '',
        'GHI CHÚ': '',
        'TRẠNG THÁI D.O.D': item.trangThaiDOD || 'NORMAL-BÌNH THƯỜNG',
        'NGÀY SẢN XUẤT': item.ngaySanXuat ? new Date(item.ngaySanXuat).toLocaleDateString('vi-VN') : '',
        'DÃY HÀNG': day,
        'STT DÃY HÀNG': stt
      }
    })
  } else {
    dataToExport = filteredBaocaoTonKhoData.value.map(item => ({
      'Vị trí': item.viTri,
      'Mã hàng': item.maHang,
      'Tên hàng': item.tenSanPham,
      'Chẵn': item.soLuongPalletChan !== null && item.soLuongPalletChan !== 0 ? item.soLuongPalletChan : '',
      'Lẻ': item.soThungLe !== null && item.soThungLe !== 0 ? item.soThungLe : '',
      'Định lượng': item.dinhLuong !== null && item.dinhLuong !== 0 ? item.dinhLuong : '',
      'Tổng': item.tong !== null && item.tong !== 0 ? item.tong : '',
      'GHI CHÚ': ''
    }))
  }

  const ws = XLSX.utils.json_to_sheet(dataToExport)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, "BaoCaoTonKho")
  XLSX.writeFile(wb, `BaoCaoTonKho_${baocaoTonKhoActiveTab.value}_${new Date().toISOString().slice(0,10)}.xlsx`)
}

const exportBaocaoNhapXuatExcel = async () => {
  if (baocaoNhapXuatData.value.length === 0) return
  
  const headers = ['THỜI GIAN', 'LOẠI', 'MÃ HÀNG', 'TÊN SẢN PHẨM', 'VỊ TRÍ', 'SỐ LƯỢNG CHẴN', 'SỐ LƯỢNG LẺ', 'SỐ XE', 'CHỨNG TỪ', 'NGƯỜI THAO TÁC']
  const rows = baocaoNhapXuatData.value.map(item => [
    formatDate(item.thoiGian, true),
    item.loaiPhatSinh,
    item.maSanPham,
    item.tenSanPham,
    item.viTri,
    item.soLuongChan || 0,
    item.soLuongLe || 0,
    item.sothe || '',
    item.soSTO || item.soShipment || '',
    item.tenNhanVien || item.user || ''
  ])
  
  const worksheet = XLSX.utils.aoa_to_sheet([headers, ...rows])
  const workbook = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(workbook, worksheet, "BaoCaoNhapXuat")
  
  const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' })
  const blob = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' })
  
  const url = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = url
  a.download = `BaoCaoNhapXuat_${khohangInfo.value?.name}_${nhapxuatDate.value}.xlsx`
  document.body.appendChild(a)
  a.click()
  document.body.removeChild(a)
  URL.revokeObjectURL(url)
}

const formatDate = (dateStr, includeTime = false) => {
  if (!dateStr) return '-'
  if (typeof dateStr === 'string' && dateStr.endsWith('Z')) {
    dateStr = dateStr.slice(0, -1)
  }
  const date = new Date(dateStr)
  const d = date.getDate().toString().padStart(2, '0')
  const m = (date.getMonth() + 1).toString().padStart(2, '0')
  const y = date.getFullYear()
  if (!includeTime) return `${d}/${m}/${y}`
  const hh = date.getHours().toString().padStart(2, '0')
  const mm = date.getMinutes().toString().padStart(2, '0')
  return `${hh}:${mm} ${d}/${m}/${y}`
}

const updateStatus = async (id, newStatus, xe = null) => {
  if (newStatus === 2 && xe && xe.lyDoVaoKho) {
    if (xe.lyDoVaoKho.toLowerCase().includes('xuất') && !xe.xuatKho_Time) {
      alert("Vui lòng hoàn tất xuất kho (ở mục Xuất Kho) để xe chuyển sang danh sách Đang cẩu hàng trước khi Hoàn tất Xuất hàng!")
      return
    }
    if (xe.lyDoVaoKho.toLowerCase().includes('nhập') && !xe.nhapKho_Time) {
      alert("Vui lòng hoàn tất nhập kho (ở mục Nhập Kho) để xe chuyển sang danh sách Đang cẩu hàng trước khi Hoàn tất Nhập hàng!")
      return
    }
  }

  if (!await confirm('Xác nhận cập nhật trạng thái?')) return
  try {
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-status/${id}`, {
      method: 'POST',
      body: newStatus
    })
    fetchData()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra')
  }
}

const updateChangBuoc = async (id) => {
  if (!await confirm('Xác nhận xe đã chằng buộc an toàn?')) return
  try {
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-changbuoc/${id}`, {
      method: 'POST'
    })
    fetchData()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra')
  }
}

const logout = async () => {
  if (process.client) {
    localStorage.removeItem('employee_info')
    localStorage.removeItem('employee_khohang')
  }
  router.push('/')
}

onMounted(() => {
  if (process.client) {
    const infoStr = localStorage.getItem('employee_info')
    const khoStr = localStorage.getItem('employee_khohang')
    
    if (infoStr && khoStr) {
      nhanvienInfo.value = JSON.parse(infoStr)
      khohangInfo.value = JSON.parse(khoStr)
      fetchData(true)
      
      // Setup background fetch without loading blink
      const fetchInterval = setInterval(() => {
        fetchData(false)
      }, 30000)
      
      // Local clock interval for UI timer
      const clockInterval = setInterval(() => {
        now.value = new Date()
      }, 1000)
      
      onUnmounted(() => {
        clearInterval(fetchInterval)
        clearInterval(clockInterval)
      })
    } else {
      router.push('/')
    }
  }
})
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  height: 4px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: #f1f5f9; 
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: #cbd5e1; 
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: #94a3b8; 
}
</style>

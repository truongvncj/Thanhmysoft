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
                <button @click="updateStatus(xe.id, 2, xe)" class="w-full bg-orange-500 hover:bg-orange-600 text-white font-bold py-2.5 rounded-xl transition-colors">
                  Hoàn tất Nhập/Xuất
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
                    <button @click="updateStatus(xe.id, 2, xe)" class="inline-block bg-orange-500 hover:bg-orange-600 text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm">
                      Hoàn tất Nhập/Xuất
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
            <h2 class="text-xl font-bold text-slate-800">Danh sách xe đang Nhập Kho</h2>
          </div>
          
          <div class="p-4 sm:p-6 overflow-auto flex-1 bg-slate-50">
            <div v-if="nhapKhoVehicles.length === 0" class="text-center py-12 text-slate-500 bg-white rounded-xl border border-slate-200">
              <svg class="w-16 h-16 mx-auto text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"></path></svg>
              <p>Hiện tại không có xe nào đang nhập kho trong sân.</p>
            </div>
            
            <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              <div v-for="xe in nhapKhoVehicles" :key="xe.id" class="bg-white rounded-2xl shadow-sm border border-blue-100 p-5 hover:shadow-md transition-shadow relative overflow-hidden">
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
            <h2 class="text-xl font-bold text-slate-800">Sơ đồ kho: {{ khohangInfo?.name }}</h2>
            <button @click="fetchLayout" class="text-sm bg-blue-50 text-blue-600 px-3 py-1.5 rounded-lg hover:bg-blue-100 font-semibold transition-colors">
              Làm mới sơ đồ
            </button>
          </div>
          
          <div 
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
                :class="[
                  'absolute flex items-center justify-center select-none transition-shadow group',
                  (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? 'bg-slate-800 shadow-sm border-0' : 
                  item.elementType === 'text' ? `bg-transparent text-slate-800 shadow-none border-0 ring-0 outline-none leading-none ${item.hang === 'bold' ? 'font-bold' : 'font-normal'}` : 
                  'bg-white border-2 border-blue-500 shadow-md rounded hover:shadow-lg'
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
        </div>

        <div v-else-if="activeMenu === 'dieuchuyen'" class="bg-white rounded-xl shadow-sm border border-slate-200 p-8 text-center text-slate-500 h-full flex flex-col items-center justify-center">
          <h2 class="text-xl font-bold mb-2 text-slate-800">Điều chuyển nội bộ</h2>
          <p>Chức năng đang được phát triển...</p>
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
              <button @click="exportBaocaoTonKhoExcel" :disabled="baocaoTonKhoData.length === 0" class="px-4 py-2 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-emerald-200 rounded-lg shadow-sm flex items-center gap-2 font-medium">
                <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path></svg>
                Kết xuất Excel
              </button>
            </div>
            
            <div class="flex-1 overflow-auto custom-scrollbar p-0">
              <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
                <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                  <tr>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ HÀNG</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN SẢN PHẨM</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SL PALLET CHẴN</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SỐ THÙNG LẺ</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">ĐỊNH LƯỢNG (CS/PL)</th>
                    <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">TỔNG</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="baocaoTonKhoLoading" class="bg-white">
                    <td colspan="7" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="baocaoTonKhoData.length === 0" class="bg-white">
                    <td colspan="7" class="py-8 text-center text-slate-500">Không có dữ liệu tồn kho</td>
                  </tr>
                  <tr v-for="item in baocaoTonKhoData" :key="item.id" class="border-b border-slate-100 hover:bg-blue-50/50 transition-colors">
                    <td class="py-2 px-4 border-r border-slate-100">{{ item.viTri }}</td>
                    <td class="py-2 px-4 border-r border-slate-100 font-semibold">{{ item.maHang }}</td>
                    <td class="py-2 px-4 border-r border-slate-100 min-w-[200px] truncate max-w-xs" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                    <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soLuongPalletChan }}</td>
                    <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soThungLe }}</td>
                    <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.dinhLuong }}</td>
                    <td class="py-2 px-4 text-right font-bold">{{ item.tong }}</td>
                  </tr>
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
                    <td colspan="10" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="baocaoNhapXuatData.length === 0" class="bg-white">
                    <td colspan="10" class="py-8 text-center text-slate-500">Không có dữ liệu phát sinh trong ngày này.</td>
                  </tr>
                  <tr v-for="item in baocaoNhapXuatData" :key="item.id" class="hover:bg-blue-50/50 transition-colors">
                    <td class="py-3 px-4 border-r border-slate-100">{{ formatDate(item.thoiGian, true) }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 font-bold" :class="item.loaiPhatSinh === 'Nhập' ? 'text-blue-600' : 'text-red-600'">{{ item.loaiPhatSinh }}</td>
                    <td class="py-3 px-4 border-r border-slate-100">{{ item.maSanPham }}</td>
                    <td class="py-3 px-4 border-r border-slate-100 whitespace-normal min-w-[200px]">{{ item.tenSanPham }}</td>
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
        </div>
        </main>
      </div>
    </div>

    <!-- Modal 1: Chi tiết xuất kho (Shipment & STO) -->
    <div v-if="showXuatKhoModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-lg shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Chi tiết xe xuất: {{ selectedXuatKhoXe?.bienSo }}</h3>
          <button @click="showXuatKhoModal = false" class="text-slate-400 hover:text-slate-600">
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
          
          <div>
            <h4 class="font-semibold text-slate-700 mb-3 border-b pb-2">Số STO <span class="text-xs text-slate-400 font-normal">(Nhấn để chọn xuất kho)</span></h4>
            <div v-if="selectedXuatKhoXe?.stOs?.length || selectedXuatKhoXe?.stos?.length" class="space-y-4">
              <div v-for="sto in (selectedXuatKhoXe.stOs || selectedXuatKhoXe.stos)" :key="sto" class="flex flex-col gap-2">
                <div @click="openXuatKhoSTOModal(sto)" class="p-3 bg-emerald-50 text-emerald-800 rounded-lg font-medium border border-emerald-100 cursor-pointer hover:bg-emerald-100 transition-colors flex justify-between items-center group shadow-sm">
                  <span>{{ sto }}</span>
                  <div class="flex items-center gap-2">
                    <span class="text-xs bg-white px-2 py-1 rounded text-emerald-600 border border-emerald-200 shadow-sm" v-if="draftLoadTickets[sto]?.length">{{ draftLoadTickets[sto].length }} mục đã chọn</span>
                    <svg class="w-5 h-5 text-emerald-400 group-hover:text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                  </div>
                </div>
                <!-- Draft items list -->
                <div v-if="draftLoadTickets[sto]?.length" class="pl-4 border-l-2 border-emerald-200 space-y-1">
                  <div v-for="item in draftLoadTickets[sto]" :key="item.id" class="bg-slate-50 text-xs px-3 py-2 rounded border border-slate-200 text-slate-700">
                    <span class="font-bold text-emerald-700">{{ item.maHang }}</span> - {{ item.tenSanPham }}
                    <span class="text-slate-500 ml-2">(Vị trí: <span class="font-bold text-slate-700">{{ item.viTri }}</span>)</span>
                    <span class="ml-2 font-medium text-emerald-600">SL: {{ item.xuatChan }} chẵn / {{ item.xuatLe }} lẻ</span>
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="text-slate-500 italic text-sm">Không có STO</div>
          </div>
        </div>
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-slate-50 gap-3">
          <button @click="showXuatKhoModal = false" class="px-4 py-2 bg-white border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-50 font-medium">Đóng</button>
          <button @click="rejectXuatKho" class="px-4 py-2 bg-red-600 text-white border border-transparent rounded-lg hover:bg-red-700 font-bold shadow-sm flex items-center gap-2">
            Hủy yêu cầu
          </button>
          <button @click="submitFinalXuatKho" class="px-4 py-2 bg-blue-600 text-white border border-transparent rounded-lg hover:bg-blue-700 font-bold shadow-sm flex items-center gap-2">
             <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
             Xác nhận Xuất kho
          </button>
        </div>
      </div>
    </div>

    <!-- Modal 2: Chọn sản phẩm và vị trí xuất kho -->
    <div v-if="showXuatKhoSTOModal" class="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-4xl shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Xuất kho {{ selectedXuatKhoType }}: {{ selectedXuatKhoDoc }}</h3>
          <button @click="showXuatKhoSTOModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1 flex flex-col gap-6 bg-slate-50/50">
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
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Chẵn</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Lẻ</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Xuất Chẵn</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Xuất Lẻ</th>
                  </tr>
                </thead>
                <tbody class="bg-white divide-y divide-slate-100">
                  <tr v-if="loadingTonKho" class="bg-slate-50/50">
                    <td colspan="5" class="px-4 py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="!tonKhoList.length" class="bg-slate-50/50">
                    <td colspan="5" class="px-4 py-8 text-center text-slate-500">Không có tồn kho cho sản phẩm này</td>
                  </tr>
                  <tr v-for="tk in tonKhoList" :key="tk.id" class="hover:bg-slate-50 transition-colors">
                    <td class="px-4 py-3 text-sm font-medium text-slate-900">{{ tk.viTri }}</td>
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
                 Chọn (Thêm vào phiếu Load)
               </button>
            </div>
          </div>
          
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
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-white">
          <button @click="showXuatKhoSTOModal = false" class="px-4 py-2 bg-slate-100 border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-200 font-medium">Đóng</button>
        </div>
      </div>
    </div>

    <!-- Nhập Kho Modal -->
    <div v-if="showNhapKhoModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white rounded-xl w-full max-w-lg shadow-xl overflow-hidden flex flex-col max-h-[90vh]">
        <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h3 class="text-lg font-bold text-slate-800">Chi tiết xe xuất: {{ selectedNhapKhoXe?.bienSo }}</h3>
          <button @click="showNhapKhoModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        
        <div class="p-6 overflow-y-auto">
          <div class="mb-6">
            <h4 class="font-semibold text-slate-700 mb-3 border-b pb-2">Số Shipment <span class="text-xs text-slate-400 font-normal">(Nhấn để chọn nhập kho)</span></h4>
            <div v-if="selectedNhapKhoXe?.shipments?.length" class="space-y-4">
              <div v-for="shipment in selectedNhapKhoXe.shipments" :key="shipment" class="flex flex-col gap-2">
                <div @click="openNhapKhoShipmentModal(shipment)" class="p-3 bg-blue-50 text-blue-800 rounded-lg font-medium border border-blue-100 cursor-pointer hover:bg-blue-100 transition-colors flex justify-between items-center group shadow-sm">
                  <span>{{ shipment }}</span>
                  <div class="flex items-center gap-2">
                    <span class="text-xs bg-white px-2 py-1 rounded text-blue-600 border border-blue-200 shadow-sm" v-if="draftNhapKhoTickets[shipment]?.length">{{ draftNhapKhoTickets[shipment].length }} mục đã chọn</span>
                    <svg class="w-5 h-5 text-blue-400 group-hover:text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                  </div>
                </div>
                <!-- Draft items list -->
                <div v-if="draftNhapKhoTickets[shipment]?.length" class="pl-4 border-l-2 border-blue-200 space-y-1">
                  <div v-for="item in draftNhapKhoTickets[shipment]" :key="item.id" class="bg-slate-50 text-xs px-3 py-2 rounded border border-slate-200 text-slate-700">
                    <span class="font-bold text-blue-700">{{ item.maHang }}</span> - {{ item.tenSanPham }}
                    <span class="text-slate-500 ml-2">(Vị trí: <span class="font-bold text-slate-700">{{ item.viTri }}</span>)</span>
                    <span class="ml-2 font-medium text-emerald-600">SL: {{ item.nhapChan }} chẵn / {{ item.nhapLe }} lẻ</span>
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="text-slate-500 italic text-sm">Không có Shipment</div>
          </div>
          
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
          <button @click="submitFinalNhapKho" class="px-4 py-2 bg-blue-600 text-white border border-transparent rounded-lg hover:bg-blue-700 font-bold shadow-sm flex items-center gap-2">
             <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
             Xác nhận Nhập kho
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
        
        <div class="p-6 overflow-y-auto flex-1 flex flex-col gap-6 bg-slate-50/50">
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

          <!-- Kết quả Tồn Kho -->
          <div v-if="selectedProductIdForNhapKho" class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden flex flex-col z-0">
            <div class="p-4 border-b border-slate-100 bg-slate-50 flex justify-between items-center">
              <h4 class="font-semibold text-slate-700">Chọn vị trí và nhập số lượng xuất</h4>
              <div class="flex gap-4">
                <button @click="addNewLocationForNhapKho" class="text-sm text-emerald-600 hover:text-emerald-800 flex items-center font-medium">
                  <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                  Vị trí mới
                </button>
                <button @click="fetchTonKhoLocationsForNhapKho" class="text-sm text-blue-600 hover:text-blue-800 flex items-center font-medium">
                  <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15"></path></svg>
                  Làm mới
                </button>
              </div>
            </div>
            
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-slate-200">
                <thead class="bg-slate-50">
                  <tr>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Vị trí</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Chẵn (Hiện tại)</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider">Tồn Lẻ (Hiện tại)</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Nhập Chẵn</th>
                    <th scope="col" class="px-4 py-3 text-center text-xs font-semibold text-slate-500 uppercase tracking-wider w-32">Nhập Lẻ</th>
                  </tr>
                </thead>
                <tbody class="bg-white divide-y divide-slate-100">
                  <tr v-if="loadingTonKho" class="bg-slate-50/50">
                    <td colspan="5" class="px-4 py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                  </tr>
                  <tr v-else-if="!tonKhoList.length" class="bg-slate-50/50">
                    <td colspan="5" class="px-4 py-8 text-center text-slate-500">Không có tồn kho cho sản phẩm này</td>
                  </tr>
                  <tr v-for="tk in tonKhoList" :key="tk.id" class="hover:bg-slate-50 transition-colors">
                    <td class="px-4 py-3 text-sm font-medium text-slate-900">
                      <input v-if="tk.isNew" type="text" v-model="tk.viTri" placeholder="Vị trí..." class="w-full text-center border-slate-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm py-1.5" />
                      <template v-else>{{ tk.viTri }}</template>
                    </td>
                    <td class="px-4 py-3 text-sm text-center text-slate-600 font-medium">{{ tk.soLuongPalletChan }}</td>
                    <td class="px-4 py-3 text-sm text-center text-slate-600 font-medium">{{ tk.soThungLe }}</td>
                    <td class="px-4 py-3 text-sm text-center">
                      <input type="number" v-model.number="tk.nhapChan" min="0"  class="w-full text-center border-slate-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm py-1.5" />
                    </td>
                    <td class="px-4 py-3 text-sm text-center">
                      <input type="number" v-model.number="tk.nhapLe" min="0"  class="w-full text-center border-slate-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm py-1.5" />
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            
            <div v-if="tonKhoList.length > 0" class="p-4 bg-slate-50 border-t border-slate-100 flex justify-end">
               <button @click="addNhapKhoDraftTicket" class="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-sm transition-colors flex items-center gap-2">
                 <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                 Chọn (Thêm vào phiếu cẩu hàng)
               </button>
            </div>
          </div>
          
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
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Chẵn</th>
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase">SL Lẻ</th>
                      <th class="px-4 py-2 text-center text-xs font-semibold text-slate-500 uppercase w-16">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody class="bg-white divide-y divide-slate-100">
                    <tr v-if="!draftNhapKhoTickets[selectedNhapKhoDoc] || draftNhapKhoTickets[selectedNhapKhoDoc].length === 0">
                       <td colspan="6" class="px-4 py-6 text-center text-slate-500 italic">Chưa có mặt hàng nào được chọn</td>
                    </tr>
                    <tr v-for="item in draftNhapKhoTickets[selectedNhapKhoDoc]" :key="item.id" class="hover:bg-slate-50">
                       <td class="px-4 py-2 text-sm font-bold text-blue-700">{{ item.maHang }}</td>
                       <td class="px-4 py-2 text-sm text-slate-700 max-w-[200px] truncate">{{ item.tenSanPham }}</td>
                       <td class="px-4 py-2 text-sm font-medium text-slate-800">{{ item.viTri }}</td>
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
        
        <div class="px-6 py-4 border-t border-slate-100 flex justify-end bg-white">
          <button @click="showNhapKhoSTOModal = false" class="px-4 py-2 bg-slate-100 border border-slate-300 rounded-lg text-slate-700 hover:bg-slate-200 font-medium">Đóng</button>
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
              <h2 class="text-2xl font-bold text-slate-800">Phiếu cẩu hàng</h2>
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
            <h3 class="font-bold text-slate-800 p-4 border-b border-slate-100 bg-slate-50">Chi tiết hàng lấy tại các vị trí</h3>
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
                    <td colspan="5" class="px-4 py-6 text-center text-slate-500">Không có dữ liệu xuất kho.</td>
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
                    <td colspan="2" class="px-4 py-3 font-bold text-slate-800 text-right">TỔNG CỘNG LOAD:</td>
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

const onLayoutMouseDown = (e) => {
  isDraggingLayout.value = true
  startDragX.value = e.clientX
  startDragY.value = e.clientY
  startScrollLeft.value = layoutContainer.value.scrollLeft
  startScrollTop.value = layoutContainer.value.scrollTop
}

const onLayoutMouseMove = (e) => {
  if (!isDraggingLayout.value || !layoutContainer.value) return
  e.preventDefault()
  const dx = e.clientX - startDragX.value
  const dy = e.clientY - startDragY.value
  layoutContainer.value.scrollLeft = startScrollLeft.value - dx
  layoutContainer.value.scrollTop = startScrollTop.value - dy
}

const onLayoutMouseUp = () => {
  isDraggingLayout.value = false
}

const onLayoutMouseLeave = () => {
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

const hasPermission = (code) => {
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
const draftLoadTickets = ref({})

const productsList = ref([])
const selectedProductIdForXuatKho = ref('')
const tonKhoList = ref([])
const loadingTonKho = ref(false)

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
    const res = await $fetch(`${apiBase}/PhatSinhSanPhams/BySothe`, {
      params: { sothe: xe.sothe }
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

const printTicket = () => {
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
const openXuatKhoModal = (xe) => {
  if (activeXuatKhoTab.value === 1) {
    openViewLoadTicketModal(xe)
    return
  }
  selectedXuatKhoXe.value = xe
  showXuatKhoModal.value = true
}

const openXuatKhoSTOModal = (sto) => {
  selectedXuatKhoType.value = 'STO'
  selectedXuatKhoDoc.value = sto
  showXuatKhoSTOModal.value = true
  fetchProducts()
  xuatKhoSearchQuery.value = ''
  selectedProductIdForXuatKho.value = ''
  tonKhoList.value = []
}

const openXuatKhoShipmentModal = (shipment) => {
  selectedXuatKhoType.value = 'Shipment'
  selectedXuatKhoDoc.value = shipment
  showXuatKhoSTOModal.value = true
  fetchProducts()
  xuatKhoSearchQuery.value = ''
  selectedProductIdForXuatKho.value = ''
  tonKhoList.value = []
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

const selectXuatKhoProduct = (sp) => {
  xuatKhoSearchQuery.value = sp.maSanPham
  selectedProductIdForXuatKho.value = sp.maSanPham
  showXuatKhoProductDropdown.value = false
  fetchTonKhoLocations()
}

const handleXuatKhoProductBlur = () => {
  showXuatKhoProductDropdown.value = false
}

const onXuatKhoProductInput = (e) => {
  xuatKhoSearchQuery.value = e.target.value
  selectedProductIdForXuatKho.value = '' 
  tonKhoList.value = []
  showXuatKhoProductDropdown.value = true
  selectedXuatKhoProductIndex.value = -1
}

const onXuatKhoArrowDown = () => {
  if (!showXuatKhoProductDropdown.value) {
    showXuatKhoProductDropdown.value = true
    return
  }
  if (selectedXuatKhoProductIndex.value < filteredXuatKhoProducts.value.length - 1) {
    selectedXuatKhoProductIndex.value++
    scrollXuatKhoItem()
  }
}

const onXuatKhoArrowUp = () => {
  if (selectedXuatKhoProductIndex.value > 0) {
    selectedXuatKhoProductIndex.value--
    scrollXuatKhoItem()
  }
}

const onXuatKhoEnter = (e) => {
  e.preventDefault()
  if (showXuatKhoProductDropdown.value && selectedXuatKhoProductIndex.value >= 0 && selectedXuatKhoProductIndex.value < filteredXuatKhoProducts.value.length) {
    selectXuatKhoProduct(filteredXuatKhoProducts.value[selectedXuatKhoProductIndex.value])
  } else {
    showXuatKhoProductDropdown.value = false
  }
}

const scrollXuatKhoItem = () => {
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
    const res = await $fetch(`${apiBase}/Sanphamnuocs/ByKhohang/${khohangInfo.value.id}`)
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

const addDraftTicket = () => {
  const itemsToAdd = tonKhoList.value.filter(tk => tk.xuatChan > 0 || tk.xuatLe > 0)
  if (itemsToAdd.length === 0) {
    alert('Vui lòng nhập số lượng xuất cho ít nhất 1 vị trí!')
    return
  }
  
  const productInfo = productsList.value.find(p => p.maSanPham === selectedProductIdForXuatKho.value)
  const doc = selectedXuatKhoDoc.value
  
  if (!draftLoadTickets.value[doc]) {
    draftLoadTickets.value[doc] = []
  }
  
  itemsToAdd.forEach(tk => {
    const existing = draftLoadTickets.value[doc].find(d => d.maHang === tk.maHang && d.viTri === tk.viTri)
    if (existing) {
      existing.xuatChan = (existing.xuatChan || 0) + (tk.xuatChan || 0)
      existing.xuatLe = (existing.xuatLe || 0) + (tk.xuatLe || 0)
    } else {
      draftLoadTickets.value[doc].push({
        id: Date.now() + Math.random(),
        maHang: tk.maHang,
        tenSanPham: productInfo?.tenSanPham || tk.tenSanPham,
        viTri: tk.viTri,
        hanSuDung: tk.hanSuDung,
        xuatChan: tk.xuatChan || 0,
        xuatLe: tk.xuatLe || 0
      })
    }
    tk.xuatChan = 0
    tk.xuatLe = 0
  })
}

const removeDraftItem = (doc, itemId) => {
  if (draftLoadTickets.value[doc]) {
    draftLoadTickets.value[doc] = draftLoadTickets.value[doc].filter(i => i.id !== itemId)
  }
}
const rejectXuatKho = async () => {
  const reason = prompt('Vui lòng nhập lý do hủy yêu cầu xuất kho (bắt buộc):')
  if (!reason || reason.trim() === '') return
  if (!confirm(`Xác nhận hủy yêu cầu xuất kho với lý do: "${reason}" và cho xe ra cổng luôn?`)) return
  try {
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
  
  if (!confirm(`Xác nhận ghi nhận xuất kho cho ${payload.length} mặt hàng?`)) return
  
  try {
    loading.value = true
    for (const item of payload) {
      await $fetch(`${apiBase}/PhatSinhSanPhams/XuatKho`, {
        method: 'POST',
        body: item
      })
    }
    
    // Đánh dấu là đã hoàn tất xuất kho cho xe
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-xuatkho/${selectedXuatKhoXe.value.id}`, {
      method: 'POST'
    })
    
    alert('Đã hoàn tất xuất kho!')
    draftLoadTickets.value = {}
    showXuatKhoModal.value = false
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
const selectedNhapKhoType = ref('STO')
const selectedNhapKhoDoc = ref('')
const draftNhapKhoTickets = ref({})

const selectedProductIdForNhapKho = ref('')

const showNhapKhoProductDropdown = ref(false)
const selectedNhapKhoProductIndex = ref(-1)
const nhapKhoDropdownList = ref(null)
const nhapKhoSearchQuery = ref('')

// --- XUẤT KHO ---
const openNhapKhoModal = (xe) => {
  if (activeNhapKhoTab.value === 1) {
    openViewLoadTicketModal(xe)
    return
  }
  selectedNhapKhoXe.value = xe
  showNhapKhoModal.value = true
}

const openNhapKhoSTOModal = (sto) => {
  selectedNhapKhoType.value = 'STO'
  selectedNhapKhoDoc.value = sto
  showNhapKhoSTOModal.value = true
  fetchProducts()
  nhapKhoSearchQuery.value = ''
  selectedProductIdForNhapKho.value = ''
  tonKhoList.value = []
}

const openNhapKhoShipmentModal = (shipment) => {
  selectedNhapKhoType.value = 'Shipment'
  selectedNhapKhoDoc.value = shipment
  showNhapKhoSTOModal.value = true
  fetchProducts()
  nhapKhoSearchQuery.value = ''
  selectedProductIdForNhapKho.value = ''
  tonKhoList.value = []
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

const selectNhapKhoProduct = (sp) => {
  nhapKhoSearchQuery.value = sp.maSanPham
  selectedProductIdForNhapKho.value = sp.maSanPham
  showNhapKhoProductDropdown.value = false
  fetchTonKhoLocationsForNhapKho()
}

const handleNhapKhoProductBlur = () => {
  showNhapKhoProductDropdown.value = false
}

const onNhapKhoProductInput = (e) => {
  nhapKhoSearchQuery.value = e.target.value
  selectedProductIdForNhapKho.value = '' 
  tonKhoList.value = []
  showNhapKhoProductDropdown.value = true
  selectedNhapKhoProductIndex.value = -1
}

const onNhapKhoArrowDown = () => {
  if (!showNhapKhoProductDropdown.value) {
    showNhapKhoProductDropdown.value = true
    return
  }
  if (selectedNhapKhoProductIndex.value < filteredNhapKhoProducts.value.length - 1) {
    selectedNhapKhoProductIndex.value++
    scrollNhapKhoItem()
  }
}

const onNhapKhoArrowUp = () => {
  if (selectedNhapKhoProductIndex.value > 0) {
    selectedNhapKhoProductIndex.value--
    scrollNhapKhoItem()
  }
}

const onNhapKhoEnter = (e) => {
  e.preventDefault()
  if (showNhapKhoProductDropdown.value && selectedNhapKhoProductIndex.value >= 0 && selectedNhapKhoProductIndex.value < filteredNhapKhoProducts.value.length) {
    selectNhapKhoProduct(filteredNhapKhoProducts.value[selectedNhapKhoProductIndex.value])
  } else {
    showNhapKhoProductDropdown.value = false
  }
}

const scrollNhapKhoItem = () => {
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

const addNewLocationForNhapKho = () => {
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

const addNhapKhoDraftTicket = () => {
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
        nhapChan: tk.nhapChan || 0,
        nhapLe: tk.nhapLe || 0
      })
    }
    tk.nhapChan = 0
    tk.nhapLe = 0
  })
}

const removeNhapKhoDraftItem = (doc, itemId) => {
  if (draftNhapKhoTickets.value[doc]) {
    draftNhapKhoTickets.value[doc] = draftNhapKhoTickets.value[doc].filter(i => i.id !== itemId)
  }
}
const rejectNhapKho = async () => {
  const reason = prompt('Vui lòng nhập lý do hủy yêu cầu nhập kho (bắt buộc):')
  if (!reason || reason.trim() === '') return
  if (!confirm(`Xác nhận hủy yêu cầu nhập kho với lý do: "${reason}" và cho xe ra cổng luôn?`)) return
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
  
  if (!confirm(`Xác nhận ghi nhận nhập kho cho ${payload.length} mặt hàng?`)) return
  
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
    fetchData(false)
  } catch (err) {
    console.error(err)
    alert(err.response?._data?.message || 'Có lỗi khi nhập kho')
  } finally {
    loading.value = false
  }
}


const openAuditModal = (xe) => {
  selectedAuditXe.value = xe
  showAuditModal.value = true
}

const closeAuditModal = () => {
  showAuditModal.value = false
  selectedAuditXe.value = null
}

const handleAuditSuccess = () => {
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

const activeReport = ref(null)

const nhapxuatDate = ref(new Date().toISOString().split('T')[0])
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

const openReport = (reportType) => {
  activeReport.value = reportType
  if (reportType === 'tonkho') {
    fetchBaocaoTonKho()
  } else if (reportType === 'nhapxuat') {
    fetchBaocaoNhapXuat()
  }
}

const closeReport = () => {
  activeReport.value = null
}

const baocaoTonKhoData = ref([])
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
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải báo cáo tồn kho')
  } finally {
    baocaoTonKhoLoading.value = false
  }
}

const exportBaocaoTonKhoExcel = () => {
  if (baocaoTonKhoData.value.length === 0) return
  const dataToExport = baocaoTonKhoData.value.map(item => ({
    'Vị Trí': item.viTri,
    'Mã Hàng': item.maHang,
    'Tên Sản Phẩm': item.tenSanPham,
    'SL Pallet Chẵn': item.soLuongPalletChan,
    'Số Thùng Lẻ': item.soThungLe,
    'Định Lượng (CS/PL)': item.dinhLuong,
    'Tổng': item.tong
  }))

  const ws = XLSX.utils.json_to_sheet(dataToExport)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, "BaoCaoTonKho")
  XLSX.writeFile(wb, `BaoCaoTonKho_${new Date().toISOString().slice(0,10)}.xlsx`)
}

const exportBaocaoNhapXuatExcel = () => {
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
  if (newStatus === 2 && xe && xe.lyDoVaoKho && xe.lyDoVaoKho.toLowerCase().includes('xuất')) {
    if (!xe.xuatKho_Time) {
      alert("Vui lòng hoàn tất xuất kho (ở mục Xuất Kho) để xe chuyển sang danh sách Đang cẩu hàng trước khi Hoàn tất Nhập/Xuất!")
      return
    }
  }

  if (!confirm('Xác nhận cập nhật trạng thái?')) return
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
  if (!confirm('Xác nhận xe đã chằng buộc an toàn?')) return
  try {
    await $fetch(`${apiBase}/Danhsachxetrongkho/update-changbuoc/${id}`, {
      method: 'POST'
    })
    fetchData()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra')
  }
}

const logout = () => {
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

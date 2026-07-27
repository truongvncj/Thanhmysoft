<template>
  <div class="min-h-screen bg-slate-50 flex flex-col">
    <!-- Header -->
    <header class="bg-white shadow-sm px-6 py-4 flex items-center justify-between sticky top-0 z-10 border-b border-slate-200">
      <div class="flex items-center gap-4">
        <NuxtLink to="/admin" class="text-slate-400 hover:text-blue-600 transition-colors">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
        </NuxtLink>
        <div>
          <h1 class="text-xl font-bold text-slate-800 tracking-tight">Quản lý Tồn Kho Đầu Kỳ</h1>
          <p class="text-sm text-slate-500 mt-1">Lưu trữ báo cáo lượng hàng tồn hiện tại</p>
        </div>
      </div>
    </header>

    <div class="flex-1 p-6 max-w-[1600px] mx-auto w-full flex flex-col gap-6">
      
      <!-- Top Action Bar -->
      <div class="bg-white p-4 rounded-2xl shadow-sm border border-slate-200 flex flex-col gap-4 z-20">
        <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <!-- Chọn Kho -->
          <div class="flex items-center gap-3 w-full sm:w-auto">
            <label class="font-semibold text-slate-700 whitespace-nowrap">Chọn Kho:</label>
            <select 
              v-model="selectedKhohangId" 
              @change="fetchTonKhoData"
              class="w-full sm:w-64 border border-slate-300 rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all outline-none bg-slate-50"
            >
              <option value="" disabled>-- Chọn kho hàng --</option>
              <option v-for="kho in khohangs" :key="kho.id" :value="kho.id">
                {{ kho.tenKho }}
              </option>
            </select>
          </div>
          
          <!-- Excel Action Buttons -->
          <div class="flex items-center gap-2 w-full sm:w-auto" v-if="selectedKhohangId && globalThoiDiemGhiNhan">
            <button @click="exportExcel" class="flex-1 sm:flex-none px-3 py-1.5 rounded-full text-sm font-medium bg-emerald-50 text-emerald-600 hover:bg-emerald-100 transition-colors border border-emerald-200 shadow-sm flex items-center justify-center gap-1" title="Tải xuống File Excel">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" /></svg>
              <span>Kết xuất Excel</span>
            </button>
            <button @click="triggerUpload" class="flex-1 sm:flex-none px-3 py-1.5 rounded-full text-sm font-medium bg-blue-50 text-blue-600 hover:bg-blue-100 transition-colors border border-blue-200 shadow-sm flex items-center justify-center gap-1" title="Tải lên dữ liệu">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" /></svg>
              <span>Upload Excel</span>
            </button>
            <input type="file" ref="fileInput" @change="handleFileUpload" accept=".xlsx, .xls" class="hidden" />
          </div>
        </div>
        
        <!-- Thời Điểm Ghi Nhận Chung (Xuống hàng dưới) -->
        <div class="flex items-center gap-3">
          <label class="font-semibold text-slate-700 whitespace-nowrap">Thời Điểm Ghi Nhận:</label>
          <input 
            v-model="globalThoiDiemGhiNhan" 
            type="datetime-local" 
            @change="$event.target.blur()"
            @dblclick="$event.target.blur()"
            class="w-full sm:w-auto border border-slate-300 rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all outline-none bg-slate-50" 
          />
        </div>
      </div>

      <!-- Main Layout -->
      <div class="flex flex-col gap-6 flex-1 min-h-0" v-if="selectedKhohangId">

        <!-- Top Row: Input Form -->
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200 flex flex-col">
          <div class="p-4 border-b border-slate-100 bg-slate-50 flex justify-between items-center rounded-t-2xl">
            <h2 class="font-bold text-slate-800">{{ isEditing ? 'Cập nhật Dữ liệu' : 'Thêm Dữ liệu mới' }}</h2>
            <button v-if="isEditing" @click="resetForm" class="text-xs bg-slate-200 text-slate-700 px-2 py-1 rounded hover:bg-slate-300">Hủy sửa</button>
          </div>
          
          <div class="p-5 overflow-y-auto custom-scrollbar flex-1">
            <form @submit.prevent="saveItem" @keydown.enter="$event.target.tagName !== 'TEXTAREA' && $event.preventDefault()" class="space-y-4">
              <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Vị Trí <span class="text-red-500">*</span></label>
                  <select v-model="form.viTri" required class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none bg-white">
                    <option value="" disabled>-- Chọn vị trí --</option>
                    <option v-for="local in validLayoutLocals" :key="local.id" :value="local.maLocal">
                      {{ local.maLocal }}
                    </option>
                  </select>
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Dãy Hàng</label>
                  <input v-model="form.dayHang" type="text" disabled class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm bg-slate-100 text-slate-500 cursor-not-allowed outline-none" placeholder="Tự động" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">STT Dãy Hàng</label>
                  <input v-model="form.sttDayHang" type="text" disabled class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm bg-slate-100 text-slate-500 cursor-not-allowed outline-none" placeholder="Tự động" />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="relative">
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Mã Hàng</label>
                  <input :value="form.maHang" 
                         @input="e => { form.maHang = e.target.value; onInput() }"
                         @focus="showDropdown = true" 
                         @blur="handleBlur" 
                         @keydown.down.prevent="onArrowDown"
                         @keydown.up.prevent="onArrowUp"
                         @keydown.enter="onEnter"
                         type="text" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" autocomplete="off" />
                  
                  <!-- Dropdown -->
                  <div v-if="showDropdown && filteredSanPhams.length > 0" ref="dropdownList" class="absolute z-50 w-full mt-1 bg-white border border-slate-200 rounded-lg shadow-lg max-h-48 overflow-y-auto custom-scrollbar">
                    <ul class="py-1 text-sm text-slate-700">
                      <li v-for="(sp, index) in filteredSanPhams" :key="sp.id" @mousedown.prevent="selectProduct(sp)" :class="['px-3 py-2 cursor-pointer flex justify-between items-center transition-colors', selectedIndex === index ? 'bg-blue-100' : 'hover:bg-blue-50']">
                        <span class="font-medium text-blue-700 shrink-0">{{ sp.maSanPham }}</span>
                        <span class="text-xs text-slate-500 truncate ml-2 text-right">{{ sp.tenSanPham }}</span>
                      </li>
                    </ul>
                  </div>
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Tên Sản Phẩm</label>
                  <input v-model="form.tenSanPham" type="text" disabled class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm bg-slate-100 text-slate-500 cursor-not-allowed outline-none" placeholder="Tự động" />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Ngày SX <span class="text-red-500">*</span></label>
                  <input v-model="form.ngaySanXuat" type="date" required class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Hạn Sử Dụng <span class="text-red-500">*</span></label>
                  <input v-model="form.hanSuDung" type="date" required class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-5 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">% HSD Còn Lại</label>
                  <input v-model="form.phanTramHanSuDung" ref="phanTramHsdInput" type="text" placeholder="VD: 78%" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Định Lượng (CS/PL)</label>
                  <input v-model.number="form.dinhLuong" type="number" disabled class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm bg-slate-100 text-slate-500 cursor-not-allowed outline-none" placeholder="Tự động" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1" title="Số Lượng Pallet Chẵn">SL Pallet</label>
                  <input v-model.number="form.soLuongPalletChan" type="number" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Thùng Lẻ</label>
                  <input v-model.number="form.soThungLe" type="number" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Tổng</label>
                  <input v-model.number="form.tong" type="number" readonly class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none bg-blue-50 font-bold cursor-not-allowed" title="Tự động tính = SL Pallet * Định Lượng + Thùng Lẻ" />
                </div>
              </div>

              <div class="grid grid-cols-1 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Trạng Thái D.O.D</label>
                  <input v-model="form.trangThaiDOD" type="text" placeholder="NORMAL-BÌNH THƯỜNG" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" />
                </div>
              </div>

              <div>
                <label class="block text-xs font-semibold text-slate-600 mb-1">Ghi Chú</label>
                <textarea v-model="form.ghiChu" rows="1" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none custom-scrollbar"></textarea>
              </div>

              <div class="pt-4 mt-2 border-t border-slate-100 flex justify-end">
                <button type="submit" class="w-full md:w-auto bg-blue-600 text-white font-bold py-2 px-8 rounded-xl hover:bg-blue-700 transition-colors shadow-md flex items-center justify-center gap-2">
                  <svg v-if="saving" class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
                  <span v-else>{{ isEditing ? 'Lưu Cập Nhật' : 'Thêm Vào Kho' }}</span>
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- Bottom Row: Table Data -->
        <div class="flex-1 bg-white rounded-2xl shadow-sm border border-slate-200 p-0 overflow-hidden flex flex-col">
          <div class="p-4 border-b border-slate-200 bg-slate-50 flex justify-between items-center">
            <h2 class="font-bold text-slate-800">Báo Cáo Tồn Kho</h2>
            <div class="text-sm text-slate-500">Tổng số dòng: <span class="font-bold text-slate-800">{{ tonKhos.length }}</span></div>
          </div>
          
          <div class="flex-1 overflow-auto custom-scrollbar">
            <table class="w-full text-left border-collapse text-sm whitespace-nowrap">
              <thead class="bg-slate-100 sticky top-0 z-10 shadow-sm">
                <tr>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">VỊ TRÍ</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">MÃ HÀNG</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TÊN SẢN PHẨM</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">HẠN SỬ DỤNG</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">% HSD CÒN LẠI</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SL PALLET CHẴN</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">SỐ THÙNG LẺ</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">ĐỊNH LƯỢNG (CS/PL)</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TỔNG</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">THỜI ĐIỂM GHI NHẬN</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">GHI CHÚ</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">TRẠNG THÁI D.O.D</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">NGÀY SẢN XUẤT</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-r border-slate-200">DÃY HÀNG</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-slate-200">STT DÃY HÀNG</th>
                  <th class="py-3 px-4 font-semibold text-slate-700 border-b border-l border-slate-200 bg-slate-50 sticky right-0">Thao tác</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="loadingData" class="bg-white">
                  <td colspan="16" class="py-8 text-center text-slate-500">Đang tải dữ liệu...</td>
                </tr>
                <tr v-else-if="tonKhos.length === 0" class="bg-white">
                  <td colspan="16" class="py-8 text-center text-slate-500">Chưa có dữ liệu tồn kho nào</td>
                </tr>
                <tr 
                  v-for="item in tonKhos" 
                  :key="item.id" 
                  class="border-b border-slate-100 hover:bg-blue-50/50 cursor-pointer transition-colors"
                  :class="{'bg-blue-50': editingId === item.id}"
                  @click="editItem(item)"
                >
                  <td class="py-2 px-4 border-r border-slate-100">{{ item.viTri }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 font-semibold">{{ item.maHang }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 min-w-[200px] truncate max-w-xs" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                  <td class="py-2 px-4 border-r border-slate-100">{{ formatDate(item.hanSuDung) }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 font-medium" :class="item.phanTramHanSuDung?.includes('%') ? 'text-blue-600' : ''">{{ item.phanTramHanSuDung }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soLuongPalletChan }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.soThungLe }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-right">{{ item.dinhLuong }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-right font-bold">{{ item.tong }}</td>
                  <td class="py-2 px-4 border-r border-slate-100">{{ formatDateTime(item.thoiDiemGhiNhan) }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-slate-500 max-w-[150px] truncate">{{ item.ghiChu }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 text-xs">{{ item.trangThaiDOD }}</td>
                  <td class="py-2 px-4 border-r border-slate-100">{{ formatDate(item.ngaySanXuat) }}</td>
                  <td class="py-2 px-4 border-r border-slate-100">{{ item.dayHang }}</td>
                  <td class="py-2 px-4">{{ item.sttDayHang }}</td>
                  <td class="py-2 px-4 border-l border-slate-100 bg-white sticky right-0 shadow-[-4px_0_6px_-1px_rgba(0,0,0,0.05)]">
                    <button @click.stop="deleteItem(item.id)" class="text-red-500 hover:text-red-700 bg-red-50 hover:bg-red-100 p-1.5 rounded-lg transition-colors" title="Xóa">
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

      </div>

      <!-- Placeholder when no warehouse selected -->
      <div v-else class="flex-1 flex flex-col items-center justify-center bg-white rounded-2xl shadow-sm border border-slate-200 text-slate-400 p-12">
        <svg class="w-20 h-20 mb-4 text-slate-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
        <p class="text-lg font-medium">Vui lòng chọn một kho hàng phía trên để xem và cấu hình Tồn kho đầu kỳ.</p>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import * as XLSX from 'xlsx'

definePageMeta({
  layout: false
})

const apiBase = 'http://localhost:5121/api'

const khohangs = ref([])
const selectedKhohangId = ref('')
const globalThoiDiemGhiNhan = ref('')
const tonKhos = ref([])
const layoutLocals = ref([])
const sanPhams = ref([])

const validLayoutLocals = computed(() => {
  return layoutLocals.value.filter(item => 
    (!item.elementType && item.maLocal && !item.maLocal.startsWith('LINE_')) || item.elementType === 'local'
  )
})

const showDropdown = ref(false)
const selectedIndex = ref(-1)
const dropdownList = ref(null)
const phanTramHsdInput = ref(null)
const fileInput = ref(null)

const triggerUpload = () => {
  if (!selectedKhohangId.value || !globalThoiDiemGhiNhan.value) {
    alert('Vui lòng chọn Kho và Thời Điểm Ghi Nhận trước khi upload Excel!')
    return
  }
  if (fileInput.value) {
    fileInput.value.click()
  }
}

const exportExcel = () => {
  const dataToExport = tonKhos.value.length > 0 ? tonKhos.value.map(item => ({
    'Vị Trí': item.viTri,
    'Mã Hàng': item.maHang,
    'Ngày SX': item.ngaySanXuat ? item.ngaySanXuat.split('T')[0] : '',
    'Hạn Sử Dụng': item.hanSuDung ? item.hanSuDung.split('T')[0] : '',
    '% HSD Còn Lại': item.phanTramHanSuDung,
    'SL Pallet': item.soLuongPalletChan,
    'Thùng Lẻ': item.soThungLe,
    'Trạng Thái D.O.D': item.trangThaiDOD,
    'Ghi Chú': item.ghiChu
  })) : [{
    'Vị Trí': '',
    'Mã Hàng': '',
    'Ngày SX': '',
    'Hạn Sử Dụng': '',
    '% HSD Còn Lại': '',
    'SL Pallet': '',
    'Thùng Lẻ': '',
    'Trạng Thái D.O.D': '',
    'Ghi Chú': ''
  }]

  const ws = XLSX.utils.json_to_sheet(dataToExport)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, "TonKhoDauKy")
  XLSX.writeFile(wb, `TonKhoDauKy_Template.xlsx`)
}

const handleFileUpload = async (event) => {
  const file = event.target.files[0]
  if (!file) return

  const reader = new FileReader()
  reader.onload = async (e) => {
    try {
      const data = new Uint8Array(e.target.result)
      const workbook = XLSX.read(data, { type: 'array' })
      const firstSheetName = workbook.SheetNames[0]
      const worksheet = workbook.Sheets[firstSheetName]
      const jsonData = XLSX.utils.sheet_to_json(worksheet, { raw: false }) 

      let successCount = 0
      let errorCount = 0
      saving.value = true

      for (const row of jsonData) {
        const viTri = String(row['Vị Trí'] || '').trim()
        const maHang = String(row['Mã Hàng'] || '').trim()
        
        let ngaySX = row['Ngày SX'] || ''
        if (ngaySX && !ngaySX.includes('-')) {
          // Xử lý format ngày nếu Excel convert sang MM/DD/YYYY
          const d = new Date(ngaySX)
          if (!isNaN(d)) ngaySX = d.toISOString().split('T')[0]
        }
        
        let hanSD = row['Hạn Sử Dụng'] || ''
        if (hanSD && !hanSD.includes('-')) {
          const d = new Date(hanSD)
          if (!isNaN(d)) hanSD = d.toISOString().split('T')[0]
        }
        
        if (!viTri || !maHang || !ngaySX || !hanSD) {
          continue 
        }

        const phanTramHSD = String(row['% HSD Còn Lại'] || '')
        const slPallet = parseInt(row['SL Pallet']) || 0
        const thungLe = parseInt(row['Thùng Lẻ']) || 0
        const dod = String(row['Trạng Thái D.O.D'] || '')
        const ghiChu = String(row['Ghi Chú'] || '')

        const sp = sanPhams.value.find(s => s.maSanPham.toLowerCase() === maHang.toLowerCase())
        const tenSanPham = sp ? sp.tenSanPham : ''
        const dinhLuong = sp ? sp.dinhLuong : 0

        let payload = {
          khohangId: selectedKhohangId.value,
          thoiDiemGhiNhan: globalThoiDiemGhiNhan.value,
          viTri: viTri,
          maHang: maHang,
          tenSanPham: tenSanPham,
          ngaySanXuat: ngaySX,
          hanSuDung: hanSD,
          phanTramHanSuDung: phanTramHSD,
          dinhLuong: dinhLuong,
          soLuongPalletChan: slPallet,
          soThungLe: thungLe,
          trangThaiDOD: dod,
          ghiChu: ghiChu
        }

        try {
          await $fetch(apiBase + '/tonkhodauky', {
            method: 'POST',
            body: payload
          })
          successCount++
        } catch (err) {
          errorCount++
        }
      }

      alert(`Upload hoàn tất!\nThành công: ${successCount} dòng.\nLỗi/Bỏ qua: ${errorCount} dòng.`)
      fetchTonKhoData()
    } catch (error) {
      console.error(error)
      alert("Lỗi khi đọc file Excel!")
    } finally {
      saving.value = false
      event.target.value = '' 
    }
  }
  reader.readAsArrayBuffer(file)
}

const filteredSanPhams = computed(() => {
  if (!form.value.maHang) return sanPhams.value.slice(0, 50)
  const query = String(form.value.maHang).trim().toLowerCase()
  
  const queryTokens = query.split(/\s+/)
  const isNumeric = /^\d+$/.test(query.replace(/\s+/g, ''))
  
  return sanPhams.value.filter(s => {
    if (isNumeric) {
      const ma = String(s.maSanPham || '').toLowerCase()
      return queryTokens.every(token => ma.includes(token))
    } else {
      const ten = String(s.tenSanPham || '').toLowerCase()
      return queryTokens.every(token => ten.includes(token))
    }
  }).slice(0, 50)
})

const selectProduct = (sp) => {
  form.value.maHang = sp.maSanPham
  showDropdown.value = false
  setTimeout(() => {
    if (phanTramHsdInput.value) {
      phanTramHsdInput.value.focus()
    }
  }, 50)
}

const handleBlur = () => {
  showDropdown.value = false
}

const onInput = () => {
  showDropdown.value = true
  selectedIndex.value = -1
}

const onArrowDown = () => {
  if (!showDropdown.value) {
    showDropdown.value = true
    return
  }
  if (selectedIndex.value < filteredSanPhams.value.length - 1) {
    selectedIndex.value++
    scrollToItem()
  }
}

const onArrowUp = () => {
  if (selectedIndex.value > 0) {
    selectedIndex.value--
    scrollToItem()
  }
}

const onEnter = (e) => {
  e.preventDefault() // Luôn chặn submit form khi bấm Enter ở ô Mã Hàng
  if (showDropdown.value && selectedIndex.value >= 0 && selectedIndex.value < filteredSanPhams.value.length) {
    selectProduct(filteredSanPhams.value[selectedIndex.value])
  } else {
    // Nếu gõ xong và bấm Enter (không chọn từ dropdown), tự động nhảy focus
    showDropdown.value = false
    if (phanTramHsdInput.value) {
      phanTramHsdInput.value.focus()
    }
  }
}

const scrollToItem = () => {
  setTimeout(() => {
    if (dropdownList.value) {
      const activeItem = dropdownList.value.querySelector('.bg-blue-100')
      if (activeItem) {
        activeItem.scrollIntoView({ block: 'nearest', behavior: 'smooth' })
      }
    }
  }, 50)
}

const loadingData = ref(false)
const saving = ref(false)

const isEditing = ref(false)
const editingId = ref(null)

const defaultForm = {
  viTri: '',
  maHang: '',
  tenSanPham: '',
  hanSuDung: '',
  phanTramHanSuDung: '',
  soLuongPalletChan: null,
  soThungLe: null,
  dinhLuong: null,
  tong: null,
  ghiChu: '',
  trangThaiDOD: 'NORMAL-BÌNH THƯỜNG',
  ngaySanXuat: '',
  dayHang: '',
  sttDayHang: ''
}

const form = ref({ ...defaultForm })

// Tự động tính Tổng = (SL Pallet * Định Lượng) + Thùng Lẻ
watch([() => form.value.soLuongPalletChan, () => form.value.dinhLuong, () => form.value.soThungLe], ([slPallet, dinhLuong, thungLe]) => {
  const sl = Number(slPallet) || 0
  const dl = Number(dinhLuong) || 0
  const tl = Number(thungLe) || 0
  if (sl === 0 && dl === 0 && tl === 0 && form.value.tong === null) {
    // avoid overriding with 0 if user just reset form
    return
  }
  form.value.tong = (sl * dl) + tl
}, { deep: true })

// Tự động lấy Dãy Hàng và STT Dãy Hàng khi chọn Vị Trí
watch(() => form.value.viTri, (newViTri) => {
  if (newViTri) {
    const layoutItem = validLayoutLocals.value.find(l => l.maLocal === newViTri)
    form.value.dayHang = layoutItem ? layoutItem.hang : ''
    form.value.sttDayHang = layoutItem ? layoutItem.cot : ''
  }
})

// Tự động lấy Tên Sản Phẩm và Định Lượng khi nhập Mã Hàng
watch(() => form.value.maHang, (newMaHang) => {
  if (newMaHang) {
    const sp = sanPhams.value.find(s => s.maSanPham.toLowerCase() === String(newMaHang).trim().toLowerCase())
    if (sp) {
      form.value.tenSanPham = sp.tenSanPham
      form.value.dinhLuong = sp.dinhLuong
    } else {
      form.value.tenSanPham = ''
      form.value.dinhLuong = null
    }
  } else {
    form.value.tenSanPham = ''
    form.value.dinhLuong = null
  }
})

onMounted(async () => {
  await fetchKhoHangs()
})

const fetchKhoHangs = async () => {
  try {
    const res = await $fetch(`${apiBase}/Khohangs`)
    khohangs.value = res
  } catch (err) {
    console.error('Error fetching khohangs:', err)
  }
}

const fetchTonKhoData = async () => {
  if (!selectedKhohangId.value) return
  loadingData.value = true
  try {
    const [tonKhoRes, layoutRes, sanPhamRes] = await Promise.all([
      $fetch(`${apiBase}/TonKhoDauKies/ByKhohang/${selectedKhohangId.value}`),
      $fetch(`${apiBase}/Layoutkhos?khohangId=${selectedKhohangId.value}`),
      $fetch(`${apiBase}/Sanphamnuocs/ByKhohang/${selectedKhohangId.value}`)
    ])
    
    tonKhos.value = tonKhoRes || []
    layoutLocals.value = layoutRes || []
    sanPhams.value = sanPhamRes || []
    resetForm()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải dữ liệu tồn kho hoặc layout')
  } finally {
    loadingData.value = false
  }
}

// Convert backend Date string to YYYY-MM-DD for input[type=date]
const toInputDate = (dateStr) => {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return d.toISOString().split('T')[0]
}

// Format date for display in table (MM/DD/YYYY or DD/MM/YYYY)
const formatDate = (dateStr) => {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return d.toLocaleDateString('en-US', { year: '2-digit', month: 'numeric', day: 'numeric' })
}

// Format datetime for display in table
const formatDateTime = (dateStr) => {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return d.toLocaleString('en-US', { year: '2-digit', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' })
}

// Convert backend Date string to YYYY-MM-DDTHH:mm for input[type=datetime-local]
const toInputDateTime = (dateStr) => {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  // Need to adjust for timezone offset to get local time string in correct format
  const offset = d.getTimezoneOffset() * 60000;
  const localISOTime = (new Date(d - offset)).toISOString().slice(0, 16);
  return localISOTime;
}

const editItem = (item) => {
  isEditing.value = true
  editingId.value = item.id
  form.value = {
    ...item,
    ngaySanXuat: toInputDate(item.ngaySanXuat),
    hanSuDung: toInputDate(item.hanSuDung)
  }
  // When editing, also load the item's datetime into the global selector to be clear
  if (item.thoiDiemGhiNhan) {
    globalThoiDiemGhiNhan.value = toInputDateTime(item.thoiDiemGhiNhan)
  }
}

const resetForm = () => {
  isEditing.value = false
  editingId.value = null
  form.value = { ...defaultForm }
}

const saveItem = async () => {
  if (!selectedKhohangId.value) {
    alert('Vui lòng chọn kho hàng trước!')
    return
  }

  saving.value = true
  
  const payload = { 
    ...form.value, 
    khohangId: selectedKhohangId.value,
    thoiDiemGhiNhan: globalThoiDiemGhiNhan.value || null
  }
  
  // Clean empty strings for numbers
  if (payload.soLuongPalletChan === '') payload.soLuongPalletChan = null
  if (payload.soThungLe === '') payload.soThungLe = null
  if (payload.dinhLuong === '') payload.dinhLuong = null
  if (payload.tong === '') payload.tong = null

  // Convert empty dates to null
  if (!payload.ngaySanXuat) payload.ngaySanXuat = null
  if (!payload.hanSuDung) payload.hanSuDung = null
  if (!payload.thoiDiemGhiNhan) payload.thoiDiemGhiNhan = null

  try {
    if (isEditing.value && editingId.value) {
      payload.id = editingId.value
      await $fetch(`${apiBase}/TonKhoDauKies/${editingId.value}`, {
        method: 'PUT',
        body: payload
      })
    } else {
      await $fetch(`${apiBase}/TonKhoDauKies`, {
        method: 'POST',
        body: payload
      })
    }
    await fetchTonKhoData()
  } catch (err) {
    console.error(err)
    alert('Có lỗi xảy ra khi lưu dữ liệu')
  } finally {
    saving.value = false
  }
}

const deleteItem = async (id) => {
  if (!confirm('Bạn có chắc muốn xóa dòng tồn kho này?')) return
  try {
    await $fetch(`${apiBase}/TonKhoDauKies/${id}`, { method: 'DELETE' })
    await fetchTonKhoData()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi xóa')
  }
}
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: transparent;
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background-color: #cbd5e1;
  border-radius: 20px;
}
.custom-scrollbar::-webkit-scrollbar-corner {
  background: transparent;
}
</style>

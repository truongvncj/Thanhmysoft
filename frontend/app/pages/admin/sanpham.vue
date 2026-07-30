<template>
  <div class="h-screen bg-slate-50 flex flex-col font-sans overflow-hidden">
    
    <div class="bg-indigo-600 text-white px-6 py-4 flex items-center gap-4 shadow-md z-10 shrink-0">
      <NuxtLink to="/admin" class="hover:bg-white/20 p-2 rounded-full transition-colors">
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
      </NuxtLink>
      <div>
        <h1 class="text-xl font-bold tracking-wide">Quản lý Sản Phẩm</h1>
        <p class="text-indigo-200 text-sm">Danh mục sản phẩm theo từng kho hàng</p>
      </div>
    </div>

    <div class="flex-1 overflow-y-auto p-4 md:p-6 flex flex-col gap-6 relative">
      
      <!-- Top Action Bar -->
      <div class="bg-white p-4 rounded-2xl shadow-sm border border-slate-200 flex flex-col sm:flex-row gap-4 justify-between items-center z-20">
        <div class="flex flex-col sm:flex-row items-center gap-4 sm:gap-6 w-full">
          <!-- Chọn Kho -->
          <div class="flex items-center gap-3 flex-1 sm:flex-none">
            <label class="font-semibold text-slate-700 whitespace-nowrap">Chọn Kho:</label>
            <select 
              v-model="selectedKhohangId" 
              @change="fetchSanphamData"
              class="w-full sm:w-64 border border-slate-300 rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all outline-none bg-slate-50"
            >
              <option value="" disabled>-- Chọn kho hàng --</option>
              <option v-for="kho in khohangs" :key="kho.id" :value="kho.id">
                {{ kho.tenKho }}
              </option>
            </select>
          </div>


        </div>
      </div>

            <div v-if="selectedKhohangId" class="flex items-center gap-4 border-b border-slate-200 mt-2 px-2">
        <button 
          @click="activeTab = 'Thành phẩm'; resetForm()" 
          :class="['px-4 py-2 font-medium transition-colors border-b-2', activeTab === 'Thành phẩm' ? 'border-blue-600 text-blue-600' : 'border-transparent text-slate-500 hover:text-slate-700']"
        >
          Thành phẩm
        </button>
        <button 
          @click="activeTab = 'Vỏ'; resetForm()" 
          :class="['px-4 py-2 font-medium transition-colors border-b-2', activeTab === 'Vỏ' ? 'border-blue-600 text-blue-600' : 'border-transparent text-slate-500 hover:text-slate-700']"
        >
          Vỏ
        </button>
      </div>

      <div v-if="selectedKhohangId" class="flex flex-col gap-6">
        
        <!-- Top Row: Form -->
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden flex flex-col relative">
          <div v-if="saving" class="absolute inset-0 bg-white/50 backdrop-blur-sm z-10 flex items-center justify-center">
            <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
          </div>
          <div class="bg-slate-50 border-b border-slate-100 px-5 py-3 flex justify-between items-center flex-wrap gap-2">
            <div class="flex items-center gap-3">
              <h3 class="font-bold text-slate-700">{{ isEditing ? 'Cập nhật Sản Phẩm' : 'Thêm Sản Phẩm Mới' }}</h3>
              <button v-if="isEditing" @click="resetForm" class="text-xs bg-slate-200 text-slate-700 px-2 py-1 rounded hover:bg-slate-300">Hủy sửa</button>
            </div>
            <div class="flex items-center gap-2">
              <button @click="exportExcel" class="flex items-center justify-center gap-1.5 bg-emerald-50 text-emerald-700 hover:bg-emerald-100 border border-emerald-200 px-3 py-1.5 rounded-full text-sm font-medium transition-colors">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                Kết xuất Excel
              </button>
              <button @click="triggerUpload" class="flex items-center justify-center gap-1.5 bg-blue-50 text-blue-700 hover:bg-blue-100 border border-blue-200 px-3 py-1.5 rounded-full text-sm font-medium transition-colors relative overflow-hidden">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path></svg>
                Upload Excel
              </button>
              <input type="file" ref="fileInput" @change="handleFileUpload" accept=".xlsx, .xls" class="hidden" />
            </div>
          </div>
          
          <div class="p-5 overflow-y-auto custom-scrollbar flex-1">
            <form @submit.prevent="saveItem" class="space-y-4">
              
              <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Mã Sản Phẩm <span class="text-red-500">*</span></label>
                  <input v-model="form.maSanPham" required type="text" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Nhập mã SP..." />
                </div>
                <div class="md:col-span-2">
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Tên Sản Phẩm <span class="text-red-500">*</span></label>
                  <input v-model="form.tenSanPham" required type="text" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Nhập tên SP..." />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Định Lượng (CS/PL)</label>
                  <input v-model.number="form.dinhLuong" type="number" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Nhập số lượng..." />
                </div>
              </div>
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4" v-if="activeTab === 'Thành phẩm'">
                <div class="relative">
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Mã Vỏ</label>
                  <input v-model="form.maVo" @input="onMaVoInput" @focus="showVoDropdown = true" @blur="handleVoBlur" type="text" class="w-full border border-slate-300 rounded-lg px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Nhập hoặc chọn mã vỏ..." />
                  <!-- Dropdown -->
                  <ul v-if="showVoDropdown && filteredVoList.length > 0" class="absolute z-50 mt-1 w-full bg-white border border-slate-200 shadow-lg max-h-60 rounded-md overflow-auto custom-scrollbar">
                    <li v-for="vo in filteredVoList" :key="vo.id" @mousedown.prevent="selectVo(vo)" class="px-4 py-2 hover:bg-blue-50 cursor-pointer text-sm">
                      <span class="font-semibold text-blue-600">{{ vo.maSanPham }}</span> - <span class="text-slate-600">{{ vo.tenSanPham }}</span>
                    </li>
                  </ul>
                </div>
                <div>
                  <label class="block text-xs font-semibold text-slate-600 mb-1">Tên Vỏ</label>
                  <input v-model="form.tenVo" readonly type="text" class="w-full border border-slate-200 bg-slate-100 rounded-lg px-3 py-2 text-sm outline-none text-slate-500" placeholder="Tên vỏ tự động điền..." />
                </div>
              </div>

              <div class="pt-4 mt-2 border-t border-slate-100 flex justify-end">
                <button type="submit" class="w-full md:w-auto bg-blue-600 text-white font-bold py-2 px-8 rounded-xl hover:bg-blue-700 transition-colors shadow-md flex items-center justify-center gap-2">
                  <svg v-if="saving" class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
                  <span v-else>{{ isEditing ? 'Lưu Cập Nhật' : 'Thêm Sản Phẩm' }}</span>
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- Bottom Row: Table Data -->
        <div class="flex-1 bg-white rounded-2xl shadow-sm border border-slate-200 p-0 overflow-hidden flex flex-col">
          <div class="bg-slate-50 border-b border-slate-100 px-5 py-3 flex justify-between items-center">
            <h3 class="font-bold text-slate-700">Danh sách Sản Phẩm</h3>
            <span class="text-xs bg-blue-100 text-blue-700 font-bold px-2 py-1 rounded-md">Tổng số: {{ filteredDisplaySanPhams.length }}</span>
          </div>
          
          <div class="flex-1 overflow-auto custom-scrollbar relative">
            <div v-if="loadingData" class="absolute inset-0 bg-white/50 backdrop-blur-sm z-10 flex flex-col items-center justify-center">
              <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mb-2"></div>
              <p class="text-sm text-slate-500 font-medium">Đang tải dữ liệu...</p>
            </div>

            <table class="w-full text-sm text-left whitespace-nowrap min-w-[600px]">
              <thead class="text-xs text-slate-500 uppercase bg-slate-50 sticky top-0 z-10 shadow-sm">
                <tr>
                  <th class="py-3 px-4 border-r border-slate-100 font-bold">Mã Sản Phẩm</th>
                  <th class="py-3 px-4 border-r border-slate-100 font-bold">Tên Sản Phẩm</th>
                  <th class="py-3 px-4 border-r border-slate-100 font-bold">Định Lượng (CS/PL)</th>
                  <th class="py-3 px-4 border-r border-slate-100 font-bold" v-if="activeTab === 'Thành phẩm'">Mã Vỏ</th>
                  <th class="py-3 px-4 border-r border-slate-100 font-bold" v-if="activeTab === 'Thành phẩm'">Tên Vỏ</th>
                  <th class="py-3 px-4 border-l border-slate-100 bg-slate-50 sticky right-0 shadow-[-4px_0_6px_-1px_rgba(0,0,0,0.05)] w-20 text-center font-bold">Thao tác</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="sanPhams.length === 0 && !loadingData">
                  <td colspan="4" class="py-8 text-center text-slate-400">
                    Chưa có sản phẩm nào.
                  </td>
                </tr>
                <tr 
                  v-for="(item, index) in filteredDisplaySanPhams" 
                  :key="item.id" 
                  class="border-b border-slate-100 hover:bg-blue-50/50 transition-colors cursor-pointer"
                  :class="{'bg-blue-50/30': editingId === item.id}"
                  @click="editItem(item)"
                >
                  <td class="py-2 px-4 border-r border-slate-100 font-medium text-slate-700">{{ item.maSanPham }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 truncate max-w-[300px]" :title="item.tenSanPham">{{ item.tenSanPham }}</td>
                  <td class="py-2 px-4 border-r border-slate-100">{{ item.dinhLuong }}</td>
                  <td class="py-2 px-4 border-r border-slate-100" v-if="activeTab === 'Thành phẩm'">{{ item.maVo }}</td>
                  <td class="py-2 px-4 border-r border-slate-100 truncate max-w-[200px]" :title="item.tenVo" v-if="activeTab === 'Thành phẩm'">{{ item.tenVo }}</td>
                  <td class="py-2 px-4 border-l border-slate-100 bg-white sticky right-0 shadow-[-4px_0_6px_-1px_rgba(0,0,0,0.05)] text-center">
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
        <svg class="w-20 h-20 mb-4 text-slate-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"></path></svg>
        <p class="text-lg font-medium">Vui lòng chọn một kho hàng phía trên để quản lý Sản phẩm.</p>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import * as XLSX from 'xlsx'

definePageMeta({
  layout: false
})

const config = useRuntimeConfig();
const apiBase = config.public.apiBaseUrl

const khohangs = ref([])
const selectedKhohangId = ref('')
const sanPhams = ref([])

const loadingData = ref(false)
const saving = ref(false)


const activeTab = ref('Thành phẩm')
const showVoDropdown = ref(false)

const filteredDisplaySanPhams = computed(() => {
  return sanPhams.value.filter(s => s.loaiSanPham === activeTab.value)
})

const filteredVoList = computed(() => {
  const voList = sanPhams.value.filter(s => s.loaiSanPham === 'Vỏ')
  if (!form.value.maVo) return voList.slice(0, 50)
  const query = String(form.value.maVo).toLowerCase().trim()
  return voList.filter(v => v.maSanPham.toLowerCase().includes(query) || v.tenSanPham.toLowerCase().includes(query)).slice(0, 50)
})

const onMaVoInput = () => {
  showVoDropdown.value = true
  const query = String(form.value.maVo).trim().toLowerCase()
  const matchedVo = sanPhams.value.find(s => s.loaiSanPham === 'Vỏ' && s.maSanPham.toLowerCase() === query)
  if (matchedVo) {
    form.value.tenVo = matchedVo.tenSanPham
  } else {
    form.value.tenVo = ''
  }
}

const selectVo = (vo) => {
  form.value.maVo = vo.maSanPham
  form.value.tenVo = vo.tenSanPham
  showVoDropdown.value = false
}

const handleVoBlur = () => {
  setTimeout(() => {
    showVoDropdown.value = false
  }, 200)
}

const isEditing = ref(false)
const editingId = ref(null)

const defaultForm = {
  maSanPham: '',
  tenSanPham: '',
  dinhLuong: null,
  loaiSanPham: 'Thành phẩm',
  maVo: '',
  tenVo: ''
}

const form = ref({ ...defaultForm })
const fileInput = ref(null)

onMounted(async () => {
  await fetchKhoHangs()
})

const fetchKhoHangs = async () => {
  try {
    const res = await $fetch(`${apiBase}/Khohangs`)
    khohangs.value = res || []
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải danh sách kho hàng')
  }
}

const fetchSanphamData = async () => {
  if (!selectedKhohangId.value) return
  loadingData.value = true
  try {
    const res = await $fetch(`${apiBase}/Sanphams/ByKhohang/${selectedKhohangId.value}`)
    sanPhams.value = res || []
    resetForm()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải danh sách sản phẩm')
  } finally {
    loadingData.value = false
  }
}

const resetForm = async () => {
  form.value = { ...defaultForm, loaiSanPham: activeTab.value }
  isEditing.value = false
  editingId.value = null
}

const editItem = async (item) => {
  isEditing.value = true
  editingId.value = item.id
  form.value = { ...item }
}

const saveItem = async () => {
  if (!selectedKhohangId.value) {
    alert('Vui lòng chọn kho hàng trước!')
    return
  }

  saving.value = true
  
  const payload = {
    id: isEditing.value ? editingId.value : undefined,
    maSanPham: form.value.maSanPham,
    tenSanPham: form.value.tenSanPham,
    dinhLuong: form.value.dinhLuong,
    khohangId: selectedKhohangId.value,
    loaiSanPham: form.value.loaiSanPham,
    maVo: form.value.maVo,
    tenVo: form.value.tenVo
  }
  
  if (payload.dinhLuong === '') payload.dinhLuong = null

  try {
    if (isEditing.value) {
      await $fetch(`${apiBase}/Sanphams/${editingId.value}`, {
        method: 'PUT',
        body: payload
      })
    } else {
      await $fetch(`${apiBase}/Sanphams`, {
        method: 'POST',
        body: payload
      })
    }
    await fetchSanphamData()
  } catch (err) {
    console.error(err)
    if (err.data && err.data.message) {
      alert(err.data.message)
    } else {
      alert('Lỗi khi lưu dữ liệu!')
    }
  } finally {
    saving.value = false
  }
}

const deleteItem = async (id) => {
  if (!await confirm('Bạn có chắc muốn xóa sản phẩm này?')) return
  try {
    await $fetch(`${apiBase}/Sanphams/${id}`, {
      method: 'DELETE'
    })
    await fetchSanphamData()
  } catch (err) {
    console.error(err)
    alert('Lỗi khi xóa dữ liệu!')
  }
}

const exportExcel = async () => {
  if (!selectedKhohangId.value) {
    alert('Vui lòng chọn kho hàng trước!')
    return
  }
  
    const dataToExport = filteredDisplaySanPhams.value.map((item, index) => {
    const row = {
      'STT': index + 1,
      'Mã Sản Phẩm': item.maSanPham,
      'Tên Sản Phẩm': item.tenSanPham,
      'Định Lượng (CS/PL)': item.dinhLuong,
      'Loại Sản Phẩm': item.loaiSanPham
    };
    if (activeTab.value === 'Thành phẩm') {
      row['Mã Vỏ'] = item.maVo || '';
      row['Tên Vỏ'] = item.tenVo || '';
    }
    return row;
  })
  
  if (dataToExport.length === 0) {
        dataToExport.push({
      'STT': '',
      'Mã Sản Phẩm': '',
      'Tên Sản Phẩm': '',
      'Định Lượng (CS/PL)': '',
      'Loại Sản Phẩm': activeTab.value,
      'Mã Vỏ': '',
      'Tên Vỏ': ''
    })
  }

  const ws = XLSX.utils.json_to_sheet(dataToExport)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, 'DanhSachSanPham')
  
  const khoName = khohangs.value.find(k => k.id === selectedKhohangId.value)?.tenKho || 'Kho'
  XLSX.writeFile(wb, `DanhSachSanPham_${khoName}.xlsx`)
}

const triggerUpload = async () => {
  if (!selectedKhohangId.value) {
    alert('Vui lòng chọn kho hàng trước!')
    return
  }
  fileInput.value.click()
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
      const jsonData = XLSX.utils.sheet_to_json(worksheet)

      let successCount = 0
      let skipCount = 0
      let errorCount = 0
      saving.value = true

      for (const row of jsonData) {
        const rawMaSanPham = row['Mã Sản Phẩm']
        const rawTenSanPham = row['Tên Sản Phẩm']
        let dinhLuong = row['Định Lượng (CS/PL)']

        if (!rawMaSanPham || !rawTenSanPham) continue // Bỏ qua nếu thiếu dữ liệu bắt buộc

        const maSanPham = String(rawMaSanPham).trim()
        const tenSanPham = String(rawTenSanPham).trim()

        if (maSanPham === '' || tenSanPham === '') continue

        // Kiểm tra xem mã SP đã có trong sanPhams (dữ liệu hiện tại) chưa
        const exists = sanPhams.value.some(s => s.maSanPham.trim().toLowerCase() === maSanPham.toLowerCase())
        if (exists) {
          skipCount++
          continue
        }

        if (dinhLuong === '' || dinhLuong === undefined || dinhLuong === null) {
          dinhLuong = null
        } else {
          dinhLuong = Number(dinhLuong)
          if (isNaN(dinhLuong)) dinhLuong = null
        }

        let loaiSanPham = activeTab.value;
        let maVo = row['Mã Vỏ'] || '';
        let tenVo = row['Tên Vỏ'] || '';

        const payload = {
          maSanPham: maSanPham,
          tenSanPham: tenSanPham,
          dinhLuong: dinhLuong,
          khohangId: selectedKhohangId.value,
          loaiSanPham: loaiSanPham,
          maVo: maVo,
          tenVo: tenVo
        }

        try {
          await $fetch(`${apiBase}/Sanphams`, {
            method: 'POST',
            body: payload
          })
          successCount++
        } catch (postErr) {
          console.error('Lỗi khi thêm sản phẩm:', payload.maSanPham, postErr)
          errorCount++
        }
      }

      await fetchSanphamData()
      alert(`Upload hoàn tất!\n- Thành công (Mới): ${successCount} sản phẩm.\n- Bỏ qua (Trùng lặp đã có sẵn): ${skipCount} sản phẩm.\n- Lỗi thêm: ${errorCount} sản phẩm.`)
    } catch (err) {
      console.error(err)
      alert('Lỗi khi đọc file Excel!')
    } finally {
      saving.value = false
      event.target.value = '' // Reset input
    }
  }
  reader.readAsArrayBuffer(file)
}
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: #f1f5f9;
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}
</style>

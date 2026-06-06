<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto">
      <!-- Header -->
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h1 class="text-2xl font-bold text-slate-800">Quản lý Lái xe (Thẻ nhà thầu)</h1>
          <p class="text-slate-500 mt-1">Danh sách tất cả lái xe do các nhà vận tải đăng ký</p>
        </div>
        <NuxtLink to="/admin" class="flex items-center gap-2 text-blue-600 bg-blue-50 px-4 py-2 rounded-xl font-semibold hover:bg-blue-100 transition-colors">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
          </svg>
          Quay lại Admin
        </NuxtLink>
      </div>

      <!-- Main Content -->
      <div class="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
        <div class="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50/50">
          <div class="relative w-full max-w-md">
            <input v-model="searchQuery" type="text" placeholder="Tìm kiếm theo tên, số thẻ, SĐT..." class="w-full pl-10 pr-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white shadow-sm text-sm">
            <svg class="w-5 h-5 text-slate-400 absolute left-3 top-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
          </div>
        </div>

        <div v-if="loading" class="p-12 flex flex-col items-center justify-center">
          <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mb-4"></div>
          <p class="text-slate-500">Đang tải dữ liệu...</p>
        </div>

        <div v-else-if="filteredDrivers.length === 0" class="p-12 text-center text-slate-500">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-slate-300 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm14 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z" />
          </svg>
          Không tìm thấy thẻ lái xe nào.
        </div>

        <div v-else>
          <!-- Mobile View: Cards -->
          <div class="grid grid-cols-1 gap-4 p-4 lg:hidden">
            <div v-for="driver in filteredDrivers" :key="'mob-'+driver.maLx" class="bg-white rounded-xl shadow-sm border border-slate-200 p-4 flex flex-col gap-3 relative">
              <div class="flex justify-between items-start">
                <div>
                  <h3 class="font-bold text-lg text-slate-800">{{ driver.ten }}</h3>
                  <p class="text-sm font-medium text-slate-500">{{ driver.tenNvt }}</p>
                </div>
                <div class="flex gap-2">
                  <button @click="openEditModal(driver)" class="text-blue-600 bg-blue-50 p-2 rounded-lg hover:bg-blue-100 border border-blue-100">Sửa</button>
                </div>
              </div>
              <div class="space-y-2 mt-2 text-sm">
                <div class="flex justify-between items-center">
                  <span class="text-slate-500">Số thẻ:</span>
                  <span class="inline-block px-3 py-1 bg-slate-100 text-slate-800 rounded-lg font-mono text-sm shadow-sm border border-slate-200">
                    {{ driver.sothe || 'Chưa cấp' }}
                  </span>
                </div>
                <div class="flex justify-between items-center">
                  <span class="text-slate-500">Hạn thẻ:</span>
                  <div class="flex items-center gap-2">
                    <span :class="isExpired(driver.thoihanthe) ? 'text-red-600 font-semibold' : 'text-slate-700'">
                      {{ formatDate(driver.thoihanthe) }}
                    </span>
                    <button @click="openExtendModal(driver)" class="text-blue-500 hover:text-blue-700 bg-blue-50 p-1 rounded-md transition-colors" title="Gia hạn">
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path></svg>
                    </button>
                  </div>
                </div>
                <div class="flex justify-between items-center">
                  <span class="text-slate-500">Trạng thái:</span>
                  <span>
                    <span v-if="driver.trangThai === 0" class="inline-flex items-center gap-1.5 px-3 py-1 bg-amber-100 text-amber-700 rounded-full text-xs font-bold border border-amber-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-amber-500"></span> Chờ duyệt
                    </span>
                    <span v-else-if="driver.trangThai === 1" class="inline-flex items-center gap-1.5 px-3 py-1 bg-emerald-100 text-emerald-700 rounded-full text-xs font-bold border border-emerald-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-emerald-500"></span> Hoạt động
                    </span>
                    <span v-else class="inline-flex items-center gap-1.5 px-3 py-1 bg-red-100 text-red-700 rounded-full text-xs font-bold border border-red-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-red-500"></span> Đã khoá
                    </span>
                  </span>
                </div>
              </div>
              <div class="pt-3 border-t border-slate-100 mt-2 flex justify-end gap-2" v-if="driver.trangThai !== 1 || driver.trangThai === 1">
                <button v-if="driver.trangThai !== 1" @click="changeStatus(driver.maLx, 1)" class="w-full py-2 bg-emerald-50 text-emerald-600 hover:bg-emerald-500 hover:text-white rounded-lg font-semibold text-sm transition-colors border border-emerald-200 text-center">
                  Duyệt thẻ
                </button>
                <button v-if="driver.trangThai === 1" @click="changeStatus(driver.maLx, 2)" class="w-full py-2 bg-red-50 text-red-600 hover:bg-red-500 hover:text-white rounded-lg font-semibold text-sm transition-colors border border-red-200 text-center">
                  Khoá thẻ
                </button>
              </div>
            </div>
          </div>

          <!-- Desktop View: Table -->
          <div class="overflow-x-auto hidden lg:block">
            <table class="w-full text-left border-collapse">
              <thead>
                <tr class="bg-slate-50 text-slate-600 text-sm border-b border-slate-200">
                  <th class="p-4 font-semibold whitespace-nowrap">Số thẻ</th>
                  <th class="p-4 font-semibold">Tên Lái Xe</th>
                  <th class="p-4 font-semibold">Nhà Vận Tải</th>
                  <th class="p-4 font-semibold whitespace-nowrap">Hạn thẻ</th>
                  <th class="p-4 font-semibold text-center whitespace-nowrap">Trạng thái</th>
                  <th class="p-4 font-semibold text-right whitespace-nowrap">Thao tác</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100">
                <tr v-for="driver in filteredDrivers" :key="'desk-'+driver.maLx" class="hover:bg-slate-50/80 transition-colors">
                  <td class="p-4">
                    <span class="inline-block px-3 py-1 bg-slate-100 text-slate-800 rounded-lg font-mono text-sm shadow-sm border border-slate-200">
                      {{ driver.sothe || 'Chưa cấp' }}
                    </span>
                  </td>
                  <td class="p-4">
                    <div class="font-bold text-slate-800">{{ driver.ten }}</div>
                  </td>
                  <td class="p-4">
                    <div class="text-slate-800">{{ driver.tenNvt }}</div>
                  </td>
                  <td class="p-4">
                    <div class="flex items-center gap-2">
                      <span :class="isExpired(driver.thoihanthe) ? 'text-red-600 font-semibold' : 'text-slate-700'">
                        {{ formatDate(driver.thoihanthe) }}
                      </span>
                      <button @click="openExtendModal(driver)" class="text-blue-500 hover:text-blue-700 bg-blue-50 p-1.5 rounded-lg transition-colors" title="Gia hạn">
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path></svg>
                      </button>
                    </div>
                  </td>
                  <td class="p-4 text-center">
                    <!-- Trạng thái: 0: Chờ duyệt, 1: Đang hoạt động, 2: Bị khoá -->
                    <span v-if="driver.trangThai === 0" class="inline-flex items-center gap-1.5 px-3 py-1 bg-amber-100 text-amber-700 rounded-full text-xs font-bold border border-amber-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-amber-500"></span> Chờ duyệt
                    </span>
                    <span v-else-if="driver.trangThai === 1" class="inline-flex items-center gap-1.5 px-3 py-1 bg-emerald-100 text-emerald-700 rounded-full text-xs font-bold border border-emerald-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-emerald-500"></span> Hoạt động
                    </span>
                    <span v-else class="inline-flex items-center gap-1.5 px-3 py-1 bg-red-100 text-red-700 rounded-full text-xs font-bold border border-red-200">
                      <span class="w-1.5 h-1.5 rounded-full bg-red-500"></span> Đã khoá
                    </span>
                  </td>
                  <td class="p-4 text-right">
                    <div class="flex justify-end gap-2">
                      <button @click="openEditModal(driver)" class="px-3 py-1.5 bg-blue-50 text-blue-600 hover:bg-blue-500 hover:text-white rounded-lg font-semibold text-sm transition-colors border border-blue-200">
                        Sửa
                      </button>
                      <button v-if="driver.trangThai !== 1" @click="changeStatus(driver.maLx, 1)" class="px-3 py-1.5 bg-emerald-50 text-emerald-600 hover:bg-emerald-500 hover:text-white rounded-lg font-semibold text-sm transition-colors border border-emerald-200">
                        Duyệt
                      </button>
                      <button v-if="driver.trangThai === 1" @click="changeStatus(driver.maLx, 2)" class="px-3 py-1.5 bg-red-50 text-red-600 hover:bg-red-500 hover:text-white rounded-lg font-semibold text-sm transition-colors border border-red-200">
                        Khoá
                      </button>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Gia Hạn -->
    <div v-if="showExtendModal" class="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/60 backdrop-blur-sm px-4">
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-sm overflow-hidden transform transition-all border border-slate-100">
        <div class="px-6 py-4 border-b border-slate-100 bg-blue-50 flex items-center gap-3">
          <div class="w-8 h-8 bg-blue-100 text-blue-600 rounded-full flex items-center justify-center">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path></svg>
          </div>
          <h3 class="text-lg font-bold text-blue-800">Gia hạn thẻ Lái Xe</h3>
        </div>
        <div class="px-6 py-6">
          <p class="text-sm text-slate-600 mb-4">Gia hạn thẻ cho lái xe <strong class="text-slate-800">{{ selectedDriver?.ten }}</strong> (Thẻ: {{ selectedDriver?.sothe }})</p>
          <div class="space-y-2">
            <label class="text-sm font-semibold text-slate-700">Ngày hết hạn mới</label>
            <input v-model="newDate" type="date" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
        </div>
        <div class="px-6 py-4 bg-slate-50 flex justify-end gap-3 border-t border-slate-100">
          <button @click="showExtendModal = false" class="px-5 py-2 bg-white border border-slate-300 text-slate-700 rounded-xl hover:bg-slate-100 font-semibold transition-colors">
            Huỷ
          </button>
          <button @click="submitExtend" class="px-5 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 font-bold shadow-sm transition-colors" :disabled="extending">
            {{ extending ? 'Đang lưu...' : 'Lưu thay đổi' }}
          </button>
        </div>
      </div>
    </div>

    <!-- Modal Sửa Lái Xe -->
    <div v-if="showEditModal" class="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/60 backdrop-blur-sm px-4">
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-lg overflow-hidden transform transition-all border border-slate-100">
        <div class="px-6 py-4 border-b border-slate-100 bg-blue-50 flex items-center justify-between">
          <div class="flex items-center gap-3">
            <h3 class="text-lg font-bold text-blue-800">Sửa thông tin Lái Xe</h3>
          </div>
          <button @click="showEditModal = false" class="text-slate-400 hover:text-slate-600">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>
        </div>
        <div class="px-6 py-4 max-h-[70vh] overflow-y-auto space-y-4">
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">Tên lái xe</label>
            <input v-model="editForm.ten" type="text" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">Số thẻ</label>
            <input v-model="editForm.sothe" type="text" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">SĐT</label>
            <input v-model="editForm.sodt" type="text" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">CCCD</label>
            <input v-model="editForm.cccd" type="text" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none">
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">Nhà Vận Tải</label>
            <select v-model="editForm.maNvt" class="w-full px-4 py-2 border border-slate-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:outline-none bg-white">
              <option value="">-- Chọn Nhà Vận Tải --</option>
              <option v-for="nvt in nhaVanTais" :key="nvt.maNvt" :value="nvt.maNvt">{{ nvt.tenNvt }}</option>
            </select>
          </div>
        </div>
        <div class="px-6 py-4 bg-slate-50 flex justify-end gap-3 border-t border-slate-100">
          <button @click="showEditModal = false" class="px-5 py-2 bg-white border border-slate-300 text-slate-700 rounded-xl hover:bg-slate-100 font-semibold transition-colors">
            Huỷ
          </button>
          <button @click="submitEdit" class="px-5 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 font-bold shadow-sm transition-colors" :disabled="saving">
            {{ saving ? 'Đang lưu...' : 'Lưu thay đổi' }}
          </button>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'

definePageMeta({
  layout: false,
  title: 'Quản lý Lái Xe'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const drivers = ref([])
const loading = ref(true)
const searchQuery = ref('')

const showExtendModal = ref(false)
const selectedDriver = ref(null)
const newDate = ref('')
const extending = ref(false)

const showEditModal = ref(false)
const saving = ref(false)
const nhaVanTais = ref([])
const editForm = ref({
  maLx: '',
  ten: '',
  sothe: '',
  sodt: '',
  cccd: '',
  maNvt: '',
  tenNvt: '',
  thoihanthe: '',
  trangThai: 1
})

const fetchNhaVanTais = async () => {
  try {
    const res = await $fetch(`${apiBase}/NhaVanTais`)
    nhaVanTais.value = res || []
  } catch (err) {
    console.error("Lỗi khi tải nhà vận tải:", err)
  }
}

const filteredDrivers = computed(() => {
  if (!searchQuery.value) return drivers.value
  const q = searchQuery.value.toLowerCase()
  return drivers.value.filter(d => 
    (d.ten && d.ten.toLowerCase().includes(q)) ||
    (d.sothe && d.sothe.toLowerCase().includes(q)) ||
    (d.sodt && d.sodt.includes(q)) ||
    (d.tenNvt && d.tenNvt.toLowerCase().includes(q))
  )
})

const isExpired = (dateString) => {
  if (!dateString) return true
  const expiry = new Date(dateString)
  const today = new Date()
  today.setHours(0,0,0,0)
  return expiry < today
}

const formatDate = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN')
}

const fetchDrivers = async () => {
  try {
    loading.value = true
    const res = await $fetch(`${apiBase}/Thenhathaus`)
    drivers.value = res || []
  } catch (err) {
    console.error("Lỗi khi tải danh sách:", err)
    alert("Không thể tải danh sách lái xe.")
  } finally {
    loading.value = false
  }
}

const changeStatus = async (maLx, newStatus) => {
  const statusName = newStatus === 1 ? 'Duyệt' : 'Khoá'
  if (!confirm(`Bạn có chắc muốn ${statusName} thẻ này?`)) return
  
  try {
    await $fetch(`${apiBase}/Thenhathaus/${maLx}/status`, {
      method: 'PUT',
      body: { trangThai: newStatus }
    })
    // Cập nhật local state thay vì fetch lại cho nhanh
    const driver = drivers.value.find(d => d.maLx === maLx)
    if (driver) driver.trangThai = newStatus
  } catch (err) {
    alert("Có lỗi xảy ra: " + (err.response?._data?.message || err.message))
  }
}

const openExtendModal = (driver) => {
  selectedDriver.value = driver
  if (driver.thoihanthe) {
    const d = new Date(driver.thoihanthe)
    const year = d.getFullYear()
    const month = String(d.getMonth() + 1).padStart(2, '0')
    const day = String(d.getDate()).padStart(2, '0')
    newDate.value = `${year}-${month}-${day}`
  } else {
    newDate.value = ''
  }
  showExtendModal.value = true
}

const submitExtend = async () => {
  if (!newDate.value) {
    alert("Vui lòng chọn ngày")
    return
  }
  
  try {
    extending.value = true
    await $fetch(`${apiBase}/Thenhathaus/${selectedDriver.value.maLx}/extend`, {
      method: 'PUT',
      body: { newDate: newDate.value }
    })
    
    // Cập nhật local state
    const driver = drivers.value.find(d => d.maLx === selectedDriver.value.maLx)
    if (driver) driver.thoihanthe = newDate.value
    
    showExtendModal.value = false
  } catch (err) {
    alert("Có lỗi xảy ra: " + (err.response?._data?.message || err.message))
  } finally {
    extending.value = false
  }
}

const openEditModal = (driver) => {
  editForm.value = { ...driver }
  showEditModal.value = true
}

const submitEdit = async () => {
  try {
    saving.value = true
    await $fetch(`${apiBase}/Thenhathaus/${editForm.value.maLx}`, {
      method: 'PUT',
      body: editForm.value
    })
    
    // Update local state
    const index = drivers.value.findIndex(d => d.maLx === editForm.value.maLx)
    if (index !== -1) {
      if (editForm.value.maNvt) {
        const nvt = nhaVanTais.value.find(n => n.maNvt === editForm.value.maNvt)
        if (nvt) editForm.value.tenNvt = nvt.tenNvt
      }
      drivers.value[index] = { ...editForm.value }
    }
    
    showEditModal.value = false
  } catch (err) {
    alert("Có lỗi xảy ra: " + (err.response?._data?.message || err.message))
  } finally {
    saving.value = false
  }
}

onMounted(() => {
  fetchDrivers()
  fetchNhaVanTais()
})
</script>

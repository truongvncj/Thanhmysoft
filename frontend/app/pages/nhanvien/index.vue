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

    <!-- Main Content -->
    <main class="flex-1 w-full max-w-7xl mx-auto p-4 sm:p-6 lg:p-8">
      
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
                <button @click="updateStatus(xe.id, 2)" class="w-full bg-orange-500 hover:bg-orange-600 text-white font-bold py-2.5 rounded-xl transition-colors">
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
                    <button @click="updateStatus(xe.id, 2)" class="inline-block bg-orange-500 hover:bg-orange-600 text-white text-sm font-bold px-5 py-2 rounded-lg transition-colors shadow-sm">
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
      </div>
    </main>

    <GuardAuditModal 
      :show="showAuditModal" 
      :xe="selectedAuditXe" 
      @close="closeAuditModal"
      @success="handleAuditSuccess"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'

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

const tabs = [
  { name: 'Đã đăng ký', status: 0 },
  { name: 'Trong sân', status: 1 },
  { name: 'Chuẩn bị ra sân', status: 2 },
  { name: 'Đã hoàn thành', status: 3 }
]
const activeTab = ref(0)

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

const updateStatus = async (id, newStatus) => {
  if (!confirm('Xác nhận thao tác này?')) return
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

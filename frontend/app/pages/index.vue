<template>
  <div class="min-h-screen w-full bg-slate-50 flex flex-col items-center justify-center p-4 relative">
    
    <!-- Nút Admin ở góc trên cùng -->
    <button v-if="!selectedRole" @click="selectedRole = 'admin'" class="absolute top-4 right-4 sm:top-6 sm:right-6 bg-green-500 hover:bg-green-400 text-red-600 py-2 px-5 rounded-xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-green-700 transition-all duration-300 transform hover:-translate-y-1 hover:scale-[1.05] hover:shadow-[0_10px_20px_rgba(34,197,94,0.4)] flex items-center justify-center font-bold text-sm">
      Admin
    </button>

    <div class="w-full max-w-sm flex flex-col items-center">
      <!-- Dòng chữ tiêu đề ở phía trên -->
      <h1 v-if="!selectedRole" class="text-[18px] font-bold text-slate-800 mb-10 uppercase tracking-wide text-center w-full">
        Hệ thống quản lý
      </h1>
      <h1 v-else class="text-[18px] font-bold text-slate-800 mb-6 uppercase tracking-wide text-center w-full">
        Hệ thống quản lý
      </h1>
      
      <!-- Màn hình chọn vai trò (hiển thị khi chưa chọn) -->
      <div v-if="!selectedRole" class="w-full flex flex-col gap-7">
        <button @click="selectedRole = 'carrier'" class="w-full bg-blue-600 hover:bg-blue-500 text-yellow-300 py-4 px-4 rounded-2xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-blue-700 transition-all duration-300 transform hover:-translate-y-2 hover:scale-[1.02] hover:shadow-[0_15px_25px_rgba(37,99,235,0.4)] flex items-center justify-center font-bold">
          <span class="text-[16px] sm:text-[18px] text-center truncate">Nhà Vận Tải</span>
        </button>
        
        <button @click="selectedRole = 'driver'" class="w-full bg-blue-600 hover:bg-blue-500 text-yellow-300 py-4 px-4 rounded-2xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-blue-700 transition-all duration-300 transform hover:-translate-y-2 hover:scale-[1.02] hover:shadow-[0_15px_25px_rgba(37,99,235,0.4)] flex items-center justify-center font-bold">
          <span class="text-[16px] sm:text-[18px] text-center truncate">Lái xe</span>
        </button>
        
        <button @click="selectedRole = 'employee'" class="w-full bg-blue-600 hover:bg-blue-500 text-yellow-300 py-4 px-4 rounded-2xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-blue-700 transition-all duration-300 transform hover:-translate-y-2 hover:scale-[1.02] hover:shadow-[0_15px_25px_rgba(37,99,235,0.4)] flex items-center justify-center font-bold">
          <span class="text-[16px] sm:text-[18px] text-center truncate">Nhân viên</span>
        </button>
      </div>

      <!-- Màn hình Form đăng nhập (hiển thị khi đã chọn) -->
      <div v-else class="w-full flex flex-col gap-5 bg-white p-6 rounded-2xl shadow-lg border border-slate-100 animate-[fadeIn_0.3s_ease-out]">
        <!-- Tiêu đề form -->
        <h2 class="text-[16px] font-bold text-slate-700 text-center mb-2 border-b pb-4">
          {{ roleTitle }}
        </h2>
        
        <!-- Form cho Nhà Vận Tải -->
        <template v-if="selectedRole === 'carrier'">
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Mã nhà vận tải</label>
            <input v-model="carrierUsername" @keyup.enter="handleLogin" type="text" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors" placeholder="Nhập mã nhà vận tải">
          </div>
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Mật khẩu</label>
            <input v-model="carrierPassword" @keyup.enter="handleLogin" type="password" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors" placeholder="Nhập mật khẩu">
          </div>
        </template>
        
        <!-- Form cho Lái xe -->
        <template v-if="selectedRole === 'driver'">
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Số thẻ</label>
            <input v-model="driverSothe" @keyup.enter="handleDriverAuth" type="text" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors" placeholder="Nhập số thẻ">
          </div>
        </template>
        
        <!-- Form cho Nhân viên -->
        <template v-if="selectedRole === 'employee'">
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Mã nhân viên</label>
            <input v-model="employeeMnv" type="text" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors" placeholder="Nhập mã nhân viên">
          </div>
          <div class="flex flex-col gap-2 mt-2">
            <label class="text-sm font-semibold text-slate-600">Mật khẩu</label>
            <input v-model="employeePassword" @keyup.enter="handleLogin" type="password" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors" placeholder="Nhập mật khẩu">
          </div>
          <div class="flex flex-col gap-2 mt-2">
            <label class="text-sm font-semibold text-slate-600">Chọn kho làm việc</label>
            <select v-model="employeeKhohangId" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors">
              <option value="" disabled>-- Chọn kho --</option>
              <option v-for="kho in activeKhohangs" :key="kho.id" :value="kho.id">{{ kho.tenKho }}</option>
            </select>
          </div>
        </template>
        
        <!-- Form cho Admin -->
        <template v-if="selectedRole === 'admin'">
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Username</label>
            <input v-model="adminUsername" @keyup.enter="handleLogin" type="text" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:bg-white transition-colors" placeholder="Nhập Username">
          </div>
          <div class="flex flex-col gap-2">
            <label class="text-sm font-semibold text-slate-600">Password</label>
            <input v-model="adminPassword" @keyup.enter="handleLogin" type="password" class="px-4 py-3 bg-slate-50 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:bg-white transition-colors" placeholder="Nhập Password">
          </div>
        </template>
        
        <!-- Các nút hành động -->
        <div class="mt-4 flex flex-col gap-3">
          <!-- Đổi màu nút Đăng nhập nếu là Admin -->
          <button v-if="selectedRole === 'admin'" @click="handleLogin" class="w-full bg-green-500 hover:bg-green-600 text-white font-bold py-3.5 px-4 rounded-xl shadow-md hover:shadow-lg transition-all transform hover:-translate-y-1">
            Đăng nhập
          </button>
          <button v-else-if="selectedRole === 'driver'" @click="handleDriverAuth" class="w-full bg-blue-600 hover:bg-blue-700 text-yellow-300 font-bold py-3.5 px-4 rounded-xl shadow-md hover:shadow-lg transition-all transform hover:-translate-y-1">
            Đăng nhập
          </button>
          <button v-else @click="handleLogin" class="w-full bg-blue-600 hover:bg-blue-700 text-yellow-300 font-bold py-3.5 px-4 rounded-xl shadow-md hover:shadow-lg transition-all transform hover:-translate-y-1">
            Đăng nhập
          </button>
          
          <button @click="selectedRole = null" class="w-full bg-slate-100 hover:bg-slate-200 text-slate-600 font-bold py-3.5 px-4 rounded-xl transition-colors">
            Quay lại
          </button>
        </div>
      </div>
    </div>

    <!-- Modal chọn Kho và Lý do cho Lái xe -->
    <div v-if="showDriverModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-sm p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4 text-center border-b pb-2">Chọn Kho & Lý do</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">Kho hàng</label>
            <select v-model="driverKhohangId" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none">
              <option value="" disabled>-- Chọn kho --</option>
              <option v-for="kho in activeKhohangs" :key="kho.id" :value="kho.id">{{ kho.tenKho }}</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-1">Biển số xe</label>
            <input v-model="driverBienSo" type="text" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none uppercase" placeholder="Ví dụ: 29A-123.45">
          </div>
          <div>
            <label class="block text-sm font-semibold text-slate-700 mb-2">Lý do</label>
            <div class="flex gap-4 mb-4">
              <label class="flex items-center gap-2 cursor-pointer">
                <input type="radio" v-model="driverLyDo" value="Nhập hàng" class="text-blue-600 w-4 h-4">
                <span class="text-sm font-medium">Nhập hàng</span>
              </label>
              <label class="flex items-center gap-2 cursor-pointer">
                <input type="radio" v-model="driverLyDo" value="Xuất hàng" class="text-blue-600 w-4 h-4">
                <span class="text-sm font-medium">Xuất hàng</span>
              </label>
            </div>
          </div>
          
          <!-- STO / Shipment Fields -->
          <div v-if="driverLyDo === 'Nhập hàng'" class="space-y-3 bg-slate-50 p-3 rounded-lg border">
            <label class="block text-xs font-semibold text-slate-600 uppercase">Chứng từ Nhập hàng</label>
            <div v-for="(item, index) in nhapHangItems" :key="index" class="flex gap-2 relative group pb-2">
              <div class="flex-1">
                <input v-model="item.soTransferOut" type="text" class="w-full px-3 py-2 text-sm border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số Transfer Out">
              </div>
              <div class="flex-1">
                <input v-model="item.soSTO" type="text" class="w-full px-3 py-2 text-sm border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số STO">
              </div>
              <button @click="removeNhapHangItem(index)" v-if="nhapHangItems.length > 1" class="text-red-500 hover:bg-red-100 p-1.5 rounded-md transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>
            <button @click="addNhapHangItem" class="w-full py-1.5 border border-dashed border-blue-400 text-blue-600 text-sm font-medium rounded hover:bg-blue-50 transition-colors">
              + Thêm cặp Transfer Out & STO
            </button>
          </div>

          <div v-if="driverLyDo === 'Xuất hàng'" class="space-y-3 bg-slate-50 p-3 rounded-lg border">
            <label class="block text-xs font-semibold text-slate-600 uppercase">Chứng từ Xuất hàng</label>
            <div v-for="(item, index) in xuatHangItems" :key="index" class="flex gap-2 relative group pb-2">
              <div class="flex-1">
                <input v-model="item.soShipment" type="text" class="w-full px-3 py-2 text-sm border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số Shipment">
              </div>
              <button @click="removeXuatHangItem(index)" v-if="xuatHangItems.length > 1" class="text-red-500 hover:bg-red-100 p-1.5 rounded-md transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>
            <button @click="addXuatHangItem" class="w-full py-1.5 border border-dashed border-blue-400 text-blue-600 text-sm font-medium rounded hover:bg-blue-50 transition-colors">
              + Thêm Số Shipment
            </button>
          </div>
          <div class="flex justify-end gap-3 mt-6">
            <button @click="showDriverModal = false" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-sm font-semibold">Hủy</button>
            <button @click="handleDriverLoginFinish" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm text-sm font-semibold" :disabled="!driverKhohangId || !driverLyDo || !driverBienSo">
              Xác nhận
            </button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'

definePageMeta({
  layout: false,
  title: 'Hệ thống quản lý'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const router = useRouter()
const selectedRole = ref(null)

const adminUsername = ref('')
const adminPassword = ref('')

const carrierUsername = ref('')
const carrierPassword = ref('')

const driverSothe = ref('')
const showDriverModal = ref(false)
const driverKhohangId = ref('')
const driverBienSo = ref('')
const driverLyDo = ref('')
const driverTempAuthData = ref(null)

const nhapHangItems = ref([{ soTransferOut: '', soSTO: '' }])
const addNhapHangItem = () => nhapHangItems.value.push({ soTransferOut: '', soSTO: '' })
const removeNhapHangItem = (index) => nhapHangItems.value.splice(index, 1)

const xuatHangItems = ref([{ soShipment: '' }])
const addXuatHangItem = () => xuatHangItems.value.push({ soShipment: '' })
const removeXuatHangItem = (index) => xuatHangItems.value.splice(index, 1)

const employeeMnv = ref('')
const employeePassword = ref('')
const employeeKhohangId = ref('')

const activeKhohangs = ref([])

const fetchActiveKhohangs = async () => {
  try {
    const res = await $fetch(`${apiBase}/Khohangs?onlyOpen=true`)
    activeKhohangs.value = res || []
  } catch (err) {
    console.error('Lỗi tải danh sách kho', err)
  }
}

onMounted(() => {
  fetchActiveKhohangs()
})

const handleDriverAuth = async () => {
  if (!driverSothe.value) return
  try {
    const apiUrl = `${apiBase}/auth/driver-login`
    const res = await $fetch(apiUrl, {
      method: 'POST',
      body: {
        sothe: driverSothe.value
      }
    })
    // Save temp data and show modal
    driverTempAuthData.value = res.driver
    showDriverModal.value = true
  } catch (err) {
    alert(err.response?._data?.message || 'Số thẻ không hợp lệ hoặc không tồn tại.')
  }
}

const handleDriverLoginFinish = () => {
  if (process.client && driverTempAuthData.value) {
    localStorage.setItem('driver_maLx', driverTempAuthData.value.maLx)
    localStorage.setItem('driver_sothe', driverTempAuthData.value.sothe)
    localStorage.setItem('driver_ten', driverTempAuthData.value.ten)
    localStorage.setItem('driver_tenNvt', driverTempAuthData.value.tenNvt || '')
    localStorage.setItem('driver_khohang_id', driverKhohangId.value)
    localStorage.setItem('driver_bienSo', driverBienSo.value)
    localStorage.setItem('driver_lydo', driverLyDo.value)

    if (driverLyDo.value === 'Nhập hàng') {
      const validItems = nhapHangItems.value.filter(item => item.soTransferOut || item.soSTO)
      localStorage.setItem('driver_chungtus', JSON.stringify(validItems))
    } else if (driverLyDo.value === 'Xuất hàng') {
      const validItems = xuatHangItems.value.filter(item => item.soShipment)
      localStorage.setItem('driver_chungtus', JSON.stringify(validItems))
    }
  }
  showDriverModal.value = false
  router.push('/driver')
}

const handleLogin = async () => {
  if (selectedRole.value === 'admin') {
    try {
      const apiUrl = `${apiBase}/auth/admin-login`
      const res = await $fetch(apiUrl, {
        method: 'POST',
        body: {
          username: adminUsername.value,
          password: adminPassword.value
        }
      })
      // Save info if needed, then redirect
      router.push('/admin')
    } catch (err) {
      alert(err.response?._data?.message || 'Tên đăng nhập hoặc mật khẩu không đúng.')
    }
  } else if (selectedRole.value === 'carrier') {
    try {
      const apiUrl = `${apiBase}/auth/carrier-login`
      const res = await $fetch(apiUrl, {
        method: 'POST',
        body: {
          maNvt: carrierUsername.value,
          password: carrierPassword.value
        }
      })
      // Save info to localStorage
      if (process.client) {
        localStorage.setItem('carrier_maNvt', res.carrier.maNvt)
        localStorage.setItem('carrier_tenNvt', res.carrier.tenNvt)
      }
      router.push('/carrier')
    } catch (err) {
      alert(err.response?._data?.message || 'Mã nhà vận tải hoặc mật khẩu không đúng.')
    }
  } else if (selectedRole.value === 'employee') {
    if (!employeeKhohangId.value) {
      alert('Vui lòng chọn kho làm việc!')
      return
    }
    try {
      const apiUrl = `${apiBase}/auth/nhanvien-login`
      const res = await $fetch(apiUrl, {
        method: 'POST',
        body: {
          mnv: employeeMnv.value,
          password: employeePassword.value,
          khohangId: employeeKhohangId.value
        }
      })
      
      if (process.client) {
        localStorage.setItem('employee_info', JSON.stringify(res.nhanvien))
        localStorage.setItem('employee_khohang', JSON.stringify(res.khohang))
        
        const perms = res.nhanvien.permissions || ''
        if (!perms.includes('Dashboard') && perms.includes('Admin_')) {
          // Nếu không có quyền màn hình điều độ nhưng có quyền Admin, thì vào Admin
          router.push('/admin')
        } else {
          router.push('/nhanvien')
        }
      }
    } catch (err) {
      alert(err.response?._data?.message || 'Mã nhân viên hoặc mật khẩu không đúng.')
    }
  } else {
    alert('Chức năng đang được phát triển')
  }
}

const roleTitle = computed(() => {
  if (selectedRole.value === 'carrier') return 'ĐĂNG NHẬP NHÀ VẬN TẢI'
  if (selectedRole.value === 'driver') return 'ĐĂNG NHẬP LÁI XE'
  if (selectedRole.value === 'employee') return 'ĐĂNG NHẬP NHÂN VIÊN'
  if (selectedRole.value === 'admin') return 'ĐĂNG NHẬP QUẢN TRỊ VIÊN'
  return ''
})
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

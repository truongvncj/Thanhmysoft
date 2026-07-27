<template>
  <div class="min-h-screen w-full bg-slate-50 flex flex-col items-center justify-center p-4 relative">
    
    <!-- Nút Admin ở góc trên cùng -->
    <button v-if="!selectedRole" @click="selectedRole = 'admin'" class="absolute top-4 right-4 sm:top-6 sm:right-6 bg-green-500 hover:bg-green-400 text-red-600 py-2 px-5 rounded-xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-green-700 transition-all duration-300 transform hover:-translate-y-1 hover:scale-[1.05] hover:shadow-[0_10px_20px_rgba(34,197,94,0.4)] flex items-center justify-center font-bold text-sm">
      Admin
    </button>

    <div class="w-full max-w-sm flex flex-col items-center">
      <!-- Logo -->
      <img src="/logo.svg" alt="Thanh My In.,JSC" class="w-full -mt-16 mb-10 object-contain" />

      <!-- Removed title text -->
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
    
    if (process.client) {
      localStorage.setItem('driver_maLx', res.driver.maLx)
      localStorage.setItem('driver_sothe', res.driver.sothe)
      localStorage.setItem('driver_ten', res.driver.ten)
      localStorage.setItem('driver_tenNvt', res.driver.tenNvt || '')
    }
    
    router.push('/driver')
  } catch (err) {
    alert(err.response?._data?.message || 'Số thẻ không hợp lệ hoặc không tồn tại.')
  }
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

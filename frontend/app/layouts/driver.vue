<template>
  <div class="min-h-screen bg-slate-50 flex flex-col md:flex-row">
    <!-- Sidebar -->
    <aside class="w-full md:w-64 bg-slate-900 text-slate-300 flex flex-col transition-all duration-300 md:h-screen shrink-0">
      <div class="p-4 md:p-6 text-center border-b border-slate-800 flex justify-center md:block items-center">
        <div>
          <h2 class="text-lg md:text-xl font-bold text-white tracking-wider">LÁI XE</h2>
          <p class="text-xs md:text-sm text-slate-500 mt-0 md:mt-1">{{ driverTenNvt || 'Nhà vận tải' }}</p>
        </div>
      </div>
      
      <nav class="flex-none md:flex-1 py-3 md:py-6 overflow-x-auto md:overflow-y-auto hide-scrollbar">
        <ul class="flex flex-row md:flex-col space-x-2 md:space-x-0 md:space-y-2 px-4 w-max md:w-auto mx-auto">
          <li>
            <NuxtLink to="/driver" class="flex items-center gap-3 px-4 py-2 md:py-3 rounded-xl transition-all duration-200 hover:bg-blue-600 hover:text-white whitespace-nowrap" exactActiveClass="bg-blue-600 text-white shadow-md">
              <span class="font-medium text-sm md:text-base">Pre-Trip Checklist</span>
            </NuxtLink>
          </li>
          <li>
            <NuxtLink :to="state.daDangTai ? '/driver/vesinh' : undefined" :class="['flex items-center gap-3 px-4 py-2 md:py-3 rounded-xl transition-all duration-200 whitespace-nowrap', state.daDangTai ? 'hover:bg-blue-600 hover:text-white' : 'opacity-50 cursor-not-allowed']" exactActiveClass="bg-blue-600 text-white shadow-md">
              <span class="font-medium text-sm md:text-base">Kiểm tra vệ sinh</span>
            </NuxtLink>
          </li>
          <li>
            <NuxtLink :to="state.daVeSinh ? '/driver/dangky' : undefined" :class="['flex items-center gap-3 px-4 py-2 md:py-3 rounded-xl transition-all duration-200 whitespace-nowrap', state.daVeSinh ? 'hover:bg-blue-600 hover:text-white' : 'opacity-50 cursor-not-allowed']" exactActiveClass="bg-blue-600 text-white shadow-md">
              <span class="font-medium text-sm md:text-base">Đăng ký xe vào sân</span>
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/driver/history" class="flex items-center gap-3 px-4 py-2 md:py-3 rounded-xl transition-all duration-200 hover:bg-blue-600 hover:text-white whitespace-nowrap" exactActiveClass="bg-blue-600 text-white shadow-md">
              <span class="font-medium text-sm md:text-base">Lịch sử đăng ký</span>
            </NuxtLink>
          </li>
        </ul>
      </nav>
      
      <div class="p-4 border-t border-slate-800 hidden md:block mt-auto">
        <button @click="logout" class="w-full flex items-center justify-center gap-2 bg-red-500/10 text-red-500 hover:bg-red-500 hover:text-white py-3 rounded-xl transition-all duration-200 font-semibold">
          Đăng xuất
        </button>
      </div>
    </aside>

    <!-- Main Content -->
    <main class="flex-1 flex flex-col h-[calc(100vh-125px)] md:h-screen overflow-hidden">
      <!-- Header -->
      <header class="bg-white border-b border-slate-200 min-h-16 flex flex-col sm:flex-row items-center justify-between px-4 sm:px-8 py-3 shadow-sm gap-3 sm:gap-0 shrink-0">
        <div class="font-semibold text-slate-700 text-base md:text-lg w-full sm:w-1/3 text-center sm:text-left">

        </div>

        <!-- Center Content: Driver Name and Card Number -->
        <div class="flex-1 flex justify-center items-center gap-2 flex-wrap text-center w-full">
          <span class="text-slate-800 font-bold text-base md:text-lg">{{ driverTen || 'Tài xế' }}</span>
          <span class="text-slate-300 font-medium hidden sm:inline">|</span>
          <span class="text-slate-600 font-medium text-sm md:text-base whitespace-nowrap">Số thẻ: {{ driverSothe }}</span>
        </div>

        <div class="flex items-center gap-4 w-full sm:w-1/3 justify-center sm:justify-end">
          <div class="hidden sm:flex w-10 h-10 rounded-full bg-blue-100 items-center justify-center text-blue-600 font-bold border-2 border-blue-200 shadow-sm">
            LX
          </div>
          <button @click="logout" class="sm:hidden px-4 py-2 bg-red-50 text-red-500 rounded-lg text-sm font-bold border border-red-100 shadow-sm w-full">
            Đăng xuất
          </button>
        </div>
      </header>
      
      <!-- Page Content -->
      <div class="flex-1 overflow-auto p-4 md:p-8 bg-slate-50">
        <slot />
      </div>
    </main>
  </div>
</template>

<style scoped>
.hide-scrollbar::-webkit-scrollbar {
  display: none;
}
.hide-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>

<script setup>
import { ref, onMounted, watch } from 'vue'
const router = useRouter()
const route = useRoute()
const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const driverSothe = ref('')
const driverTen = ref('')
const driverTenNvt = ref('')
const driverBienSo = ref('')

const state = ref({ daDangTai: false, daVeSinh: false })

const fetchState = async () => {
  if (driverSothe.value) {
    try {
      const res = await $fetch(`${apiBase}/DangTais/state?sothe=${driverSothe.value}`)
      state.value = res
    } catch (e) {}
  }
}

watch(() => route.path, fetchState)

onMounted(() => {
  if (process.client) {
    driverSothe.value = localStorage.getItem('driver_sothe') || ''
    driverTen.value = localStorage.getItem('driver_ten') || ''
    driverTenNvt.value = localStorage.getItem('driver_tenNvt') || ''
    driverBienSo.value = localStorage.getItem('driver_bienSo') || ''
    if (driverTenNvt.value === 'undefined') driverTenNvt.value = ''
    if (!driverSothe.value) {
      router.push('/')
    } else {
      fetchState()
    }
  }
})

const logout = () => {
  if (process.client) {
    localStorage.clear()
  }
  router.push('/')
}
</script>

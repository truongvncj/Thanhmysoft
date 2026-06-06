<template>
  <div class="min-h-screen bg-gray-50 flex relative">
    <!-- Mobile Overlay -->
    <div v-if="isSidebarOpen" @click="isSidebarOpen = false" class="fixed inset-0 bg-black/50 z-40 md:hidden transition-opacity"></div>

    <!-- Sidebar -->
    <aside 
      class="fixed inset-y-0 left-0 z-50 w-64 bg-slate-900 text-white flex flex-col transition-transform duration-300 transform md:relative md:translate-x-0"
      :class="isSidebarOpen ? 'translate-x-0' : '-translate-x-full'"
    >
      <div class="h-20 flex flex-col items-center justify-center border-b border-slate-700 p-4">
        <h1 class="text-lg font-bold tracking-wider text-center text-blue-400 uppercase">Nhà Vận Tải</h1>
        <p class="text-xs text-gray-400 mt-1" v-if="tenNvt">{{ tenNvt }}</p>
      </div>
      <nav class="flex-1 py-4">
        <ul class="space-y-2">
          <li>
            <NuxtLink to="/carrier" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Tổng quan
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/carrier/thenhathau" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Đăng ký thẻ nhà thầu
            </NuxtLink>
          </li>
        </ul>
      </nav>
      <div class="p-4 border-t border-slate-700">
        <button @click="logout" class="w-full py-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors font-semibold shadow">
          Đăng xuất
        </button>
      </div>
    </aside>

    <!-- Main Content -->
    <main class="flex-1 flex flex-col h-screen overflow-hidden w-full">
      <!-- Header -->
      <header class="h-16 bg-white shadow-sm flex items-center px-4 md:px-6 justify-between shrink-0">
        <div class="flex items-center gap-3 overflow-hidden">
          <button @click="isSidebarOpen = !isSidebarOpen" class="md:hidden p-2 rounded-lg hover:bg-slate-100 text-slate-600 focus:outline-none focus:ring-2 focus:ring-blue-500 shrink-0">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path></svg>
          </button>
          <h2 class="text-lg md:text-xl font-semibold text-gray-800 truncate">{{ route.meta.title || 'Quản lý' }}</h2>
        </div>
        <div class="flex items-center gap-4">
          <div class="w-8 h-8 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 font-bold">
            {{ tenNvt ? tenNvt.charAt(0).toUpperCase() : 'C' }}
          </div>
        </div>
      </header>
      
      <!-- Page Content -->
      <div class="flex-1 overflow-auto p-6">
        <div class="max-w-7xl mx-auto">
          <slot />
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'

const route = useRoute()
const router = useRouter()
const isSidebarOpen = ref(false)
const tenNvt = ref('')

onMounted(() => {
  if (process.client) {
    const name = localStorage.getItem('carrier_tenNvt')
    if (name) {
      tenNvt.value = name
    }
  }
})

// Close sidebar on route change on mobile
watch(() => route.fullPath, () => {
  isSidebarOpen.value = false
})

const logout = () => {
  if (process.client) {
    localStorage.removeItem('carrier_maNvt')
    localStorage.removeItem('carrier_tenNvt')
  }
  router.push('/')
}
</script>

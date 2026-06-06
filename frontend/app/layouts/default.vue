<template>
  <div class="min-h-screen bg-gray-50 flex relative">
    <!-- Mobile Overlay -->
    <div v-if="isSidebarOpen" @click="isSidebarOpen = false" class="fixed inset-0 bg-black/50 z-40 md:hidden transition-opacity"></div>

    <!-- Sidebar -->
    <aside 
      class="fixed inset-y-0 left-0 z-50 w-64 bg-slate-900 text-white flex flex-col transition-transform duration-300 transform md:relative md:translate-x-0"
      :class="isSidebarOpen ? 'translate-x-0' : '-translate-x-full'"
    >
      <div class="h-20 flex items-center justify-center border-b border-slate-700 p-4">
        <NuxtLink to="/admin" class="w-full bg-green-500 hover:bg-green-400 text-red-600 py-2.5 px-5 rounded-xl shadow-[0_4px_6px_rgba(0,0,0,0.1),0_2px_4px_rgba(0,0,0,0.06)] border-b-4 border-green-700 transition-all duration-300 transform hover:-translate-y-1 hover:scale-[1.02] hover:shadow-[0_10px_20px_rgba(34,197,94,0.4)] flex items-center justify-center font-bold text-sm uppercase tracking-wider">
          Hệ Thống
        </NuxtLink>
      </div>
      <nav class="flex-1 py-4">
        <ul class="space-y-2">
          <li>
            <NuxtLink to="/nhavantais" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Nhà vận tải
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/laixe" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Lái xe
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/nhanvien" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Nhân viên
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/baove" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Bảo vệ
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/pre-trip-checklist" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Thông số Pre-Trip
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/kiem-tra-ve-sinh" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Thông số Vệ Sinh Xe
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/qrcode" class="block px-6 py-3 hover:bg-slate-800 transition-colors" active-class="bg-blue-600 hover:bg-blue-600">
              Tạo QR Code
            </NuxtLink>
          </li>
        </ul>
      </nav>
      <div class="p-4 text-xs text-slate-400 text-center border-t border-slate-700">
        &copy; 2026 SaleManager
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
            A
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
import { ref } from 'vue'

const route = useRoute()
const isSidebarOpen = ref(false)

// Close sidebar on route change on mobile
watch(() => route.fullPath, () => {
  isSidebarOpen.value = false
})
</script>

<template>
  <div>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
      <div class="bg-white rounded-xl shadow-sm border border-gray-100 p-6 flex items-center">
        <div class="w-12 h-12 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 mr-4">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1m-4 0a2 2 0 104 0m-5 8a2 2 0 100-4 2 2 0 000 4zm0 0c1.306 0 2.417.835 2.83 2M9 14a3.001 3.001 0 00-2.83 2M15 11h3m-3 4h2" />
          </svg>
        </div>
        <div>
          <p class="text-sm font-medium text-gray-500 uppercase">Tổng thẻ đã đăng ký</p>
          <p class="text-2xl font-bold text-gray-900 mt-1">{{ totalCards }}</p>
        </div>
      </div>
    </div>
    
    <div class="bg-white rounded-xl shadow-sm border border-gray-100 p-8 text-center">
      <h3 class="text-xl font-bold text-gray-800 mb-2">Chào mừng đến với Cổng thông tin Nhà Vận Tải</h3>
      <p class="text-gray-500 mb-6">Bạn có thể quản lý và đăng ký thẻ nhà thầu cho nhân viên, lái xe của mình tại đây.</p>
      <NuxtLink to="/carrier/thenhathau" class="inline-flex items-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-colors shadow font-medium">
        Đi đến Đăng ký thẻ
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
        </svg>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

definePageMeta({
  layout: 'carrier',
  title: 'Tổng quan Nhà vận tải'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl
const totalCards = ref(0)
const maNvt = ref('')

onMounted(async () => {
  if (process.client) {
    maNvt.value = localStorage.getItem('carrier_maNvt') || ''
    if (maNvt.value) {
      try {
        const res = await $fetch(`${apiBase}/Thenhathaus?maNvt=${maNvt.value}`)
        totalCards.value = res.length
      } catch (e) {
        console.error('Lỗi khi tải số lượng thẻ:', e)
      }
    }
  }
})
</script>

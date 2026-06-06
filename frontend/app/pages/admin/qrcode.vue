<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-4xl mx-auto space-y-6">
      <!-- Header -->
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Tạo Mã QR (QR Code Generator)</h2>
          <p class="text-slate-500 mt-1">Tạo mã QR nhanh từ đường link để lái xe đăng nhập</p>
        </div>
        <NuxtLink to="/admin" class="flex items-center gap-2 text-blue-600 bg-blue-50 px-4 py-2 rounded-xl font-semibold hover:bg-blue-100 transition-colors">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
          </svg>
          Quay lại Admin
        </NuxtLink>
      </div>

      <!-- Main Box -->
      <div class="bg-white rounded-2xl shadow-sm border border-gray-100 p-8">
        <div class="flex flex-col md:flex-row gap-8">
          <!-- Input Form -->
          <div class="flex-1 space-y-6">
            <div>
              <label class="block text-sm font-bold text-slate-700 mb-2">Đường dẫn (URL)</label>
              <input 
                v-model="qrData" 
                type="text" 
                placeholder="Nhập đường link (VD: https://your-app.com/login)..." 
                class="w-full px-4 py-3 border border-slate-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow shadow-sm"
              />
              <p class="text-sm text-slate-500 mt-2">Ví dụ: đường link tới trang đăng nhập của lái xe.</p>
            </div>
            
            <div>
              <label class="block text-sm font-bold text-slate-700 mb-2">Kích thước QR</label>
              <input 
                v-model.number="qrSize" 
                type="range" 
                min="100" max="400" step="10" 
                class="w-full accent-blue-600"
              />
              <p class="text-sm text-slate-500 mt-1 text-center">{{ qrSize }}px</p>
            </div>
          </div>

          <!-- QR Preview -->
          <div class="w-full md:w-1/3 flex flex-col items-center justify-center border-l border-slate-100 pl-0 md:pl-8 mt-8 md:mt-0">
            <h3 class="text-sm font-bold text-slate-700 mb-4 uppercase tracking-wider">Xem trước Mã QR</h3>
            
            <div class="p-4 bg-white border border-slate-200 rounded-2xl shadow-sm inline-block" id="qr-container">
              <ClientOnly>
                <QrcodeVue v-if="qrData" :value="qrData" :size="qrSize" level="H" />
                <div v-else class="text-slate-400 text-sm italic text-center w-48 h-48 flex items-center justify-center border-2 border-dashed border-slate-200 rounded-xl">
                  Nhập link để tạo mã QR
                </div>
              </ClientOnly>
            </div>

            <button 
              v-if="qrData" 
              @click="downloadQR" 
              class="mt-6 w-full px-4 py-3 bg-blue-600 text-white font-bold rounded-xl shadow-md shadow-blue-500/30 hover:bg-blue-700 transition-colors flex justify-center items-center gap-2"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M3 17a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm3.293-7.707a1 1 0 011.414 0L9 10.586V3a1 1 0 112 0v7.586l1.293-1.293a1 1 0 111.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z" clip-rule="evenodd" />
              </svg>
              Tải Mã QR
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import QrcodeVue from 'qrcode.vue'

definePageMeta({
  layout: false,
  title: 'Tạo Mã QR'
})

// Mặc định url để test hoặc rỗng
const qrData = ref('')
const qrSize = ref(200)

onMounted(() => {
  const savedUrl = localStorage.getItem('saved_qrcode_url')
  if (savedUrl) {
    qrData.value = savedUrl
  }
})

watch(qrData, (newValue) => {
  localStorage.setItem('saved_qrcode_url', newValue)
})

const downloadQR = () => {
  const canvas = document.querySelector('#qr-container canvas')
  if (canvas) {
    const url = canvas.toDataURL('image/png')
    const link = document.createElement('a')
    link.download = 'qrcode.png'
    link.href = url
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
}
</script>

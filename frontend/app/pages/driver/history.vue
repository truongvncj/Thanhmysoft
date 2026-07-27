<template>
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="bg-white p-6 md:p-8 rounded-2xl shadow-sm border border-slate-100">
      <h1 class="text-2xl md:text-3xl font-bold text-slate-800 text-center mb-8 tracking-tight">LỊCH SỬ ĐĂNG KÝ</h1>
      
      <!-- Tabs -->
      <div class="flex flex-col sm:flex-row gap-2 border-b border-slate-200 mb-6">
        <button 
          @click="activeTab = 'dangky'"
          :class="activeTab === 'dangky' ? 'border-b-2 border-indigo-600 text-indigo-600 font-bold' : 'text-slate-500 hover:text-slate-700 font-medium'"
          class="px-6 py-3 transition-colors text-center sm:text-left"
        >
          Đăng ký vào kho
        </button>
        <button 
          @click="activeTab = 'pretrip'"
          :class="activeTab === 'pretrip' ? 'border-b-2 border-blue-600 text-blue-600 font-bold' : 'text-slate-500 hover:text-slate-700 font-medium'"
          class="px-6 py-3 transition-colors text-center sm:text-left"
        >
          Pre-Trip Checklist
        </button>
        <button 
          @click="activeTab = 'vesinh'"
          :class="activeTab === 'vesinh' ? 'border-b-2 border-teal-600 text-teal-600 font-bold' : 'text-slate-500 hover:text-slate-700 font-medium'"
          class="px-6 py-3 transition-colors text-center sm:text-left"
        >
          Kiểm tra vệ sinh xe
        </button>
      </div>

      <div v-if="loading" class="text-center py-12">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-slate-500 font-medium">Đang tải dữ liệu lịch sử...</p>
      </div>

      <!-- Tab Content: Đăng ký vào kho -->
      <div v-else-if="activeTab === 'dangky'" class="space-y-4">
        <div v-if="dangkyHistory.length === 0" class="text-center py-10 text-slate-500">
          Chưa có lịch sử Đăng ký vào kho nào.
        </div>
        <div v-else v-for="item in dangkyHistory" :key="item.id" class="p-5 border border-slate-200 rounded-xl hover:shadow-md transition-shadow bg-slate-50">
          <div class="flex justify-between items-start mb-3">
            <div>
              <span class="inline-flex items-center gap-1 px-3 py-1 bg-indigo-100 text-indigo-700 rounded-full text-sm font-bold mb-2">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-12a1 1 0 10-2 0v4a1 1 0 00.293.707l2.828 2.829a1 1 0 101.415-1.415L11 9.586V6z" clip-rule="evenodd" /></svg>
                {{ formatDate(item.ngayDangTai) }}
              </span>
              <h3 class="font-bold text-slate-800">Đăng ký vào kho</h3>
            </div>
            <span class="text-indigo-600 bg-indigo-50 p-2 rounded-full">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" /></svg>
            </span>
          </div>
          <div class="mt-4 pl-2 border-l-2 border-slate-300 space-y-2 text-sm text-slate-600">
            <div class="flex gap-2 items-start">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-slate-400 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-6-3a2 2 0 11-4 0 2 2 0 014 0zm-2 4a5 5 0 00-4.546 2.916A5.986 5.986 0 0010 16a5.986 5.986 0 004.546-2.084A5 5 0 0010 11z" clip-rule="evenodd" /></svg>
              <span>Lý do: <span class="font-bold text-slate-800">{{ item.lyDo }}</span></span>
            </div>
            <div v-if="item.chungTus && item.chungTus.length > 0" class="flex gap-2 items-start">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-slate-400 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" clip-rule="evenodd" /></svg>
              <span>Chứng từ: <span class="font-bold text-slate-800">{{ item.chungTus.join(', ') }}</span></span>
            </div>
            <div class="flex gap-2 items-start">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-slate-400 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M5.05 4.05a7 7 0 119.9 9.9L10 18.9l-4.95-4.95a7 7 0 010-9.9zM10 11a2 2 0 100-4 2 2 0 000 4z" clip-rule="evenodd" /></svg>
              <span>Trạng thái: 
                <span class="font-bold" :class="item.trangThai === 0 ? 'text-amber-600' : (item.trangThai === 1 ? 'text-blue-600' : 'text-emerald-600')">
                  {{ item.trangThai === 0 ? 'Đã đăng ký' : (item.trangThai === 1 ? 'Trong sân' : (item.trangThai === 2 ? 'Chuẩn bị ra sân' : 'Đã hoàn thành')) }}
                </span>
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Tab Content: Pre-Trip -->
      <div v-else-if="activeTab === 'pretrip'" class="space-y-4">
        <div v-if="preTripHistory.length === 0" class="text-center py-10 text-slate-500">
          Chưa có lịch sử Pre-Trip Checklist nào.
        </div>
        <div v-else v-for="item in preTripHistory" :key="item.id" class="p-5 border border-slate-200 rounded-xl hover:shadow-md transition-shadow bg-slate-50">
          <div class="flex justify-between items-start mb-3">
            <div>
              <span class="inline-flex items-center gap-1 px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm font-bold mb-2">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd" /></svg>
                {{ formatDate(item.date) }}
              </span>
              <h3 class="font-bold text-slate-800">Pre-Trip Checklist - Đạt</h3>
            </div>
            <span class="text-green-600 bg-green-50 p-2 rounded-full">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            </span>
          </div>
          <div class="mt-4 pl-2 border-l-2 border-slate-300 space-y-2 text-sm text-slate-600">
            <div v-for="(param, idx) in parseChecklist(item.checklistData)" :key="idx" class="flex gap-2 items-start">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-green-500 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" /></svg>
              <span>{{ param.noiDung }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Tab Content: Vệ sinh -->
      <div v-else-if="activeTab === 'vesinh'" class="space-y-4">
        <div v-if="veSinhHistory.length === 0" class="text-center py-10 text-slate-500">
          Chưa có lịch sử Kiểm tra vệ sinh nào.
        </div>
        <div v-else v-for="item in veSinhHistory" :key="item.id" class="p-5 border border-slate-200 rounded-xl hover:shadow-md transition-shadow bg-slate-50">
          <div class="flex justify-between items-start mb-3">
            <div>
              <span class="inline-flex items-center gap-1 px-3 py-1 bg-teal-100 text-teal-700 rounded-full text-sm font-bold mb-2">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-12a1 1 0 10-2 0v4a1 1 0 00.293.707l2.828 2.829a1 1 0 101.415-1.415L11 9.586V6z" clip-rule="evenodd" /></svg>
                {{ formatDate(item.ngayKiemTra) }}
              </span>
              <h3 class="font-bold text-slate-800">Kiểm tra vệ sinh - Lượt {{ item.lanVaoKhoTrongNgay || 1 }}</h3>
            </div>
            <span class="text-teal-600 bg-teal-50 p-2 rounded-full">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
            </span>
          </div>
          <div class="mt-4 pl-2 border-l-2 border-slate-300 space-y-2 text-sm text-slate-600">
            <div v-for="detail in item.details" :key="detail.id" class="flex gap-2 items-start">
              <svg v-if="detail.ketQua" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-teal-500 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" /></svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-red-500 mt-0.5 shrink-0" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" /></svg>
              <span>
                {{ detail.param ? detail.param.noiDung : 'Mục kiểm tra' }} 
                <span class="font-bold text-slate-800">({{ detail.ketQua ? 'Có' : 'Không' }})</span>
              </span>
            </div>
          </div>
        </div>
      </div>
      
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

definePageMeta({
  layout: 'driver',
  title: 'Lịch sử Đăng ký'
})

const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const activeTab = ref('dangky') // 'dangky', 'pretrip' or 'vesinh'
const loading = ref(true)
const dangkyHistory = ref([])
const preTripHistory = ref([])
const veSinhHistory = ref([])
const sothe = ref('')

const formatDate = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return new Intl.DateTimeFormat('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  }).format(date)
}

const parseChecklist = (jsonStr) => {
  if (!jsonStr) return []
  try {
    return JSON.parse(jsonStr)
  } catch (e) {
    return []
  }
}

const fetchHistory = async () => {
  loading.value = true
  try {
    const [dangkyRes, preTripRes, veSinhRes] = await Promise.all([
      $fetch(`${apiBase}/DangTais/history/dangky?sothe=${sothe.value}`),
      $fetch(`${apiBase}/DangTais/history/pre-trip?sothe=${sothe.value}`),
      $fetch(`${apiBase}/DangTais/history/vesinh?sothe=${sothe.value}`)
    ])
    dangkyHistory.value = dangkyRes || []
    preTripHistory.value = preTripRes || []
    veSinhHistory.value = veSinhRes || []
  } catch (err) {
    console.error("Lỗi khi tải lịch sử:", err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  if (process.client) {
    sothe.value = localStorage.getItem('driver_sothe') || ''
    if (!sothe.value) {
      router.push('/')
      return
    }
    fetchHistory()
  }
})
</script>

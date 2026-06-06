<template>
  <div v-if="show" class="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/60 backdrop-blur-sm px-4">
    <div class="bg-white rounded-2xl shadow-2xl w-full max-w-3xl overflow-hidden flex flex-col max-h-[90vh]">
      <!-- Header -->
      <div class="px-6 py-4 border-b border-slate-100 flex items-center justify-between bg-slate-50">
        <h3 class="text-xl font-bold text-slate-800">
          Bảo vệ kiểm tra: <span class="text-blue-600">{{ xe?.bienSo }}</span>
        </h3>
        <button @click="close" class="text-slate-400 hover:text-red-500 transition-colors">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
        </button>
      </div>
      
      <!-- Body -->
      <div class="flex-1 overflow-y-auto p-6 custom-scrollbar">
        <div v-if="loading" class="text-center py-10">
          <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mx-auto"></div>
          <p class="mt-4 text-slate-500">Đang tải dữ liệu...</p>
        </div>
        
        <div v-else>
          <!-- Step 1: Selection -->
          <div v-if="step === 'select'">
            <h4 class="text-center text-lg font-bold text-slate-700 mb-6">Vui lòng chọn loại kiểm tra</h4>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <!-- Pre-trip button -->
              <button 
                @click="startPreTrip" 
                :disabled="xe?.baoVeDaKiemTraPreTripHomNay"
                class="flex flex-col items-center justify-center p-8 border-2 rounded-2xl transition-all"
                :class="xe?.baoVeDaKiemTraPreTripHomNay ? 'border-slate-200 bg-slate-50 opacity-60 cursor-not-allowed' : 'border-blue-200 hover:border-blue-500 hover:bg-blue-50 bg-white'"
              >
                <div class="w-16 h-16 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 mb-4 shadow-inner">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
                </div>
                <h4 class="text-xl font-bold text-slate-800">Pre-Trip Checklist</h4>
                <p class="text-slate-500 mt-2 text-center text-sm">
                  {{ xe?.baoVeDaKiemTraPreTripHomNay ? 'Đã kiểm tra hôm nay' : 'Kiểm tra 1 lần/ngày' }}
                </p>
                <div v-if="xe?.baoVeDaKiemTraPreTripHomNay" class="mt-4 bg-slate-200 text-slate-600 text-xs font-bold px-3 py-1 rounded-full">
                  Đã hoàn thành
                </div>
              </button>

              <!-- Ve Sinh button -->
              <button 
                @click="startVeSinh"
                class="flex flex-col items-center justify-center p-8 border-2 border-teal-200 hover:border-teal-500 hover:bg-teal-50 bg-white rounded-2xl transition-all"
              >
                <div class="w-16 h-16 rounded-full bg-teal-100 flex items-center justify-center text-teal-600 mb-4 shadow-inner">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z" /></svg>
                </div>
                <h4 class="text-xl font-bold text-slate-800">Kiểm tra Vệ sinh xe</h4>
                <p class="text-slate-500 mt-2 text-center text-sm">Kiểm tra mỗi lần vào kho</p>
              </button>
            </div>
          </div>

          <!-- Step 2: Form Pre-Trip -->
          <div v-else-if="step === 'pretrip'">
            <div class="flex items-center gap-3 mb-6">
              <button @click="step = 'select'" class="w-8 h-8 rounded-full bg-slate-100 hover:bg-slate-200 flex items-center justify-center transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-slate-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" /></svg>
              </button>
              <h4 class="font-bold text-lg text-blue-700">Thực hiện Pre-Trip Checklist</h4>
            </div>
            <div class="space-y-4">
              <div v-for="(param, index) in preTripParams" :key="'pre'+param.id" class="p-4 border border-slate-200 rounded-xl bg-white shadow-sm">
                <div class="flex flex-col sm:flex-row justify-between items-start gap-4">
                  <div class="flex-1">
                    <h5 class="font-bold text-slate-800">{{ index + 1 }}. {{ param.noiDung }}</h5>
                    <p class="text-sm text-slate-500 mt-1 italic">{{ param.dieuKien }}</p>
                  </div>
                  <div class="flex items-center gap-2 shrink-0">
                    <label class="flex items-center gap-1 cursor-pointer px-3 py-1.5 rounded-md border" :class="preTripAnswers[param.id] === true ? 'bg-green-50 border-green-500 text-green-700' : 'bg-white border-slate-200 hover:bg-slate-50'">
                      <input type="radio" :name="'pre_' + param.id" :value="true" v-model="preTripAnswers[param.id]" class="hidden">
                      <span class="text-sm font-bold">Đạt</span>
                    </label>
                    <label class="flex items-center gap-1 cursor-pointer px-3 py-1.5 rounded-md border" :class="preTripAnswers[param.id] === false ? 'bg-red-50 border-red-500 text-red-700' : 'bg-white border-slate-200 hover:bg-slate-50'">
                      <input type="radio" :name="'pre_' + param.id" :value="false" v-model="preTripAnswers[param.id]" class="hidden">
                      <span class="text-sm font-bold">Không đạt</span>
                    </label>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Step 2: Form Vệ Sinh -->
          <div v-else-if="step === 'vesinh'">
            <div class="flex items-center gap-3 mb-6">
              <button @click="step = 'select'" class="w-8 h-8 rounded-full bg-slate-100 hover:bg-slate-200 flex items-center justify-center transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-slate-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" /></svg>
              </button>
              <h4 class="font-bold text-lg text-teal-700">Kiểm tra Vệ sinh xe</h4>
            </div>
            <div class="space-y-4">
              <div v-for="(param, index) in vesinhParams" :key="'vs'+param.id" class="p-4 border border-slate-200 rounded-xl bg-white shadow-sm">
                <div class="flex flex-col sm:flex-row justify-between items-start gap-4">
                  <div class="flex-1">
                    <h5 class="font-bold text-slate-800">{{ param.stt }}. {{ param.chiTiet }}</h5>
                    <p v-if="param.ghiChu" class="text-sm text-slate-500 mt-1 italic">{{ param.ghiChu }}</p>
                  </div>
                  <div class="flex items-center gap-2 shrink-0">
                    <label class="flex items-center gap-1 cursor-pointer px-3 py-1.5 rounded-md border" :class="vesinhAnswers[param.id] === true ? 'bg-teal-50 border-teal-500 text-teal-700' : 'bg-white border-slate-200 hover:bg-slate-50'">
                      <input type="radio" :name="'vs_' + param.id" :value="true" v-model="vesinhAnswers[param.id]" class="hidden">
                      <span class="text-sm font-bold">Có</span>
                    </label>
                    <label class="flex items-center gap-1 cursor-pointer px-3 py-1.5 rounded-md border" :class="vesinhAnswers[param.id] === false ? 'bg-red-50 border-red-500 text-red-700' : 'bg-white border-slate-200 hover:bg-slate-50'">
                      <input type="radio" :name="'vs_' + param.id" :value="false" v-model="vesinhAnswers[param.id]" class="hidden">
                      <span class="text-sm font-bold">Không</span>
                    </label>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Footer -->
      <div v-if="step !== 'select'" class="px-6 py-4 bg-slate-50 border-t border-slate-100 flex justify-end gap-3">
        <button @click="step = 'select'" class="px-6 py-2.5 bg-white border border-slate-300 text-slate-700 font-medium rounded-xl hover:bg-slate-50 transition-colors disabled:opacity-50" :disabled="submitting">
          Quay lại
        </button>
        <button @click="submit" class="px-6 py-2.5 text-white font-bold rounded-xl shadow-sm transition-colors flex items-center gap-2 disabled:bg-slate-300 disabled:cursor-not-allowed" :class="step === 'pretrip' ? 'bg-blue-600 hover:bg-blue-700' : 'bg-teal-600 hover:bg-teal-700'" :disabled="submitting || !isAllAnswered">
          <span v-if="submitting">Đang lưu...</span>
          <span v-else>Xác nhận Lưu</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'

const props = defineProps({
  show: Boolean,
  xe: Object
})

const emit = defineEmits(['close', 'success'])

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const step = ref('select') // select | pretrip | vesinh
const loading = ref(false)
const submitting = ref(false)

const preTripParams = ref([])
const vesinhParams = ref([])

const preTripAnswers = ref({})
const vesinhAnswers = ref({})

const startPreTrip = () => {
  if (props.xe?.baoVeDaKiemTraPreTripHomNay) return
  step.value = 'pretrip'
}

const startVeSinh = () => {
  step.value = 'vesinh'
}

const isAllAnswered = computed(() => {
  if (step.value === 'pretrip') {
    return preTripParams.value.length > 0 && preTripParams.value.every(p => preTripAnswers.value[p.id] !== undefined)
  } else if (step.value === 'vesinh') {
    return vesinhParams.value.length > 0 && vesinhParams.value.every(p => vesinhAnswers.value[p.id] !== undefined)
  }
  return false
})

const loadData = async () => {
  if (!props.xe) return
  
  loading.value = true
  step.value = 'select'
  preTripAnswers.value = {}
  vesinhAnswers.value = {}
  
  try {
    const [preRes, vsRes] = await Promise.all([
      $fetch(`${apiBase}/PreTripChecklistParams`),
      $fetch(`${apiBase}/KiemtravesinhxeParams`)
    ])
    
    preTripParams.value = preRes.filter(p => p.isActive).sort((a, b) => a.stt - b.stt)
    vesinhParams.value = vsRes.filter(p => p.isActive).sort((a, b) => a.stt - b.stt)
  } catch (err) {
    console.error(err)
    alert("Lỗi khi tải biểu mẫu!")
  } finally {
    loading.value = false
  }
}

watch(() => props.show, (newVal) => {
  if (newVal) loadData()
})

const close = () => {
  if (!submitting.value) emit('close')
}

const submit = async () => {
  if (!isAllAnswered.value) return
  
  // Validate
  if (step.value === 'pretrip') {
    const failedPre = preTripParams.value.some(p => preTripAnswers.value[p.id] === false)
    if (failedPre) {
      alert("Tất cả các mục Pre-Trip phải Đạt!")
      return
    }
  } else if (step.value === 'vesinh') {
    const failedVs = vesinhParams.value.some(p => vesinhAnswers.value[p.id] !== p.dapAnDat)
    if (failedVs) {
      alert("Xe không đạt tiêu chuẩn vệ sinh!")
      return
    }
  }
  
  submitting.value = true
  
  try {
    const payload = {
      sothe: props.xe.sothe
    }
    
    if (step.value === 'pretrip') {
      payload.checklistData = JSON.stringify(preTripParams.value.map(p => ({
        id: p.id,
        noiDung: p.noiDung,
        isPassed: preTripAnswers.value[p.id]
      })))
    } else if (step.value === 'vesinh') {
      payload.answers = Object.keys(vesinhAnswers.value).map(k => ({
        paramId: parseInt(k),
        ketQua: vesinhAnswers.value[k]
      }))
    }
    
    await $fetch(`${apiBase}/Danhsachxetrongkho/baove-kiemtra`, {
      method: 'POST',
      body: payload
    })
    
    alert('Lưu kết quả kiểm tra thành công!')
    emit('success')
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra khi lưu')
  } finally {
    submitting.value = false
  }
}
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 6px;
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

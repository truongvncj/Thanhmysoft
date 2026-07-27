<template>
  <div class="max-w-4xl mx-auto">
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-4 sm:p-8">
      <h1 class="text-xl sm:text-2xl font-bold text-slate-800 mb-1 sm:mb-6 text-center">PRE-TRIP CHECKLIST</h1>
      
      <div v-if="loading" class="text-center py-10">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-slate-500">Đang kiểm tra thông tin...</p>
      </div>

      <div v-else-if="state && state.daDangTai" class="text-center py-10 flex flex-col items-center">
        <div class="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center text-green-500 mb-6 shadow-sm">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-10 w-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
          </svg>
        </div>
        <h2 class="text-xl font-bold text-slate-800 mb-2">Bạn đã hoàn thành Pre-Trip Checklist hôm nay</h2>
        <p class="text-slate-500 mb-8 max-w-md">Vui lòng chuyển sang bước Kiểm tra vệ sinh xe trước khi vào kho.</p>
        
        <NuxtLink to="/driver/vesinh" class="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-8 rounded-xl shadow-lg hover:shadow-blue-500/30 transition-all transform hover:-translate-y-1">
          Chuyển đến Kiểm tra Vệ sinh xe
        </NuxtLink>
      </div>

      <div v-else class="pt-2 pb-4 sm:py-6">
        <div class="mb-4 sm:mb-6 text-center">
          <h2 class="text-lg sm:text-xl font-bold text-slate-800 mb-1 sm:mb-2">Bạn chưa làm Pre-Trip Checklist</h2>
          <p class="text-sm sm:text-base text-slate-500 mt-0">Vui lòng kiểm tra các mục dưới đây trước khi vào kho.</p>
        </div>

        <div v-if="loadingParams" class="flex justify-center py-8">
           <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        </div>

        <div v-else class="space-y-6 mb-8 max-h-[600px] overflow-y-auto pr-2 custom-scrollbar">
          <div v-for="(param, index) in checklistParams" :key="param.id" class="bg-slate-50 p-4 sm:p-5 md:p-6 rounded-xl border border-slate-100 hover:border-blue-200 transition-colors duration-300">
            <div class="flex flex-col md:flex-row gap-4 md:gap-6">
              <div class="flex-1">
                <div class="flex items-start gap-3 md:gap-4">
                  <span class="flex-shrink-0 w-7 h-7 md:w-8 md:h-8 bg-blue-100 text-blue-700 rounded-full flex items-center justify-center font-bold text-xs md:text-sm shadow-sm">{{ index + 1 }}</span>
                  <div>
                    <h4 class="text-slate-800 text-base md:text-lg leading-snug mt-0.5 font-medium">{{ param.dieuKien || param.noiDung }}</h4>
                  </div>
                </div>
              </div>
              
              <div class="md:w-72 space-y-4">
                <div class="flex gap-2 sm:gap-4">
                  <label class="flex-1 flex items-center justify-center gap-2 p-2 sm:p-3 rounded-lg sm:rounded-xl border-2 cursor-pointer transition-all duration-200"
                         :class="checklistAnswers[param.id] === true ? 'bg-blue-50 border-blue-500 text-blue-700 shadow-sm' : 'bg-white border-slate-200 hover:bg-slate-50 hover:border-blue-200'">
                    <input type="radio" :name="'check_' + param.id" :value="true" v-model="checklistAnswers[param.id]" class="hidden" />
                    <span class="font-bold text-sm sm:text-base">Đạt</span>
                  </label>
                  <label class="flex-1 flex items-center justify-center gap-2 p-2 sm:p-3 rounded-lg sm:rounded-xl border-2 cursor-pointer transition-all duration-200"
                         :class="checklistAnswers[param.id] === false ? 'bg-red-50 border-red-500 text-red-700 shadow-sm' : 'bg-white border-slate-200 hover:bg-slate-50 hover:border-red-200'">
                    <input type="radio" :name="'check_' + param.id" :value="false" v-model="checklistAnswers[param.id]" class="hidden" />
                    <span class="font-bold text-sm sm:text-base">Không đạt</span>
                  </label>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="mt-10 flex flex-col items-center">
          <button @click="submitDangTai" :disabled="!isAllAnswered" :class="isAllAnswered ? 'bg-blue-600 hover:bg-blue-700 shadow-lg shadow-blue-500/30 hover:-translate-y-1' : 'bg-slate-300 cursor-not-allowed'" class="text-white font-bold py-4 px-12 rounded-xl transition-all duration-300 flex items-center gap-2 text-lg">
            Xác nhận
            <svg v-if="isAllAnswered" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
          </button>
          <p v-if="!isAllAnswered" class="text-center text-red-500 mt-4 font-medium flex items-center gap-1">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd" /></svg>
            Vui lòng kiểm tra và đảm bảo Đạt tất cả các chỉ tiêu
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 6px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: #f1f5f9; 
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: #cbd5e1; 
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: #94a3b8; 
}
</style>

<script setup>
import { ref, onMounted, computed } from 'vue'

definePageMeta({
  layout: 'driver',
})

const state = ref(null)
const loading = ref(true)
const sothe = ref('')

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl
const checklistParams = ref([])
const loadingParams = ref(false)
const checklistAnswers = ref({})

const isAllAnswered = computed(() => {
  if (checklistParams.value.length === 0) return false
  return checklistParams.value.every(p => checklistAnswers.value[p.id] === true)
})

const fetchParams = async () => {
  loadingParams.value = true
  try {
    const res = await $fetch(`${apiBase}/PreTripChecklistParams`)
    checklistParams.value = res.filter(p => p.isActive).sort((a, b) => a.stt - b.stt)
  } catch (err) {
    console.error(err)
  } finally {
    loadingParams.value = false
  }
}

const fetchState = async () => {
  try {
    const res = await $fetch(`${apiBase}/DangTais/state?sothe=${sothe.value}`)
    state.value = res
    if (res.daDangTai) {
      useRouter().push('/driver/vesinh')
    }
  } catch (err) {
    console.error(err)
    alert("Có lỗi xảy ra khi lấy thông tin")
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  if (process.client) {
    sothe.value = localStorage.getItem('driver_sothe') || ''
    if (sothe.value) {
      fetchState()
      fetchParams()
    } else {
      useRouter().push('/')
    }
  }
})

const submitDangTai = async () => {
  if (!isAllAnswered.value) return

  try {
    loading.value = true

    const checklistData = checklistParams.value.map(p => ({
      id: p.id,
      noiDung: p.noiDung,
      isPassed: checklistAnswers.value[p.id]
    }))

    await $fetch(`${apiBase}/DangTais/submit-checklist`, {
      method: 'POST',
      body: { 
        sothe: sothe.value,
        checklistData: JSON.stringify(checklistData)
      }
    })
    alert('Đã xác nhận thành công!')
    useRouter().push('/driver/vesinh')
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra khi xác nhận.')
    loading.value = false
  }
}
</script>

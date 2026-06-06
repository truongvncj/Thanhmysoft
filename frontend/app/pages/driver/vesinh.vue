<template>
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="bg-white p-4 sm:p-6 md:p-8 rounded-2xl shadow-sm border border-slate-100">
      <h1 class="text-xl sm:text-2xl md:text-3xl font-bold text-slate-800 text-center mb-2 sm:mb-8 tracking-tight">KIỂM TRA VỆ SINH XE TẢI</h1>
      
      <div v-if="loading" class="text-center py-10">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-teal-600 mx-auto"></div>
        <p class="mt-4 text-slate-500 font-medium">Đang kiểm tra thông tin...</p>
      </div>

      <div v-else-if="!state?.daDangTai" class="text-center py-12">
        <div class="w-24 h-24 bg-amber-50 rounded-full flex items-center justify-center text-amber-500 mb-6 mx-auto shadow-inner">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
        </div>
        <h2 class="text-2xl font-bold text-slate-800 mb-3">Bạn chưa làm Pre-Trip Checklist</h2>
        <p class="text-slate-500 mb-8 max-w-md mx-auto text-lg">Vui lòng quay lại màn hình Pre-Trip để xác nhận an toàn trước khi kiểm tra vệ sinh xe.</p>
        
        <NuxtLink to="/driver" class="bg-amber-500 hover:bg-amber-600 text-white font-bold py-4 px-8 rounded-xl shadow-lg hover:shadow-amber-500/30 transition-all transform hover:-translate-y-1 inline-block">
          Quay lại màn hình Pre-Trip
        </NuxtLink>
      </div>

      <div v-else-if="sessionCompleted" class="text-center py-12">
        <div class="w-24 h-24 bg-teal-50 rounded-full flex items-center justify-center text-teal-500 mb-6 mx-auto shadow-inner">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </div>
        <h2 class="text-2xl font-bold text-slate-800 mb-3">Đã xác nhận Kiểm tra vệ sinh</h2>
        <p class="text-slate-500 mb-8 text-lg">Bạn đã hoàn tất thủ tục vệ sinh cho lượt này và có thể vào kho.</p>
      </div>

      <div v-else>
        <p class="text-slate-600 mb-4 sm:mb-8 text-center text-base sm:text-lg">Vui lòng kiểm tra thực tế xe và đánh giá các chỉ tiêu vệ sinh dưới đây.</p>
        
        <div v-if="loadingParams" class="text-center py-8 text-slate-500">Đang tải danh sách kiểm tra...</div>
        <div v-else-if="checklistParams.length === 0" class="text-center py-8 text-slate-500">Không có mục kiểm tra nào.</div>
        <div v-else>
          <div class="space-y-6">
            <div v-for="(param, index) in checklistParams" :key="param.id" class="bg-slate-50 p-4 sm:p-5 md:p-6 rounded-xl border border-slate-100 hover:border-teal-200 transition-colors duration-300">
              <div class="flex flex-col md:flex-row gap-4 md:gap-6">
                <div class="flex-1">
                  <div class="flex items-start gap-3 md:gap-4">
                    <span class="flex-shrink-0 w-7 h-7 md:w-8 md:h-8 bg-teal-100 text-teal-700 rounded-full flex items-center justify-center font-bold text-xs md:text-sm shadow-sm">{{ param.stt }}</span>
                    <div>
                      <h4 class="font-bold text-slate-800 text-base md:text-lg leading-snug">{{ param.chiTiet }}</h4>
                      <p v-if="param.ghiChu" class="text-slate-400 text-sm mt-1 md:mt-2 italic flex items-center gap-1">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd" /></svg>
                        {{ param.ghiChu }}
                      </p>
                    </div>
                  </div>
                </div>
                
                <div class="md:w-72 space-y-4">
                  <div class="flex gap-2 sm:gap-4">
                    <label class="flex-1 flex items-center justify-center gap-2 p-2 sm:p-3 rounded-lg sm:rounded-xl border-2 cursor-pointer transition-all duration-200"
                           :class="checklistAnswers[param.id] === true ? 'bg-teal-50 border-teal-500 text-teal-700 shadow-sm' : 'bg-white border-slate-200 hover:bg-slate-50 hover:border-teal-200'">
                      <input type="radio" :name="'check_' + param.id" :value="true" v-model="checklistAnswers[param.id]" class="hidden" />
                      <span class="font-bold text-sm sm:text-base">Có</span>
                    </label>
                    <label class="flex-1 flex items-center justify-center gap-2 p-2 sm:p-3 rounded-lg sm:rounded-xl border-2 cursor-pointer transition-all duration-200"
                           :class="checklistAnswers[param.id] === false ? 'bg-red-50 border-red-500 text-red-700 shadow-sm' : 'bg-white border-slate-200 hover:bg-slate-50 hover:border-red-200'">
                      <input type="radio" :name="'check_' + param.id" :value="false" v-model="checklistAnswers[param.id]" class="hidden" />
                      <span class="font-bold text-sm sm:text-base">Không</span>
                    </label>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="mt-10 flex flex-col items-center">
            <button @click="handlePreSubmit" :disabled="!isAllAnswered || submitting" :class="isAllAnswered ? 'bg-teal-600 hover:bg-teal-700 shadow-lg shadow-teal-500/30 hover:-translate-y-1' : 'bg-slate-300 cursor-not-allowed'" class="text-white font-bold py-4 px-12 rounded-xl transition-all duration-300 flex items-center gap-2 text-lg">
              <span v-if="submitting">Đang xử lý...</span>
              <span v-else>Xác nhận Đạt & Lưu</span>
              <svg v-if="!submitting" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            </button>
            <p v-if="!isAllAnswered" class="text-center text-red-500 mt-4 font-medium flex items-center gap-1">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd" /></svg>
              Vui lòng đánh giá (Có/Không) cho tất cả các chỉ tiêu.
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Thông Báo / Xác Nhận Custom -->
    <div v-if="showModal" class="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/60 backdrop-blur-sm px-4 transition-opacity">
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-md overflow-hidden transform transition-all border border-slate-100">
        <!-- Header -->
        <div class="px-6 py-5 border-b border-slate-100 flex items-center gap-3" :class="modalType === 'success' ? 'bg-teal-50' : (modalType === 'error' ? 'bg-red-50' : 'bg-blue-50')">
          <div v-if="modalType === 'success'" class="w-10 h-10 bg-teal-100 text-teal-600 rounded-full flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
          </div>
          <div v-else-if="modalType === 'error'" class="w-10 h-10 bg-red-100 text-red-600 rounded-full flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
          </div>
          <div v-else class="w-10 h-10 bg-blue-100 text-blue-600 rounded-full flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
          </div>
          <h3 class="text-xl font-bold" :class="modalType === 'success' ? 'text-teal-800' : (modalType === 'error' ? 'text-red-800' : 'text-blue-800')">
            {{ modalType === 'confirm' ? 'Xác nhận gửi' : (modalType === 'success' ? 'Thành công' : 'Thông báo') }}
          </h3>
        </div>
        
        <!-- Body -->
        <div class="px-6 py-8">
          <p class="text-slate-700 text-center text-lg whitespace-pre-line">{{ modalMessage }}</p>
        </div>
        
        <!-- Footer -->
        <div class="px-6 py-4 bg-slate-50 flex justify-end gap-3 border-t border-slate-100">
          <button v-if="modalType === 'confirm'" @click="closeModal" class="px-6 py-2.5 bg-white border border-slate-300 text-slate-700 font-medium rounded-xl hover:bg-slate-50 transition-colors">
            Hủy bỏ
          </button>
          <button v-if="modalType === 'confirm'" @click="submitVeSinh" class="px-6 py-2.5 bg-teal-600 text-white font-bold rounded-xl hover:bg-teal-700 shadow-sm transition-colors">
            Đồng ý gửi
          </button>
          <button v-if="modalType !== 'confirm'" @click="closeModal" class="px-8 py-2.5 bg-slate-800 text-white font-bold rounded-xl hover:bg-slate-900 shadow-sm transition-colors w-full sm:w-auto">
            Đóng
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'

definePageMeta({
  layout: 'driver',
  title: 'Kiểm tra Vệ sinh Xe'
})

const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const state = ref(null)
const loading = ref(true)
const sothe = ref('')
const checklistParams = ref([])
const loadingParams = ref(false)
const checklistAnswers = ref({})
const validationErrors = ref([])
const submitting = ref(false)
const sessionCompleted = ref(false)

const showModal = ref(false)
const modalMessage = ref('')
const modalType = ref('info') // 'info', 'error', 'success', 'confirm'

const openModal = (message, type = 'info') => {
  modalMessage.value = message
  modalType.value = type
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
  if (modalType.value === 'success') {
    localStorage.clear()
    router.push('/')
  }
}

const isAllAnswered = computed(() => {
  if (checklistParams.value.length === 0) return false
  return checklistParams.value.every(p => checklistAnswers.value[p.id] !== undefined)
})

const fetchParams = async () => {
  try {
    loadingParams.value = true
    const res = await $fetch(`${apiBase}/KiemtravesinhxeParams`)
    checklistParams.value = res || []
  } catch (err) {
    console.error("Lỗi khi tải tham số kiểm tra:", err)
    openModal("Không thể tải danh sách câu hỏi. Vui lòng thử lại sau.", "error")
  } finally {
    loadingParams.value = false
  }
}

const fetchState = async () => {
  try {
    const res = await $fetch(`${apiBase}/DangTais/state?sothe=${sothe.value}`)
    state.value = res
  } catch (err) {
    console.error(err)
    openModal("Có lỗi xảy ra khi lấy trạng thái đăng tài", "error")
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
      router.push('/')
    }
  }
})

// Tiền xử lý: Validate các câu trả lời trực tiếp trên Frontend trước khi gọi API
const handlePreSubmit = () => {
  validationErrors.value = []

  for (const param of checklistParams.value) {
    if (checklistAnswers.value[param.id] !== param.dapAnDat) {
      validationErrors.value.push(`- ${param.chiTiet}`)
    }
  }

  if (validationErrors.value.length > 0) {
    openModal("Xe không đạt tiêu chuẩn vệ sinh ở các mục sau:\n\n" + validationErrors.value.join("\n") + "\n\nVui lòng khắc phục và kiểm tra lại!", "error")
    return
  }

  openModal("Bạn có chắc chắn muốn xác nhận đã kiểm tra thực tế xe và hoàn tất thủ tục?", "confirm")
}

const submitVeSinh = async () => {
  closeModal() // Close confirm modal
  try {
    submitting.value = true
    
    const answersData = Object.keys(checklistAnswers.value).map(key => ({
      paramId: parseInt(key),
      ketQua: checklistAnswers.value[key]
    }))

    await $fetch(`${apiBase}/DangTais/vesinh`, {
      method: 'POST',
      body: { 
        sothe: sothe.value,
        answers: answersData
      }
    })
    
    // Submit chứng từ
    const chungtusStr = localStorage.getItem('driver_chungtus')
    let chungtusData = []
    if (chungtusStr) {
      try {
        chungtusData = JSON.parse(chungtusStr)
      } catch (e) {}
    }
    
    if (chungtusData.length > 0) {
      await $fetch(`${apiBase}/DangTais/submit-chungtu`, {
        method: 'POST',
        body: {
          sothe: sothe.value,
          chungTus: chungtusData
        }
      })
    }
    
    await fetchState()
    
    // Đánh dấu session hiện tại là đã hoàn thành
    sessionCompleted.value = true
    
    // Xoá trắng cho lần đăng nhập sau
    checklistAnswers.value = {}
    validationErrors.value = []
    
    openModal('Đăng ký xe vào kho thành công! Xe đã được đưa vào bãi chờ.', 'success')
  } catch (err) {
    openModal(err.response?._data?.message || 'Có lỗi xảy ra khi lưu dữ liệu.', 'error')
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="max-w-xl mx-auto">
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-4 sm:p-8">
      <h1 class="text-xl sm:text-2xl font-bold text-slate-800 mb-6 text-center">Đăng ký & Chứng từ</h1>
      
      <div v-if="loading" class="text-center py-10">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-slate-500">Đang kiểm tra thông tin...</p>
      </div>

      <div v-else class="space-y-5">
        <div>
          <label class="block text-sm font-semibold text-slate-700 mb-1">Kho hàng</label>
          <select v-model="driverKhohangId" class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none">
            <option value="" disabled>{{ activeKhohangs.length > 0 ? '-- Chọn kho --' : '-- Chưa có kho nào hoạt động --' }}</option>
            <option v-for="kho in activeKhohangs" :key="kho.id" :value="kho.id">{{ kho.tenKho }}</option>
          </select>
        </div>
        
        <div>
          <label class="block text-sm font-semibold text-slate-700 mb-1">Biển số xe</label>
          <input v-model="driverBienSo" type="text" class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none uppercase" placeholder="Ví dụ: 29A-123.45">
        </div>
        
        <div>
          <label class="block text-sm font-semibold text-slate-700 mb-2">Lý do</label>
          <div class="flex gap-6 mb-2">
            <label class="flex items-center gap-2 cursor-pointer">
              <input type="radio" v-model="driverLyDo" value="Nhập hàng" class="text-blue-600 w-5 h-5">
              <span class="text-base font-medium text-slate-700">Nhập hàng</span>
            </label>
            <label class="flex items-center gap-2 cursor-pointer">
              <input type="radio" v-model="driverLyDo" value="Xuất hàng" class="text-blue-600 w-5 h-5">
              <span class="text-base font-medium text-slate-700">Xuất hàng</span>
            </label>
          </div>
        </div>
        
        <!-- STO / Shipment Fields -->
        <div v-if="driverLyDo === 'Nhập hàng'" class="space-y-4 bg-slate-50 p-4 rounded-xl border border-slate-200">
          <label class="block text-xs font-semibold text-slate-600 uppercase">Chứng từ Nhập hàng</label>
          <div v-for="(item, index) in nhapHangItems" :key="index" class="flex flex-col sm:flex-row gap-3 relative group pb-2">
            <div class="flex-1">
              <input v-model="item.soTransferOut" type="text" class="w-full px-3 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số Transfer Out">
            </div>
            <div class="flex-1">
              <input v-model="item.soSTO" type="text" class="w-full px-3 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số STO">
            </div>
            <button @click="removeNhapHangItem(index)" v-if="nhapHangItems.length > 1" class="text-red-500 hover:bg-red-100 p-2.5 rounded-lg transition-colors self-end sm:self-auto">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
            </button>
          </div>
          <button @click="addNhapHangItem" class="w-full py-2.5 border border-dashed border-blue-400 text-blue-600 text-sm font-medium rounded-lg hover:bg-blue-50 transition-colors">
            + Thêm cặp Transfer Out & STO
          </button>
        </div>

        <div v-if="driverLyDo === 'Xuất hàng'" class="space-y-4 bg-slate-50 p-4 rounded-xl border border-slate-200">
          <label class="block text-xs font-semibold text-slate-600 uppercase">Chứng từ Xuất hàng</label>
          <div v-for="(item, index) in xuatHangItems" :key="index" class="flex gap-3 relative group pb-2">
            <div class="flex-1">
              <input v-model="item.soShipment" type="text" class="w-full px-3 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Số Shipment">
            </div>
            <button @click="removeXuatHangItem(index)" v-if="xuatHangItems.length > 1" class="text-red-500 hover:bg-red-100 p-2.5 rounded-lg transition-colors">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
            </button>
          </div>
            <button @click="addXuatHangItem" class="w-full py-2.5 border border-dashed border-blue-400 text-blue-600 text-sm font-medium rounded-lg hover:bg-blue-50 transition-colors">
            + Thêm Số Shipment
          </button>
        </div>
        
        <div class="pt-4">
          <button @click="submitDangKy" class="w-full py-3.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-colors shadow-md text-base font-bold flex justify-center items-center gap-2" :disabled="!driverKhohangId || !driverLyDo || !driverBienSo || submitting">
            <span v-if="submitting" class="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></span>
            {{ submitting ? 'Đang xử lý...' : 'Hoàn tất Đăng ký' }}
          </button>
        </div>
      </div>
    </div>
    
    <!-- Modal -->
    <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm p-4">
      <div class="bg-white rounded-2xl shadow-xl w-full max-w-sm p-6 transform transition-all text-center">
        <div class="w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4"
             :class="{
               'bg-green-100 text-green-500': modalType === 'success',
               'bg-red-100 text-red-500': modalType === 'error'
             }">
          <svg v-if="modalType === 'success'" xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
          </svg>
          <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </div>
        <h3 class="text-lg font-bold text-slate-800 mb-2">{{ modalType === 'success' ? 'Thành công' : 'Lỗi' }}</h3>
        <p class="text-slate-600 mb-6 whitespace-pre-line">{{ modalMessage }}</p>
        <button @click="closeModal" class="w-full py-2.5 bg-slate-900 text-white rounded-xl hover:bg-slate-800 transition-colors font-semibold">
          Đóng
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useRuntimeConfig } from '#app'

definePageMeta({
  layout: 'driver'
})

const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const sothe = ref('')
const loading = ref(true)
const submitting = ref(false)

const activeKhohangs = ref([])
const driverKhohangId = ref('')
const driverBienSo = ref('')
const driverLyDo = ref('')

const nhapHangItems = ref([{ soTransferOut: '', soSTO: '' }])
const addNhapHangItem = () => nhapHangItems.value.push({ soTransferOut: '', soSTO: '' })
const removeNhapHangItem = (index) => nhapHangItems.value.splice(index, 1)

const xuatHangItems = ref([{ soShipment: '' }])
const addXuatHangItem = () => xuatHangItems.value.push({ soShipment: '' })
const removeXuatHangItem = (index) => xuatHangItems.value.splice(index, 1)

const showModal = ref(false)
const modalMessage = ref('')
const modalType = ref('success')

const fetchActiveKhohangs = async () => {
  try {
    const res = await $fetch(`${apiBase}/Khohangs?onlyOpen=true`)
    activeKhohangs.value = res || []
  } catch (err) {
    console.error('Lỗi tải danh sách kho', err)
  }
}

onMounted(() => {
  if (process.client) {
    sothe.value = localStorage.getItem('driver_sothe') || ''
    if (sothe.value) {
      fetchActiveKhohangs()
      loading.value = false
    } else {
      router.push('/')
    }
  }
})

const openModal = (message, type) => {
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

const submitDangKy = async () => {
  if (!driverKhohangId.value || !driverLyDo.value || !driverBienSo.value) return

  let chungtusData = []
  if (driverLyDo.value === 'Nhập hàng') {
    chungtusData = nhapHangItems.value
      .filter(item => item.soTransferOut || item.soSTO)
      .map(item => ({ ...item, lyDo: 'Nhập hàng' }))
  } else if (driverLyDo.value === 'Xuất hàng') {
    chungtusData = xuatHangItems.value
      .filter(item => item.soShipment)
      .map(item => ({ ...item, lyDo: 'Xuất hàng' }))
  }

  if (chungtusData.length === 0) {
    openModal('Vui lòng nhập ít nhất một số chứng từ (STO / Shipment) trước khi đăng ký.', 'error')
    return
  }

  try {
    submitting.value = true
    
    // 1. Đăng tài
    await $fetch(`${apiBase}/DangTais/dangtai`, {
      method: 'POST',
      body: {
        sothe: sothe.value,
        khohangId: parseInt(driverKhohangId.value),
        lyDo: driverLyDo.value,
        bienSo: driverBienSo.value
      }
    })

    // 2. Chứng từ
    await $fetch(`${apiBase}/DangTais/submit-chungtu`, {
      method: 'POST',
      body: {
        sothe: sothe.value,
        chungTus: chungtusData
      }
    })

    openModal('Đăng ký xe vào kho thành công! Xe đã được đưa vào bãi chờ.', 'success')
  } catch (err) {
    openModal(err.response?._data?.message || 'Có lỗi xảy ra khi lưu dữ liệu.', 'error')
  } finally {
    submitting.value = false
  }
}
</script>

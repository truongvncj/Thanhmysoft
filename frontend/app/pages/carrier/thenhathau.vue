<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h2 class="text-lg font-semibold text-gray-800">Quản lý Thẻ Nhà Thầu</h2>
      <button @click="openModal()" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
        </svg>
        Đăng ký thẻ mới
      </button>
    </div>

    <!-- Table -->
    <div class="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
      <div v-if="pending" class="p-8 text-center text-gray-500">
        Đang tải dữ liệu...
      </div>
      <div v-else-if="error" class="p-8 text-center text-red-500">
        Lỗi khi tải dữ liệu thẻ nhà thầu.
      </div>
      <div v-else>
        <!-- Mobile View: Cards -->
        <div class="grid grid-cols-1 gap-4 p-4 lg:hidden">
          <div v-for="item in thenhathaus" :key="'mobile-'+item.maLx" class="bg-white rounded-xl shadow-sm border border-slate-200 p-4 flex flex-col gap-3 relative">
            <div class="flex justify-between items-start">
              <div>
                <h3 class="font-bold text-lg text-slate-800">{{ item.ten }}</h3>
                <p class="text-sm text-slate-500 mt-0.5">{{ item.sothe }}</p>
              </div>
              <div class="flex gap-2">
                <button @click="openModal(item)" class="text-blue-600 bg-blue-50 p-2 rounded-lg hover:bg-blue-100">Sửa</button>
                <button @click="deleteThenhathau(item.maLx)" class="text-red-600 bg-red-50 p-2 rounded-lg hover:bg-red-100">Xóa</button>
              </div>
            </div>
            <div class="space-y-1 mt-2 text-sm">
              <div class="flex justify-between">
                <span class="text-slate-500">Hãng vận tải:</span>
                <span class="font-medium text-slate-800">{{ item.tenNvt || tenNvt }}</span>
              </div>
              <div class="flex justify-between">
                <span class="text-slate-500">Thời hạn:</span>
                <span class="font-medium text-slate-800">{{ new Date(item.thoihanthe).toLocaleDateString('vi-VN') }}</span>
              </div>
            </div>
          </div>
          <div v-if="thenhathaus && thenhathaus.length === 0" class="text-center py-8 text-gray-500 border border-dashed border-gray-200 rounded-xl">
            Chưa có thẻ nhà thầu nào được đăng ký
          </div>
        </div>

        <!-- Desktop View: Table -->
        <div class="overflow-x-auto hidden lg:block">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Họ & Tên</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Hãng vận tải</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Số Thẻ</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Thời hạn</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="item in thenhathaus" :key="item.maLx" class="hover:bg-gray-50 transition-colors">
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">{{ item.ten }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ item.tenNvt || tenNvt }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ item.sothe }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {{ new Date(item.thoihanthe).toLocaleDateString('vi-VN') }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button @click="openModal(item)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
                  <button @click="deleteThenhathau(item.maLx)" class="text-red-600 hover:text-red-900">Xóa</button>
                </td>
              </tr>
              <tr v-if="thenhathaus && thenhathaus.length === 0">
                <td colspan="5" class="px-6 py-8 text-center text-gray-500 border-t border-dashed">Chưa có thẻ nhà thầu nào được đăng ký</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity p-4">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-lg p-6 transform transition-all max-h-[90vh] overflow-y-auto">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ isEdit ? 'Cập nhật' : 'Đăng ký' }} Thẻ Nhà Thầu</h3>
        <form @submit.prevent="saveThenhathau" class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Họ & Tên</label>
              <input v-model="form.ten" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="Tên tài xế..." />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Hãng vận tải</label>
              <input :value="tenNvt" type="text" disabled class="w-full px-3 py-2 border border-gray-300 rounded-lg bg-gray-100 text-gray-500 outline-none cursor-not-allowed" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Số Thẻ</label>
              <input v-model="form.sothe" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="Số thẻ..." />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Thời Hạn Thẻ</label>
              <input v-model="form.thoihanthe" type="date" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" />
            </div>
          </div>
          
          <div class="flex justify-end gap-3 mt-8 pt-4 border-t border-gray-100">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm" :disabled="isSaving">
              {{ isSaving ? 'Đang lưu...' : 'Lưu dữ liệu' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'

definePageMeta({
  layout: 'carrier',
  title: 'Thẻ Nhà Thầu'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const maNvt = ref('')
const tenNvt = ref('')
const thenhathaus = ref([])
const pending = ref(true)
const error = ref(null)

const fetchData = async () => {
  pending.value = true
  error.value = null
  try {
    const res = await $fetch(`${apiBase}/Thenhathaus?maNvt=${maNvt.value}`)
    thenhathaus.value = res
  } catch (err) {
    error.value = err
  } finally {
    pending.value = false
  }
}

onMounted(() => {
  if (process.client) {
    maNvt.value = localStorage.getItem('carrier_maNvt') || ''
    tenNvt.value = localStorage.getItem('carrier_tenNvt') || ''
    if (maNvt.value) {
      fetchData()
    } else {
      error.value = new Error("Chưa đăng nhập Nhà Vận Tải")
      pending.value = false
    }
  }
})

const isModalOpen = ref(false)
const isSaving = ref(false)
const isEdit = ref(false)
const form = reactive({
  maLx: '',
  maNvt: '',
  ten: '',
  cccd: '',
  sodt: '',
  sothe: '',
  thoihanthe: ''
})

const openModal = (item = null) => {
  if (item) {
    isEdit.value = true
    form.maLx = item.maLx
    form.maNvt = item.maNvt
    form.ten = item.ten
    form.cccd = item.cccd
    form.sodt = item.sodt
    form.sothe = item.sothe
    form.thoihanthe = new Date(item.thoihanthe).toISOString().split('T')[0]
  } else {
    isEdit.value = false
    form.maLx = ''
    form.maNvt = maNvt.value
    form.ten = ''
    form.cccd = ''
    form.sodt = ''
    form.sothe = ''
    form.thoihanthe = ''
  }
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
}

const saveThenhathau = async () => {
  isSaving.value = true
  try {
    // Ensure maNvt is always the current logged in user
    form.maNvt = maNvt.value
    
    // Parse Date for C# datetime
    const payload = { ...form, thoihanthe: new Date(form.thoihanthe).toISOString() }
    
    if (isEdit.value) {
      await $fetch(`${apiBase}/Thenhathaus/${form.maLx}`, {
        method: 'PUT',
        body: payload
      })
    } else {
      await $fetch(`${apiBase}/Thenhathaus`, {
        method: 'POST',
        body: payload
      })
    }
    await fetchData()
    closeModal()
  } catch (err) {
    alert('Có lỗi xảy ra: ' + (err.response?._data?.message || err.message))
    console.error(err)
  } finally {
    isSaving.value = false
  }
}

const deleteThenhathau = async (id) => {
  if (!confirm('Bạn có chắc chắn muốn xóa thẻ này?')) return
  
  try {
    await $fetch(`${apiBase}/Thenhathaus/${id}`, {
      method: 'DELETE'
    })
    await fetchData()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa thẻ')
    console.error(err)
  }
}
</script>

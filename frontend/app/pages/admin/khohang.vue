<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto space-y-6">
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Quản lý Kho hàng</h2>
          <p class="text-slate-500 mt-1">Danh sách thông tin các kho hàng</p>
        </div>
        <div class="flex items-center gap-4">
          <button @click="openModal()" class="px-4 py-2 bg-orange-600 text-white rounded-xl hover:bg-orange-700 transition-colors shadow-sm flex items-center gap-2 font-semibold">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Thêm kho hàng
          </button>
          <NuxtLink to="/admin" class="flex items-center gap-2 text-blue-600 bg-blue-50 px-4 py-2 rounded-xl font-semibold hover:bg-blue-100 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
            </svg>
            Quay lại Admin
          </NuxtLink>
        </div>
      </div>

    <!-- Table -->
    <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
      <div v-if="loading" class="p-8 text-center text-gray-500">
        Đang tải dữ liệu...
      </div>
      <div v-else>
        <!-- Mobile View: Cards -->
        <div class="grid grid-cols-1 gap-4 p-4 lg:hidden">
          <div v-for="item in khohangs" :key="'mob-'+item.id" class="bg-white rounded-xl shadow-sm border border-slate-200 p-4 flex flex-col gap-3 relative">
            <div class="flex justify-between items-start">
              <div>
                <h3 class="font-bold text-lg text-gray-900">{{ item.tenKho }}</h3>
                <p class="text-sm font-medium text-slate-500">ID: {{ item.id }}</p>
              </div>
              <div class="flex gap-2">
                <button @click="openModal(item)" class="text-blue-600 bg-blue-50 p-2 rounded-lg hover:bg-blue-100 border border-blue-100">Sửa</button>
              </div>
            </div>
            <div class="space-y-2 mt-2 text-sm">
              <div class="flex justify-between items-center">
                <span class="text-gray-500">Trạng thái:</span>
                <span v-if="item.isClosed" class="px-2 py-1 bg-red-100 text-red-700 rounded-lg text-xs font-semibold">Đã đóng</span>
                <span v-else class="px-2 py-1 bg-green-100 text-green-700 rounded-lg text-xs font-semibold">Đang hoạt động</span>
              </div>
            </div>
            <div class="pt-3 border-t border-slate-100 mt-2 flex justify-end">
              <button @click="toggleStatus(item)" class="w-full py-2 rounded-lg font-semibold text-sm transition-colors border text-center" :class="item.isClosed ? 'bg-green-50 text-green-600 border-green-200 hover:bg-green-500 hover:text-white' : 'bg-orange-50 text-orange-600 border-orange-200 hover:bg-orange-500 hover:text-white'">
                {{ item.isClosed ? 'Mở kho' : 'Đóng kho' }}
              </button>
            </div>
          </div>
          <div v-if="khohangs.length === 0" class="text-center py-8 text-gray-500 border border-dashed border-gray-200 rounded-xl">
            Chưa có kho hàng nào
          </div>
        </div>

        <!-- Desktop View: Table -->
        <div class="overflow-x-auto hidden lg:block">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tên Kho</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Trạng thái</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="item in khohangs" :key="'desk-'+item.id" class="hover:bg-gray-50 transition-colors">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-bold text-gray-900">{{ item.id }}</td>
                <td class="px-6 py-4 text-sm font-medium text-gray-900">{{ item.tenKho }}</td>
                <td class="px-6 py-4 text-sm text-gray-500">
                    <span v-if="item.isClosed" class="px-2 py-1 bg-red-100 text-red-700 rounded-lg text-xs font-semibold">Đã đóng</span>
                    <span v-else class="px-2 py-1 bg-green-100 text-green-700 rounded-lg text-xs font-semibold">Đang hoạt động</span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button @click="openModal(item)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
                  <button @click="toggleStatus(item)" :class="item.isClosed ? 'text-green-600 hover:text-green-900' : 'text-orange-600 hover:text-orange-900'">
                    {{ item.isClosed ? 'Mở kho' : 'Đóng kho' }}
                  </button>
                </td>
              </tr>
              <tr v-if="khohangs.length === 0">
                <td colspan="4" class="px-6 py-8 text-center text-gray-500">Chưa có kho hàng nào</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-lg p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ isEdit ? 'Cập nhật' : 'Thêm mới' }} Kho hàng</h3>
        <form @submit.prevent="saveItem" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tên Kho</label>
            <input v-model="form.tenKho" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition-shadow" placeholder="Nhập tên kho..." />
          </div>

          <div class="flex justify-end gap-3 mt-6">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700 transition-colors shadow-sm" :disabled="isSaving">
              {{ isSaving ? 'Đang lưu...' : 'Lưu lại' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'

definePageMeta({
  layout: false,
  title: 'Quản lý Kho hàng'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const khohangs = ref([])
const loading = ref(true)

const isModalOpen = ref(false)
const isSaving = ref(false)
const isEdit = ref(false)
const form = reactive({
  id: 0,
  tenKho: '',
  isClosed: false
})

const fetchList = async () => {
  loading.value = true
  try {
    const res = await $fetch(`${apiBase}/Khohangs`)
    let data = res || []
    data.sort((a, b) => a.id - b.id)
    khohangs.value = data
  } catch (err) {
    console.error(err)
    khohangs.value = []
  } finally {
    loading.value = false
  }
}

const openModal = (item = null) => {
  if (item) {
    isEdit.value = true
    form.id = item.id
    form.tenKho = item.tenKho
    form.isClosed = item.isClosed
  } else {
    isEdit.value = false
    form.id = 0
    form.tenKho = ''
    form.isClosed = false
  }
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
}

const saveItem = async () => {
  isSaving.value = true
  try {
    if (isEdit.value) {
      await $fetch(`${apiBase}/Khohangs/${form.id}`, {
        method: 'PUT',
        body: form
      })
    } else {
      await $fetch(`${apiBase}/Khohangs`, {
        method: 'POST',
        body: form
      })
    }
    await fetchList()
    closeModal()
  } catch (err) {
    alert(err.response?._data?.message || 'Có lỗi xảy ra khi lưu')
    console.error(err)
  } finally {
    isSaving.value = false
  }
}



const toggleStatus = async (item) => {
  try {
    const updatedItem = { ...item, isClosed: !item.isClosed }
    await $fetch(`${apiBase}/Khohangs/${item.id}`, {
      method: 'PUT',
      body: updatedItem
    })
    await fetchList()
  } catch (err) {
    alert('Có lỗi xảy ra khi cập nhật trạng thái')
    console.error(err)
  }
}

onMounted(() => {
  fetchList()
})
</script>

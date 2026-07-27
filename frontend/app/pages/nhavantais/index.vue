<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto space-y-6">
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Danh sách Nhà Vận Tải</h2>
        </div>
        <div class="flex items-center gap-4">
          <button @click="openModal()" class="px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-colors shadow-sm flex items-center gap-2 font-semibold">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Thêm mới
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
      <div v-if="pending" class="p-8 text-center text-gray-500">
        Đang tải dữ liệu...
      </div>
      <div v-else-if="error" class="p-8 text-center text-red-500">
        Lỗi khi tải dữ liệu. Hãy kiểm tra kết nối với Backend.
      </div>
      <table v-else class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Mã NVT</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tên Nhà Vận Tải</th>
            <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="nvt in nhaVanTais" :key="nvt.maNvt" class="hover:bg-gray-50 transition-colors">
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ nvt.maNvt }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ nvt.tenNvt }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <button @click="openModal(nvt)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
              <button @click="deleteNhaVanTai(nvt.maNvt)" class="text-red-600 hover:text-red-900">Xóa</button>
            </td>
          </tr>
          <tr v-if="nhaVanTais && nhaVanTais.length === 0">
            <td colspan="3" class="px-6 py-8 text-center text-gray-500">Chưa có nhà vận tải nào</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-md p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ isEdit ? 'Cập nhật' : 'Thêm mới' }} Nhà Vận Tải</h3>
        <form @submit.prevent="saveNhaVanTai" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Mã NVT</label>
            <input v-model="form.maNvt" type="text" required :disabled="isEdit" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow disabled:bg-gray-100 disabled:text-gray-500" placeholder="Mã..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tên NVT</label>
            <input v-model="form.tenNvt" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="Tên nhà vận tải..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Mật khẩu</label>
            <input v-model="form.password" type="password" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="Mật khẩu..." />
          </div>
          <div class="flex justify-end gap-3 mt-6">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm" :disabled="isSaving">
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
import { ref, reactive } from 'vue'

definePageMeta({
  layout: false,
  title: 'Quản lý Nhà Vận Tải'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const { data: nhaVanTais, pending, error, refresh } = await useFetch(`${apiBase}/NhaVanTais`)

const isModalOpen = ref(false)
const isSaving = ref(false)
const isEdit = ref(false)
const form = reactive({
  maNvt: '',
  tenNvt: '',
  password: ''
})

const openModal = async (nvt = null) => {
  if (nvt) {
    isEdit.value = true
    form.maNvt = nvt.maNvt
    form.tenNvt = nvt.tenNvt
    form.password = nvt.password
  } else {
    isEdit.value = false
    form.maNvt = ''
    form.tenNvt = ''
    form.password = ''
  }
  isModalOpen.value = true
}

const closeModal = async () => {
  isModalOpen.value = false
}

const saveNhaVanTai = async () => {
  isSaving.value = true
  try {
    if (isEdit.value) {
      await $fetch(`${apiBase}/NhaVanTais/${form.maNvt}`, {
        method: 'PUT',
        body: form
      })
    } else {
      await $fetch(`${apiBase}/NhaVanTais`, {
        method: 'POST',
        body: form
      })
    }
    await refresh()
    closeModal()
  } catch (err) {
    alert('Có lỗi xảy ra khi lưu nhà vận tải')
    console.error(err)
  } finally {
    isSaving.value = false
  }
}

const deleteNhaVanTai = async (id) => {
  if (!await confirm('Bạn có chắc chắn muốn xóa nhà vận tải này?')) return
  
  try {
    await $fetch(`${apiBase}/NhaVanTais/${id}`, {
      method: 'DELETE'
    })
    await refresh()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa nhà vận tải')
    console.error(err)
  }
}
</script>

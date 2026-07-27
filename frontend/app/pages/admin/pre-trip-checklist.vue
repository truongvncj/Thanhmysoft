<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto space-y-6">
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Quản lý Thông số Pre_TripChecklist</h2>
        </div>
        <div class="flex items-center gap-4">
          <button @click="openModal()" class="px-4 py-2 bg-purple-600 text-white rounded-xl hover:bg-purple-700 transition-colors shadow-sm flex items-center gap-2 font-semibold">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Thêm thông số
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
      <div v-else>
        <!-- Mobile View: Cards -->
        <div class="grid grid-cols-1 gap-4 p-4 lg:hidden">
          <div v-for="item in parameters" :key="'mob-'+item.id" class="bg-white rounded-xl shadow-sm border border-slate-200 p-4 flex flex-col gap-3 relative">
            <div class="flex justify-between items-start">
              <div class="flex gap-3">
                <div class="w-8 h-8 rounded-full bg-purple-100 text-purple-700 flex items-center justify-center font-bold shrink-0">
                  {{ item.stt }}
                </div>
                <div>
                  <h3 class="font-bold text-gray-900 leading-tight">{{ item.noiDung }}</h3>
                </div>
              </div>
            </div>
            <div class="mt-2 text-sm text-gray-600 bg-slate-50 p-3 rounded-lg border border-slate-100">
              <span class="font-semibold block mb-1">Điều kiện:</span>
              <p>{{ item.dieuKien }}</p>
            </div>
            <div class="pt-3 border-t border-slate-100 mt-2 flex justify-end gap-2">
              <button @click="openModal(item)" class="px-4 py-2 text-blue-600 bg-blue-50 rounded-lg hover:bg-blue-100 font-semibold border border-blue-100 w-full text-center">Sửa</button>
              <button @click="deleteParam(item.id)" class="px-4 py-2 text-red-600 bg-red-50 rounded-lg hover:bg-red-100 font-semibold border border-red-100 w-full text-center">Xóa</button>
            </div>
          </div>
          <div v-if="parameters && parameters.length === 0" class="text-center py-8 text-gray-500 border border-dashed border-gray-200 rounded-xl">
            Chưa có thông số nào
          </div>
        </div>

        <!-- Desktop View: Table -->
        <div class="overflow-x-auto hidden lg:block">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider w-16">STT</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nội dung</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Điều kiện</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="item in parameters" :key="'desk-'+item.id" class="hover:bg-gray-50 transition-colors">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ item.stt }}</td>
                <td class="px-6 py-4 text-sm font-medium text-gray-900">{{ item.noiDung }}</td>
                <td class="px-6 py-4 text-sm text-gray-500">{{ item.dieuKien }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button @click="openModal(item)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
                  <button @click="deleteParam(item.id)" class="text-red-600 hover:text-red-900">Xóa</button>
                </td>
              </tr>
              <tr v-if="parameters && parameters.length === 0">
                <td colspan="4" class="px-6 py-8 text-center text-gray-500">Chưa có thông số nào</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-lg p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ isEdit ? 'Cập nhật' : 'Thêm mới' }} Thông số</h3>
        <form @submit.prevent="saveParam" class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div class="col-span-1">
              <label class="block text-sm font-medium text-gray-700 mb-1">STT</label>
              <input v-model.number="form.stt" type="number" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 outline-none transition-shadow" placeholder="STT..." />
            </div>
            <div class="col-span-3">
              <label class="block text-sm font-medium text-gray-700 mb-1">Nội dung</label>
              <input v-model="form.noiDung" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 outline-none transition-shadow" placeholder="Ví dụ: Hồ sơ và Nhân viên lái xe" />
            </div>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Điều kiện</label>
            <textarea v-model="form.dieuKien" required rows="3" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 outline-none transition-shadow" placeholder="Ví dụ: Bằng lái xe phù hợp..."></textarea>
          </div>
          <div class="flex justify-end gap-3 mt-6">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition-colors shadow-sm" :disabled="isSaving">
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
  title: 'Thông số Pre-Trip'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const { data: parameters, pending, error, refresh } = await useFetch(`${apiBase}/PreTripChecklistParams`)

const isModalOpen = ref(false)
const isSaving = ref(false)
const isEdit = ref(false)
const form = reactive({
  id: 0,
  noiDung: '',
  stt: 1,
  dieuKien: '',
  isActive: true
})

const openModal = async (item = null) => {
  if (item) {
    isEdit.value = true
    form.id = item.id
    form.noiDung = item.noiDung
    form.stt = item.stt
    form.dieuKien = item.dieuKien
    form.isActive = item.isActive
  } else {
    isEdit.value = false
    form.id = 0
    form.noiDung = ''
    form.stt = (parameters.value?.length || 0) + 1
    form.dieuKien = ''
    form.isActive = true
  }
  isModalOpen.value = true
}

const closeModal = async () => {
  isModalOpen.value = false
}

const saveParam = async () => {
  // Check for duplicate STT
  if (parameters.value) {
    const isDuplicate = parameters.value.some(p => p.stt === form.stt && p.id !== form.id)
    if (isDuplicate) {
      alert(`Số thứ tự ${form.stt} đã được sử dụng. Vui lòng chọn một số khác!`)
      return
    }
  }

  isSaving.value = true
  try {
    if (isEdit.value) {
      await $fetch(`${apiBase}/PreTripChecklistParams/${form.id}`, {
        method: 'PUT',
        body: form
      })
    } else {
      await $fetch(`${apiBase}/PreTripChecklistParams`, {
        method: 'POST',
        body: form
      })
    }
    await refresh()
    closeModal()
  } catch (err) {
    alert('Có lỗi xảy ra khi lưu thông số')
    console.error(err)
  } finally {
    isSaving.value = false
  }
}

const deleteParam = async (id) => {
  if (!await confirm('Bạn có chắc chắn muốn xóa thông số này?')) return
  
  try {
    await $fetch(`${apiBase}/PreTripChecklistParams/${id}`, {
      method: 'DELETE'
    })
    await refresh()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa thông số')
    console.error(err)
  }
}
</script>

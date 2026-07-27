<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto space-y-6">
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Quản lý Nhân Viên</h2>
          <p class="text-slate-500 mt-1">Danh sách thông tin nhân viên</p>
        </div>
        <div class="flex items-center gap-4">
          <button @click="openModal()" class="px-4 py-2 bg-amber-600 text-white rounded-xl hover:bg-amber-700 transition-colors shadow-sm flex items-center gap-2 font-semibold">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Thêm nhân viên
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
          <div v-for="item in nhanviens" :key="'mob-'+item.mnv" class="bg-white rounded-xl shadow-sm border border-slate-200 p-4 flex flex-col gap-3 relative">
            <div class="flex justify-between items-start">
              <div>
                <h3 class="font-bold text-lg text-gray-900">{{ item.tnv }}</h3>
                <p class="text-sm font-medium text-amber-600">{{ item.mnv }}</p>
              </div>
              <div class="flex gap-2">
                <button @click="openModal(item)" class="text-blue-600 bg-blue-50 p-2 rounded-lg hover:bg-blue-100">Sửa</button>
                <button @click="deleteItem(item.mnv)" class="text-red-600 bg-red-50 p-2 rounded-lg hover:bg-red-100">Xóa</button>
              </div>
            </div>
            <div class="space-y-1 mt-2 text-sm">
              <div class="flex justify-between">
                <span class="text-gray-500">Chức danh:</span>
                <span class="font-medium text-gray-800">{{ item.chucdanh }}</span>
              </div>
              <div class="flex justify-between items-center">
                <span class="text-gray-500">Mật khẩu:</span>
                <div class="flex items-center gap-2">
                  <span class="font-mono bg-gray-100 px-2 py-1 rounded text-gray-700">{{ item.password || '---' }}</span>
                  <button v-if="item.password" @click="copyPassword(item.password)" class="text-gray-400 hover:text-blue-600 transition-colors">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
                    </svg>
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div v-if="nhanviens.length === 0" class="text-center py-8 text-gray-500 border border-dashed border-gray-200 rounded-xl">
            Chưa có nhân viên nào
          </div>
        </div>

        <!-- Desktop View: Table -->
        <div class="overflow-x-auto hidden lg:block">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Mã NV</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tên nhân viên</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Chức danh</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Mật khẩu</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="item in nhanviens" :key="item.mnv" class="hover:bg-gray-50 transition-colors">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-bold text-gray-900">{{ item.mnv }}</td>
                <td class="px-6 py-4 text-sm font-medium text-gray-900">{{ item.tnv }}</td>
                <td class="px-6 py-4 text-sm text-gray-500">{{ item.chucdanh }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  <div class="flex items-center gap-2">
                    <span class="font-mono bg-gray-100 px-2 py-1 rounded">{{ item.password || '---' }}</span>
                    <button v-if="item.password" @click="copyPassword(item.password)" class="text-gray-400 hover:text-blue-600 transition-colors" title="Copy mật khẩu">
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
                      </svg>
                    </button>
                  </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button @click="openModal(item)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
                  <button @click="deleteItem(item.mnv)" class="text-red-600 hover:text-red-900">Xóa</button>
                </td>
              </tr>
              <tr v-if="nhanviens.length === 0">
                <td colspan="5" class="px-6 py-8 text-center text-gray-500">Chưa có nhân viên nào</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-lg p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ isEdit ? 'Cập nhật' : 'Thêm mới' }} Nhân Viên</h3>
        <form @submit.prevent="saveItem" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Mã nhân viên (MNV)</label>
            <input v-model="form.mnv" type="text" required :disabled="isEdit" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-500 focus:border-amber-500 outline-none transition-shadow disabled:bg-gray-100" placeholder="Nhập mã nhân viên..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Mật khẩu</label>
            <input v-model="form.password" type="text" class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-500 focus:border-amber-500 outline-none transition-shadow" placeholder="Mật khẩu đăng nhập..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tên nhân viên (TNV)</label>
            <input v-model="form.tnv" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-500 focus:border-amber-500 outline-none transition-shadow" placeholder="Nhập tên nhân viên..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Chức danh</label>
            <select v-model="form.chucdanh" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-500 focus:border-amber-500 outline-none transition-shadow bg-white">
              <option value="" disabled>-- Chọn chức danh --</option>
              <option value="Thủ kho">Thủ kho</option>
              <option value="Bảo vệ">Bảo vệ</option>
              <option value="Quản lý">Quản lý / Giám đốc</option>
              <option value="Nhân viên">Nhân viên khác</option>
            </select>
          </div>
          <div class="flex justify-end gap-3 mt-6">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 transition-colors shadow-sm" :disabled="isSaving">
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
  title: 'Quản lý Nhân Viên'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const nhanviens = ref([])
const loading = ref(true)

const isModalOpen = ref(false)
const isSaving = ref(false)
const isEdit = ref(false)
const form = reactive({
  mnv: '',
  tnv: '',
  chucdanh: '',
  vitri: 'Nhân viên',
  password: '',
  permissions: ''
})

const fetchList = async () => {
  loading.value = true
  try {
    const res = await $fetch(`${apiBase}/Nhanviens`)
    nhanviens.value = res || []
  } catch (err) {
    console.error(err)
    nhanviens.value = []
  } finally {
    loading.value = false
  }
}

const openModal = async (item = null) => {
  if (item) {
    isEdit.value = true
    form.mnv = item.mnv
    form.tnv = item.tnv
    form.chucdanh = item.chucdanh
    form.password = item.password || ''
    form.permissions = item.permissions || ''
  } else {
    isEdit.value = false
    form.mnv = ''
    form.tnv = ''
    form.chucdanh = ''
    form.password = ''
    form.permissions = ''
  }
  form.vitri = 'Nhân viên'
  isModalOpen.value = true
}

const closeModal = async () => {
  isModalOpen.value = false
}

const saveItem = async () => {
  isSaving.value = true
  try {
    // Tự động cấp quyền mặc định khi thêm mới
    if (!isEdit.value && !form.permissions) {
      if (form.chucdanh === 'Thủ kho') {
        form.permissions = 'Dashboard,ThuKho'
      } else if (form.chucdanh === 'Bảo vệ') {
        form.permissions = 'Dashboard,BaoVe'
      } else if (form.chucdanh === 'Quản lý') {
        form.permissions = 'Dashboard,ThuKho,BaoVe,Admin_NhanVien,Admin_PhanQuyen,Admin_KhoHang,Admin_BieuMau'
      }
    }

    if (isEdit.value) {
      await $fetch(`${apiBase}/Nhanviens/${form.mnv}`, {
        method: 'PUT',
        body: form
      })
    } else {
      await $fetch(`${apiBase}/Nhanviens`, {
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

const deleteItem = async (id) => {
  if (!await confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) return
  
  try {
    await $fetch(`${apiBase}/Nhanviens/${id}`, {
      method: 'DELETE'
    })
    await fetchList()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa')
    console.error(err)
  }
}

const copyPassword = async (text) => {
  try {
    await navigator.clipboard.writeText(text)
    alert('Đã copy mật khẩu!')
  } catch (err) {
    console.error('Failed to copy', err)
  }
}

onMounted(() => {
  fetchList()
})
</script>

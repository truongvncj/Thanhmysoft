<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h2 class="text-lg font-semibold text-gray-800">Danh sách Đơn hàng</h2>
      <button @click="openModal()" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
        </svg>
        Tạo Đơn hàng
      </button>
    </div>

    <!-- Table -->
    <div class="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
      <div v-if="pending" class="p-8 text-center text-gray-500">
        Đang tải dữ liệu...
      </div>
      <div v-else-if="error" class="p-8 text-center text-red-500">
        Lỗi khi tải dữ liệu đơn hàng. Hãy kiểm tra kết nối với Backend.
      </div>
      <table v-else class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Mã ĐH</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Khách hàng</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tổng tiền</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ngày tạo</th>
            <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="order in orders" :key="order.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">#{{ order.id }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ order.customerName }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-green-600">${{ order.totalAmount.toFixed(2) }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ new Date(order.orderDate).toLocaleString() }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <button @click="deleteOrder(order.id)" class="text-red-600 hover:text-red-900">Xóa</button>
            </td>
          </tr>
          <tr v-if="orders.length === 0">
            <td colspan="5" class="px-6 py-8 text-center text-gray-500">Chưa có đơn hàng nào</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-md p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">Tạo Đơn hàng mới</h3>
        <form @submit.prevent="saveOrder" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tên khách hàng</label>
            <input v-model="form.customerName" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="Nguyễn Văn A..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tổng tiền ($)</label>
            <input v-model.number="form.totalAmount" type="number" step="0.01" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="0.00" />
          </div>
          <div class="flex justify-end gap-3 mt-6">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">Hủy</button>
            <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm" :disabled="isSaving">
              {{ isSaving ? 'Đang lưu...' : 'Tạo Đơn hàng' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'

definePageMeta({
  title: 'Đơn hàng'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const { data: orders, pending, error, refresh } = await useFetch(`${apiBase}/Orders`)

const isModalOpen = ref(false)
const isSaving = ref(false)
const form = reactive({
  customerName: '',
  totalAmount: 0
})

const openModal = async () => {
  form.customerName = ''
  form.totalAmount = 0
  isModalOpen.value = true
}

const closeModal = async () => {
  isModalOpen.value = false
}

const saveOrder = async () => {
  isSaving.value = true
  try {
    await $fetch(`${apiBase}/Orders`, {
      method: 'POST',
      body: form
    })
    await refresh()
    closeModal()
  } catch (err) {
    alert('Có lỗi xảy ra khi tạo đơn hàng')
    console.error(err)
  } finally {
    isSaving.value = false
  }
}

const deleteOrder = async (id) => {
  if (!await confirm('Bạn có chắc chắn muốn xóa đơn hàng này?')) return
  
  try {
    await $fetch(`${apiBase}/Orders/${id}`, {
      method: 'DELETE'
    })
    await refresh()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa đơn hàng')
    console.error(err)
  }
}
</script>

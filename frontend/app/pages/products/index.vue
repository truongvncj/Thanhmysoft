<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h2 class="text-lg font-semibold text-gray-800">Danh sách Sản phẩm</h2>
      <button @click="openModal()" class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
        </svg>
        Thêm Sản phẩm
      </button>
    </div>

    <!-- Table -->
    <div class="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
      <div v-if="pending" class="p-8 text-center text-gray-500">
        Đang tải dữ liệu...
      </div>
      <div v-else-if="error" class="p-8 text-center text-red-500">
        Lỗi khi tải dữ liệu sản phẩm. Hãy kiểm tra kết nối với Backend.
      </div>
      <table v-else class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tên sản phẩm</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Giá</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tồn kho</th>
            <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="product in products" :key="product.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">#{{ product.id }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ product.name }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${{ product.price.toFixed(2) }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
              <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-green-100 text-green-800">
                {{ product.stockQuantity }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <button @click="openModal(product)" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
              <button @click="deleteProduct(product.id)" class="text-red-600 hover:text-red-900">Xóa</button>
            </td>
          </tr>
          <tr v-if="products.length === 0">
            <td colspan="5" class="px-6 py-8 text-center text-gray-500">Chưa có sản phẩm nào</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm transition-opacity">
      <div class="bg-white rounded-xl shadow-xl w-full max-w-md p-6 transform transition-all">
        <h3 class="text-lg font-bold text-gray-900 mb-4">{{ editingProduct.id ? 'Sửa Sản phẩm' : 'Thêm Sản phẩm mới' }}</h3>
        <form @submit.prevent="saveProduct" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tên sản phẩm</label>
            <input v-model="form.name" type="text" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="VD: Bàn phím cơ..." />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Giá ($)</label>
            <input v-model.number="form.price" type="number" step="0.01" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="0.00" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Số lượng tồn kho</label>
            <input v-model.number="form.stockQuantity" type="number" required class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-shadow" placeholder="0" />
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
</template>

<script setup>
import { ref, reactive } from 'vue'

definePageMeta({
  title: 'Sản phẩm'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const { data: products, pending, error, refresh } = await useFetch(`${apiBase}/Products`)

const isModalOpen = ref(false)
const isSaving = ref(false)
const editingProduct = ref({})
const form = reactive({
  name: '',
  price: 0,
  stockQuantity: 0
})

const openModal = async (product = null) => {
  if (product) {
    editingProduct.value = product
    form.name = product.name
    form.price = product.price
    form.stockQuantity = product.stockQuantity
  } else {
    editingProduct.value = {}
    form.name = ''
    form.price = 0
    form.stockQuantity = 0
  }
  isModalOpen.value = true
}

const closeModal = async () => {
  isModalOpen.value = false
}

const saveProduct = async () => {
  isSaving.value = true
  try {
    if (editingProduct.value.id) {
      // Update
      await $fetch(`${apiBase}/Products/${editingProduct.value.id}`, {
        method: 'PUT',
        body: { id: editingProduct.value.id, ...form }
      })
    } else {
      // Create
      await $fetch(`${apiBase}/Products`, {
        method: 'POST',
        body: form
      })
    }
    await refresh()
    closeModal()
  } catch (err) {
    alert('Có lỗi xảy ra khi lưu sản phẩm')
    console.error(err)
  } finally {
    isSaving.value = false
  }
}

const deleteProduct = async (id) => {
  if (!await confirm('Bạn có chắc chắn muốn xóa sản phẩm này?')) return
  
  try {
    await $fetch(`${apiBase}/Products/${id}`, {
      method: 'DELETE'
    })
    await refresh()
  } catch (err) {
    alert('Có lỗi xảy ra khi xóa sản phẩm')
    console.error(err)
  }
}
</script>

<template>
  <div class="min-h-screen bg-slate-50 p-6 flex flex-col">
    <!-- Header -->
    <div class="flex justify-between items-center mb-6">
      <div>
        <h1 class="text-2xl font-bold text-slate-800">Sơ Đồ Kho Hàng</h1>
        <p class="text-slate-500 mt-1">Thiết kế và sắp xếp sơ đồ các VỊ TRÍ trong kho</p>
      </div>
      <NuxtLink to="/admin" class="flex items-center gap-2 text-indigo-600 bg-indigo-50 px-4 py-2 rounded-xl font-semibold hover:bg-indigo-100 transition-colors">
        Quay lại Admin
      </NuxtLink>
    </div>

    <!-- Main Content: 2 columns -->
    <div class="flex-1 flex gap-6 min-h-0">
      
      <!-- Left Column: Form & Tools -->
      <div class="w-80 bg-white rounded-2xl shadow-sm border border-slate-100 p-6 flex flex-col flex-shrink-0">
        <h3 class="text-lg font-bold text-slate-700 mb-4">Cài đặt Layout</h3>
        
        <div class="mb-6">
          <label class="block text-sm font-semibold text-slate-700 mb-2">Chọn Kho</label>
          <select v-model="selectedKhohangId" @change="fetchLayout" class="w-full px-4 py-2 bg-slate-50 border border-slate-200 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:outline-none">
            <option value="">-- Vui lòng chọn kho --</option>
            <option v-for="kho in khohangs" :key="kho.id" :value="kho.id">{{ kho.tenKho }}</option>
          </select>
        </div>

        <hr class="border-slate-100 mb-6" />

        <div v-if="selectedKhohangId" class="flex-1 overflow-y-auto pr-2">
          <div class="bg-white p-5 rounded-2xl shadow-sm border border-slate-200">
            <h4 class="font-semibold text-slate-700 mb-4">Tạo VỊ TRÍ mới</h4>
            <form @submit.prevent="createLocal" class="space-y-4">
              <div>
                <label class="block text-sm text-slate-600 mb-1">VỊ TRÍ <span class="text-red-500">*</span></label>
                <input v-model="form.maLocal" required type="text" class="w-full px-3 py-2 border border-slate-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none">
              </div>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="block text-sm text-slate-600 mb-1">Dãy hàng</label>
                  <input v-model="form.hang" type="text" class="w-full px-3 py-2 border border-slate-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none">
                </div>
                <div>
                  <label class="block text-sm text-slate-600 mb-1">STT Dãy Hàng</label>
                  <input v-model="form.cot" type="text" class="w-full px-3 py-2 border border-slate-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none">
                </div>
              </div>
              <div>
                <label class="block text-sm text-slate-600 mb-1">Ghi chú</label>
                <textarea v-model="form.ghiChu" rows="2" class="w-full px-3 py-2 border border-slate-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"></textarea>
              </div>
              <button type="submit" :disabled="creating" class="w-full bg-indigo-600 text-white font-medium py-2 rounded-lg hover:bg-indigo-700 transition-colors disabled:opacity-50">
                {{ creating ? 'Đang tạo...' : 'Tạo VỊ TRÍ' }}
              </button>
            </form>
          </div>

          <hr class="border-slate-100 my-6" />

          <h4 class="font-semibold text-slate-700 mb-4">Các Ký hiệu Khác</h4>
          <div class="space-y-3">
            <button @click="createLine" type="button" class="w-full bg-slate-800 hover:bg-slate-900 text-white font-bold py-2 rounded-lg shadow-sm transition-colors flex items-center justify-center gap-2">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 12h16"></path></svg>
              Tạo Đường thẳng
            </button>
            <button @click="createText" type="button" class="w-full bg-white border-2 border-slate-200 hover:border-slate-300 text-slate-700 font-bold py-2 rounded-lg shadow-sm transition-colors flex items-center justify-center gap-2">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h7"></path></svg>
              Thêm Text
            </button>
          </div>
        </div>
        <div v-else class="text-center text-slate-400 mt-10">
          Vui lòng chọn kho để bắt đầu
        </div>
      </div>

      <!-- Right Column: Canvas Board -->
      <div class="flex-1 bg-slate-200 rounded-2xl shadow-inner border border-slate-300 relative overflow-auto" ref="boardRef">
        <div v-if="!selectedKhohangId" class="absolute inset-0 flex items-center justify-center text-slate-400 font-medium">
          Sơ đồ Kho sẽ hiển thị tại đây
        </div>
        <div v-else ref="canvasRef" class="relative w-[3000px] h-[3000px] bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAiIGhlaWdodD0iMjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGNpcmNsZSBjeD0iMiIgY3k9IjIiIHI9IjEiIGZpbGw9IiNjYmQ1ZTEiLz48L3N2Zz4=')]">
          <!-- Loading State -->
          <div v-if="loading" class="absolute inset-0 flex items-center justify-center bg-white/50 z-50">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
          </div>

          <!-- Layout Items -->
          <div 
            v-for="item in layoutItems" 
            :key="item.id"
            :class="[
              'absolute flex items-center justify-center cursor-move group select-none transition-shadow',
              (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? 'bg-slate-800 shadow-sm border-0' : 
              item.elementType === 'text' ? `bg-transparent text-slate-800 shadow-none border-0 ring-0 outline-none leading-none ${item.hang === 'bold' ? 'font-bold' : 'font-normal'}` : 
              'bg-white border-2 border-indigo-500 shadow-md rounded hover:shadow-lg'
            ]"
            :style="{ 
              left: item.positionX + 'px', 
              top: item.positionY + 'px',
              width: item.width + 'px',
              height: item.height + 'px',
              borderRadius: (item.borderRadius || 0) + 'px',
              transform: `rotate(${item.rotation || 0}deg)`,
              transformOrigin: (item.elementType === 'line' || item.maLocal.startsWith('LINE_')) ? '0% 50%' : '50% 50%',
              fontSize: item.elementType === 'text' ? (item.height * 0.6) + 'px' : undefined,
              zIndex: activeItem?.id === item.id ? 10 : 1
            }"
            @mousedown.stop="startDrag(item, $event)"
            @dblclick.stop="toggleBold(item)"
          >
            <!-- Content -->
            <div class="text-center w-full px-2 overflow-hidden" v-if="(!item.elementType && !item.maLocal.startsWith('LINE_')) || item.elementType === 'local'">
              <div class="font-bold text-slate-800 truncate" :title="item.maLocal">{{ item.maLocal }}</div>
              <div class="text-[10px] text-slate-500 truncate" v-if="item.hang || item.cot">H:{{item.hang}} - C:{{item.cot}}</div>
            </div>
            <div class="w-full h-full flex items-center justify-center overflow-hidden" v-else-if="item.elementType === 'text'">
              {{ item.maLocal }}
            </div>

            <!-- Curve Button (Visible on hover) -->
            <button 
              @click.stop="toggleCurve(item)" 
              class="absolute -top-3 -left-3 bg-blue-500 text-white w-6 h-6 rounded-full opacity-0 group-hover:opacity-100 flex items-center justify-center shadow hover:bg-blue-600 transition-opacity"
              title="Bo góc / Cong"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 8a3 3 0 013-3h8a3 3 0 013 3v8a3 3 0 01-3 3H8a3 3 0 01-3-3V8z"></path></svg>
            </button>

            <!-- Rotate Handle (Visible on hover) -->
            <div 
              @mousedown.stop="startRotate(item, $event)"
              class="absolute top-1/2 -right-3 w-6 h-6 -translate-y-1/2 cursor-crosshair bg-emerald-500 rounded-full opacity-50 group-hover:opacity-100 flex items-center justify-center text-white shadow-lg z-20"
              title="Kéo để xoay"
            >
              <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15"></path></svg>
            </div>

            <!-- Delete Button (Visible on hover) -->
            <button 
              @click.stop="deleteLocal(item.id)" 
              class="absolute -top-2 -right-2 bg-red-500 text-white w-6 h-6 rounded-full flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity z-20 hover:bg-red-600 shadow-md"
              title="Xóa VỊ TRÍ"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
            </button>

            <!-- Resize Handle -->
            <div 
              class="absolute bottom-0 right-0 w-4 h-4 cursor-se-resize bg-indigo-500 rounded-tl-full opacity-50 group-hover:opacity-100"
              @mousedown.stop="startResize(item, $event)"
            ></div>
            
            <!-- Tooltip info -->
            <div class="absolute bottom-full left-1/2 -translate-x-1/2 mb-2 w-48 bg-gray-900 text-white text-xs rounded py-1 px-2 opacity-0 group-hover:opacity-100 pointer-events-none z-20 shadow-lg text-center transition-opacity" v-if="!item.elementType || item.elementType === 'local'">
              <p class="font-bold">{{ item.maLocal }}</p>
              <p v-if="item.hang">Dãy hàng: {{ item.hang }}</p>
              <p v-if="item.cot">STT Dãy Hàng: {{ item.cot }}</p>
              <p v-if="item.ghiChu" class="mt-1 border-t border-gray-700 pt-1">{{ item.ghiChu }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

definePageMeta({
  layout: false,
  title: 'Layout Kho'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const khohangs = ref([])
const selectedKhohangId = ref('')
const layoutItems = ref([])
const loading = ref(false)
const creating = ref(false)

const form = ref({
  maLocal: '',
  hang: '',
  cot: '',
  ghiChu: ''
})

const boardRef = ref(null)
const canvasRef = ref(null)

// --- DRAG & RESIZE STATE ---
const activeItem = ref(null)
const actionType = ref(null) // 'drag' or 'resize'
const startMouseX = ref(0)
const startMouseY = ref(0)
const startItemX = ref(0)
const startItemY = ref(0)
const startItemW = ref(0)
const startItemH = ref(0)

const fetchKhohangs = async () => {
  try {
    const res = await $fetch(`${apiBase}/Khohangs`)
    khohangs.value = res || []
  } catch (err) {
    console.error(err)
  }
}

const fetchLayout = async () => {
  if (!selectedKhohangId.value) {
    layoutItems.value = []
    return
  }
  try {
    loading.value = true
    const res = await $fetch(`${apiBase}/Layoutkhos?khohangId=${selectedKhohangId.value}`)
    layoutItems.value = res || []
  } catch (err) {
    console.error(err)
    alert('Lỗi khi tải layout')
  } finally {
    loading.value = false
  }
}

const createLocal = async () => {
  if (!selectedKhohangId.value || !form.value.maLocal) return
  
  try {
    creating.value = true
    
    // Find the center of the current visible board area for initial placement
    let initialX = 50
    let initialY = 50
    if (boardRef.value) {
      initialX = boardRef.value.scrollLeft + 50
      initialY = boardRef.value.scrollTop + 50
    }

    const payload = {
      khohangId: selectedKhohangId.value,
      maLocal: form.value.maLocal,
      hang: form.value.hang,
      cot: form.value.cot,
      ghiChu: form.value.ghiChu,
      positionX: initialX,
      positionY: initialY,
      width: 120,
      height: 60
    }

    const res = await $fetch(`${apiBase}/Layoutkhos`, {
      method: 'POST',
      body: payload
    })

    layoutItems.value.push(res)
    
    // Reset form
    form.value.maLocal = ''
    form.value.hang = ''
    form.value.cot = ''
    form.value.ghiChu = ''
  } catch (err) {
    console.error(err)
    alert('Lỗi tạo local')
  } finally {
    creating.value = false
  }
}

const createLine = async () => {
  if (!selectedKhohangId.value) return
  try {
    let initialX = 50
    let initialY = 50
    if (boardRef.value) {
      initialX = boardRef.value.scrollLeft + 50
      initialY = boardRef.value.scrollTop + 50
    }
    const payload = {
      khohangId: selectedKhohangId.value,
      maLocal: 'LINE_' + Date.now(),
      elementType: 'line',
      positionX: initialX,
      positionY: initialY,
      width: 200,
      height: 4,
      borderRadius: 0,
      rotation: 0
    }
    const res = await $fetch(`${apiBase}/Layoutkhos`, { method: 'POST', body: payload })
    layoutItems.value.push(res)
  } catch (err) {
    alert('Lỗi tạo đường thẳng')
  }
}

const createText = async () => {
  if (!selectedKhohangId.value) return
  const textContent = prompt('Nhập nội dung chữ muốn hiển thị:')
  if (!textContent) return
  
  try {
    let initialX = 50
    let initialY = 50
    if (boardRef.value) {
      initialX = boardRef.value.scrollLeft + 50
      initialY = boardRef.value.scrollTop + 50
    }
    const payload = {
      khohangId: selectedKhohangId.value,
      maLocal: textContent,
      elementType: 'text',
      positionX: initialX,
      positionY: initialY,
      width: 150,
      height: 40,
      borderRadius: 0,
      rotation: 0
    }
    const res = await $fetch(`${apiBase}/Layoutkhos`, { method: 'POST', body: payload })
    layoutItems.value.push(res)
  } catch (err) {
    alert('Lỗi thêm text')
  }
}

const toggleCurve = async (item) => {
  item.borderRadius = item.borderRadius > 0 ? 0 : 50
  try {
    await $fetch(`${apiBase}/Layoutkhos/${item.id}`, { method: 'PUT', body: item })
  } catch (err) {
    alert('Lỗi lưu bo cong')
  }
}

const toggleBold = async (item) => {
  if (item.elementType !== 'text') return
  item.hang = item.hang === 'bold' ? '' : 'bold'
  try {
    await $fetch(`${apiBase}/Layoutkhos/${item.id}`, { method: 'PUT', body: item })
  } catch (err) {
    alert('Lỗi lưu định dạng chữ')
  }
}

const deleteLocal = async (id) => {
  if (!confirm('Bạn có chắc muốn xóa Local này?')) return
  try {
    await $fetch(`${apiBase}/Layoutkhos/${id}`, { method: 'DELETE' })
    layoutItems.value = layoutItems.value.filter(i => i.id !== id)
  } catch (err) {
    alert('Lỗi khi xóa local')
  }
}

// --- DRAG LOGIC ---
const startDrag = (item, e) => {
  if (e.button !== 0) return // Only left click
  activeItem.value = item
  actionType.value = 'drag'
  startMouseX.value = e.clientX
  startMouseY.value = e.clientY
  startItemX.value = item.positionX
  startItemY.value = item.positionY
}

// --- RESIZE LOGIC ---
const startResize = (item, e) => {
  if (e.button !== 0) return
  activeItem.value = item
  actionType.value = 'resize'
  startMouseX.value = e.clientX
  startMouseY.value = e.clientY
  startItemW.value = item.width
  startItemH.value = item.height
}

const startRotate = (item, e) => {
  if (e.button !== 0) return
  activeItem.value = item
  actionType.value = 'rotate'
  
  if (canvasRef.value) {
    const rect = canvasRef.value.getBoundingClientRect()
    // Tọa độ tâm xoay (trái-giữa của item)
    startItemX.value = rect.left + item.positionX
    startItemY.value = rect.top + item.positionY + (item.height / 2)
  }
}

const onMouseMove = (e) => {
  if (!activeItem.value) return
  
  const dx = e.clientX - startMouseX.value
  const dy = e.clientY - startMouseY.value

  if (actionType.value === 'drag') {
    // Cập nhật vị trí tạm thời
    let newX = startItemX.value + dx
    let newY = startItemY.value + dy
    
    // Không cho ra ngoài lề trái/trên
    if (newX < 0) newX = 0
    if (newY < 0) newY = 0
    
    activeItem.value.positionX = newX
    activeItem.value.positionY = newY
  } 
  else if (actionType.value === 'resize') {
    let newW = startItemW.value + dx
    let newH = startItemH.value + dy
    
    // Giới hạn kích thước tối thiểu
    let minW = 60
    let minH = 40
    if (activeItem.value.elementType === 'text' || activeItem.value.elementType === 'line' || activeItem.value.maLocal.startsWith('LINE_')) {
      minW = 5
      minH = 5
    }
    
    if (newW < minW) newW = minW
    if (newH < minH) newH = minH
    
    activeItem.value.width = newW
    activeItem.value.height = newH
  } 
  else if (actionType.value === 'rotate') {
    const dx = e.clientX - startItemX.value
    const dy = e.clientY - startItemY.value
    let angle = Math.atan2(dy, dx) * (180 / Math.PI)
    activeItem.value.rotation = Math.round(angle)
    
    // Update width based on mouse distance from origin (min length 20)
    const distance = Math.sqrt(dx * dx + dy * dy)
    activeItem.value.width = Math.max(20, Math.round(distance))
  }
}

const onMouseUp = async () => {
  if (!activeItem.value) return
  
  const itemToSave = activeItem.value
  activeItem.value = null
  actionType.value = null
  
  // Lưu tọa độ/kích thước mới lên server
  try {
    await $fetch(`${apiBase}/Layoutkhos/${itemToSave.id}`, {
      method: 'PUT',
      body: itemToSave
    })
  } catch (err) {
    console.error('Lỗi khi lưu vị trí', err)
    // Optional: reload to revert state
  }
}

onMounted(() => {
  fetchKhohangs()
  window.addEventListener('mousemove', onMouseMove)
  window.addEventListener('mouseup', onMouseUp)
})

onUnmounted(() => {
  window.removeEventListener('mousemove', onMouseMove)
  window.removeEventListener('mouseup', onMouseUp)
})
</script>

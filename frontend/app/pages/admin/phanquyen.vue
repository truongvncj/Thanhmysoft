<template>
  <div class="min-h-screen bg-slate-50 p-6">
    <div class="max-w-7xl mx-auto space-y-6">
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h2 class="text-2xl font-bold text-gray-800">Phân quyền Hệ thống</h2>
          <p class="text-slate-500 mt-1">Cấp quyền truy cập các màn hình chức năng cho Nhân viên</p>
        </div>
        <div class="flex items-center gap-4">
          <NuxtLink to="/admin" class="flex items-center gap-2 text-indigo-600 bg-indigo-50 px-4 py-2 rounded-xl font-semibold hover:bg-indigo-100 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
            </svg>
            Quay lại Admin
          </NuxtLink>
        </div>
      </div>

      <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
        <div v-if="loading" class="p-8 text-center text-gray-500">
          Đang tải dữ liệu...
        </div>
        <div v-else>
          <!-- Mobile View: Cards -->
          <div class="grid grid-cols-1 gap-6 p-4 lg:hidden">
            <div v-for="item in nhanviens" :key="'mobile-'+item.mnv" class="bg-white rounded-xl shadow-sm border border-slate-200 p-5 flex flex-col gap-4">
              <div class="flex justify-between items-start border-b border-slate-100 pb-3">
                <div>
                  <h3 class="text-lg font-bold text-gray-900">{{ item.tnv }}</h3>
                  <p class="text-sm text-gray-500">{{ item.mnv }}</p>
                </div>
                <span class="px-3 py-1 bg-indigo-50 text-indigo-700 text-sm font-semibold rounded-lg">{{ item.chucdanh }}</span>
              </div>
              
              <div class="grid grid-cols-2 gap-4">
                <div v-for="q in quyenList" :key="'mob-'+q.code" class="flex items-start gap-3 p-2 rounded-lg hover:bg-slate-50 transition-colors">
                  <label class="inline-flex items-center cursor-pointer flex-shrink-0 mt-0.5">
                    <input type="checkbox" 
                           :checked="hasQuyen(item.permissions, q.code)"
                           @change="toggleQuyen(item, q.code)"
                           class="w-5 h-5 text-indigo-600 border-gray-300 rounded focus:ring-indigo-500 transition-colors cursor-pointer">
                  </label>
                  <div class="flex flex-col">
                    <span class="text-xs font-bold text-gray-700 uppercase leading-tight">{{ q.name }}</span>
                    <span class="text-[10px] text-gray-400 leading-tight mt-0.5">{{ q.desc }}</span>
                  </div>
                </div>
              </div>
              
              <div class="mt-2 pt-4 border-t border-slate-100 flex justify-end">
                <button @click="savePermissions(item)" 
                        :disabled="!item.hasChanged"
                        :class="item.hasChanged ? 'bg-indigo-600 hover:bg-indigo-700 text-white shadow-md' : 'bg-gray-100 text-gray-400 cursor-not-allowed'"
                        class="px-6 py-2.5 rounded-xl text-sm font-bold transition-all w-full text-center">
                  {{ item.isSaving ? 'Đang lưu...' : 'Lưu' }}
                </button>
              </div>
            </div>
          </div>

          <!-- Desktop View: Table -->
          <div class="overflow-x-auto hidden lg:block custom-scrollbar pb-2">
            <table class="min-w-full divide-y divide-gray-200">
              <thead class="bg-gray-50">
                <tr>
                  <th class="px-4 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider sticky left-0 bg-gray-50 z-20 border-r border-gray-200" style="min-width: 180px;">Tên nhân viên</th>
                  <th class="px-4 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider sticky bg-gray-50 z-20 border-r border-gray-200" style="left: 180px; min-width: 120px;">Chức danh</th>
                  <th v-for="q in quyenList" :key="q.code" class="px-2 py-4 text-center border-l border-gray-200" style="min-width: 110px;">
                    <div class="flex flex-col items-center gap-1">
                      <span :class="q.colorClass" class="w-8 h-8 rounded-full flex items-center justify-center text-white" v-html="q.icon"></span>
                      <span class="mt-2 text-[10px] font-bold text-gray-600 uppercase leading-tight">{{ q.name }}</span>
                      <span class="text-[9px] text-gray-400 font-normal normal-case leading-tight text-center">{{ q.desc }}</span>
                    </div>
                  </th>
                  <th class="px-6 py-4 text-center text-xs font-bold text-gray-500 uppercase tracking-wider sticky right-0 bg-gray-50 z-10 border-l border-gray-200">Lưu</th>
                </tr>
              </thead>
              <tbody class="bg-white divide-y divide-gray-200">
                <tr v-for="item in nhanviens" :key="'desk-'+item.mnv" class="hover:bg-indigo-50/30 transition-colors">
                  <td class="px-4 py-4 whitespace-nowrap sticky left-0 bg-white group-hover:bg-indigo-50/30 z-10 border-r border-gray-100">
                    <div class="flex flex-col">
                      <span class="text-sm font-bold text-gray-900">{{ item.tnv }}</span>
                      <span class="text-xs text-gray-500">{{ item.mnv }}</span>
                    </div>
                  </td>
                  <td class="px-4 py-4 whitespace-nowrap sticky bg-white group-hover:bg-indigo-50/30 z-10 border-r border-gray-100" style="left: 180px;">
                    <span class="text-sm font-semibold text-indigo-600">{{ item.chucdanh }}</span>
                  </td>
                  <td v-for="q in quyenList" :key="q.code" class="px-4 py-4 text-center border-b border-gray-100">
                    <label class="inline-flex items-center justify-center cursor-pointer">
                      <input type="checkbox" 
                             :checked="hasQuyen(item.permissions, q.code)"
                             @change="toggleQuyen(item, q.code)"
                             class="w-5 h-5 text-indigo-600 border-gray-300 rounded focus:ring-indigo-500 transition-colors cursor-pointer">
                    </label>
                  </td>
                  <td class="px-6 py-4 text-center sticky right-0 bg-white border-l border-gray-100 group-hover:bg-indigo-50/30 z-10">
                    <button @click="savePermissions(item)" 
                            :disabled="!item.hasChanged"
                            :class="item.hasChanged ? 'bg-indigo-600 hover:bg-indigo-700 text-white shadow-md hover:shadow-lg' : 'bg-gray-100 text-gray-400 cursor-not-allowed'"
                            class="px-4 py-2 rounded-lg text-sm font-bold transition-all w-24">
                      {{ item.isSaving ? 'Đang lưu...' : 'Lưu' }}
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

definePageMeta({
  layout: false,
  title: 'Phân quyền'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBaseUrl

const loading = ref(true)
const nhanviens = ref([])

const quyenList = [
  { code: 'Dashboard', name: 'ĐIỀU ĐỘ', desc: 'Màn hình theo dõi bãi xe', colorClass: 'bg-blue-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z"></path></svg>' },
  { code: 'ThuKho', name: 'THỦ KHO', desc: 'Nhận, xuất và đóng/mở cửa kho', colorClass: 'bg-orange-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"></path></svg>' },
  { code: 'BaoVe', name: 'BẢO VỆ', desc: 'Kiểm tra xe ra vào cổng', colorClass: 'bg-emerald-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"></path></svg>' },
  { code: 'Admin_NhaVanTai', name: 'QL NHÀ VẬN TẢI', desc: 'Thêm sửa xoá NVT', colorClass: 'bg-indigo-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4"></path></svg>' },
  { code: 'Admin_LaiXe', name: 'QL LÁI XE', desc: 'Thêm sửa xoá tài xế', colorClass: 'bg-indigo-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm14 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1z"></path></svg>' },
  { code: 'Admin_NhanVien', name: 'QL NHÂN VIÊN', desc: 'Thêm sửa xoá nhân sự', colorClass: 'bg-indigo-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>' },
  { code: 'Admin_KhoHang', name: 'QL KHO HÀNG', desc: 'Quản lý cửa kho', colorClass: 'bg-indigo-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>' },
  { code: 'Admin_BieuMau', name: 'QL BIỂU MẪU', desc: 'Thông số kiểm tra xe', colorClass: 'bg-indigo-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4"></path></svg>' },
  { code: 'Admin_PhanQuyen', name: 'PHÂN QUYỀN', desc: 'Chỉ định quyền truy cập', colorClass: 'bg-red-500', icon: '<svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>' }
]

const fetchList = async () => {
  loading.value = true
  try {
    const res = await $fetch(`${apiBase}/Nhanviens`)
    nhanviens.value = (res || []).map(nv => {
      let perms = nv.permissions || ''
      let changed = false
      
      // Auto-assign default permissions based on chucdanh if empty
      if (!perms) {
        if (nv.chucdanh === 'Thủ kho') {
          perms = 'Dashboard,ThuKho'
          changed = true
        } else if (nv.chucdanh === 'Bảo vệ') {
          perms = 'Dashboard,BaoVe'
          changed = true
        } else if (nv.chucdanh === 'Quản lý' || nv.chucdanh === 'Giám đốc') {
          perms = 'Dashboard,ThuKho,BaoVe,Admin_NhaVanTai,Admin_LaiXe,Admin_NhanVien,Admin_PhanQuyen,Admin_KhoHang,Admin_BieuMau'
          changed = true
        }
      }
      
      return {
        ...nv,
        permissions: perms,
        hasChanged: changed,
        isSaving: false
      }
    })
  } catch (err) {
    console.error(err)
    nhanviens.value = []
  } finally {
    loading.value = false
  }
}

const hasQuyen = (permissionsStr, code) => {
  if (!permissionsStr) return false
  const perms = permissionsStr.split(',').map(p => p.trim())
  return perms.includes(code)
}

const toggleQuyen = (item, code) => {
  let perms = item.permissions ? item.permissions.split(',').map(p => p.trim()) : []
  
  if (perms.includes(code)) {
    perms = perms.filter(p => p !== code)
  } else {
    perms.push(code)
  }
  
  item.permissions = perms.join(',')
  item.hasChanged = true
}

const savePermissions = async (item) => {
  if (!item.hasChanged) return
  
  item.isSaving = true
  try {
    await $fetch(`${apiBase}/Nhanviens/${item.mnv}`, {
      method: 'PUT',
      body: {
        mnv: item.mnv,
        tnv: item.tnv,
        chucdanh: item.chucdanh,
        vitri: item.vitri,
        password: item.password,
        permissions: item.permissions
      }
    })
    item.hasChanged = false
    alert(`Đã lưu quyền cho nhân viên: ${item.tnv}`)
  } catch (err) {
    console.error(err)
    alert('Lỗi khi lưu phân quyền!')
  } finally {
    item.isSaving = false
  }
}

onMounted(() => {
  fetchList()
})
</script>

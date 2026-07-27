import Swal from 'sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'

export default defineNuxtPlugin(() => {
  if (process.client) {
    window.alert = (message) => {
      return Swal.fire({
        title: 'Thông báo',
        text: message,
        icon: 'info',
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6',
        width: '400px'
      })
    }

    window.confirm = async (message) => {
      const result = await Swal.fire({
        title: 'Thông báo',
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'OK',
        cancelButtonText: 'Hủy',
        width: '400px'
      })
      return result.isConfirmed
    }

    window.prompt = async (message, defaultText = '') => {
      const result = await Swal.fire({
        title: 'Thông báo',
        text: message,
        input: 'text',
        inputValue: defaultText,
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Hủy',
        width: '400px'
      })
      return result.isConfirmed ? result.value : null
    }
  }
})

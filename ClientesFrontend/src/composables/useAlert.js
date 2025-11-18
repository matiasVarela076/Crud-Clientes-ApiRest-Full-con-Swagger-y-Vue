import { ref } from 'vue'

export function useAlert() {
  const alert = ref({
    show: false,
    message: '',
    type: 'info' // 'success', 'error', 'warning', 'info'
  })

  function showAlert(message, type = 'info', duration = 3000) {
    alert.value = {
      show: true,
      message,
      type
    }

    if (duration > 0) {
      setTimeout(() => {
        hideAlert()
      }, duration)
    }
  }

  function hideAlert() {
    alert.value.show = false
  }

  return {
    alert,
    showAlert,
    hideAlert
  }
}

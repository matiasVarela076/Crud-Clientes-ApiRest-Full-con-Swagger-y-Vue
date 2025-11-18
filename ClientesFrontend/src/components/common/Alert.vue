<template>
  <transition name="alert">
    <div v-if="show" :class="['alert', `alert-${type}`]">
      <span>{{ message }}</span>
      <button @click="$emit('close')" class="alert-close">×</button>
    </div>
  </transition>
</template>

<script setup>
defineProps({
  show: Boolean,
  message: String,
  type: { type: String, default: 'success' }
})

defineEmits(['close'])
</script>

<style scoped>
.alert {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem;
  border-radius: 12px;
  margin-bottom: 1.5rem;
  color: white;
  font-weight: 500;
}

.alert-success { background: linear-gradient(135deg, #10b981 0%, #059669 100%); }
.alert-error { background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); }
.alert-warning { background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); }

.alert-close {
  background: none;
  border: none;
  color: white;
  font-size: 1.5rem;
  cursor: pointer;
  padding: 0;
  opacity: 0.8;
  transition: opacity 0.2s;
}

.alert-close:hover { opacity: 1; }

.alert-enter-active, .alert-leave-active { transition: all 0.3s ease; }
.alert-enter-from, .alert-leave-to { opacity: 0; transform: translateY(-20px); }
</style>

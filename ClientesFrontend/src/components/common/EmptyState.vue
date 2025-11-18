<template>
  <div class="empty-state">
    <div class="empty-icon">{{ icon }}</div>
    <h3>{{ title }}</h3>
    <p>{{ description }}</p>
    <button v-if="showButton" @click="$emit('action')" class="btn-action">
      {{ buttonText }}
    </button>
  </div>
</template>

<script setup>
defineProps({
  icon: { type: String, default: '📋' },
  title: { type: String, default: 'Sin resultados' },
  description: { type: String, default: 'No hay datos para mostrar' },
  showButton: Boolean,
  buttonText: { type: String, default: 'Agregar' }
})

defineEmits(['action'])
</script>

<style scoped>
.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  color: white;
}

.empty-icon { font-size: 4rem; margin-bottom: 1rem; opacity: 0.7; }
.empty-state h3 { font-size: 1.5rem; margin-bottom: 0.5rem; }
.empty-state p { margin-bottom: 2rem; opacity: 0.9; }

.btn-action {
  padding: 0.875rem 1.5rem;
  border: none;
  border-radius: 12px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  box-shadow: 0 2px 8px rgba(102, 126, 234, 0.3);
}

.btn-action:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(102, 126, 234, 0.4);
}
</style>

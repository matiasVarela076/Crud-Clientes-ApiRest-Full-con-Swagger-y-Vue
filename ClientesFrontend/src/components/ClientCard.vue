<template>
  <div class="client-card">
    <div class="card-header">
      <div class="avatar">{{ initials }}</div>
      <div class="client-info">
        <h3>{{ cliente.NOMBRE || cliente.nombre }} {{ cliente.APELLIDO || cliente.apellido }}</h3>
        <span class="cuit">{{ cliente.CUIT || cliente.cuit }}</span>
      </div>
    </div>
    <div class="card-body">
      <div class="info-item">
        <span class="icon">📞</span>
        <span>{{ cliente.TELEFONO || cliente.telefono }}</span>
      </div>
      <div class="info-item">
        <span class="icon">✉️</span>
        <span class="email">{{ cliente.EMAIL || cliente.email }}</span>
      </div>
    </div>
    <div class="card-actions">
      <button @click="$emit('view')" class="btn-action btn-view" title="Ver">
        <span>👁️</span>
      </button>
      <button @click="$emit('edit')" class="btn-action btn-edit" title="Editar">
        <span>✏️</span>
      </button>
      <button @click="$emit('delete')" class="btn-action btn-delete" title="Eliminar">
        <span>🗑️</span>
      </button>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  cliente: {
    type: Object,
    required: true
  }
})

defineEmits(['view', 'edit', 'delete'])

const initials = computed(() => {
  const first = (props.cliente.NOMBRE || props.cliente.nombre || '')?.charAt(0) || ''
  const last = (props.cliente.APELLIDO || props.cliente.apellido || '')?.charAt(0) || ''
  return (first + last).toUpperCase()
})
</script>

<style scoped>
.client-card {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
}

.client-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.avatar {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: bold;
  font-size: 1.2rem;
  flex-shrink: 0;
}

.client-info {
  flex: 1;
  min-width: 0;
}

.client-info h3 {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #1f2937;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.cuit {
  font-size: 0.875rem;
  color: #6b7280;
}

.card-body {
  margin: 1rem 0;
  padding: 1rem 0;
  border-top: 1px solid #e5e7eb;
  border-bottom: 1px solid #e5e7eb;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 0;
  font-size: 0.875rem;
  color: #4b5563;
}

.icon {
  font-size: 1rem;
}

.email {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
  margin-top: 1rem;
}

.btn-action {
  flex: 1;
  padding: 0.625rem;
  border: none;
  border-radius: 8px;
  background: #f3f4f6;
  cursor: pointer;
  font-size: 1.25rem;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-action:hover {
  transform: scale(1.05);
}

.btn-view:hover {
  background: #dbeafe;
}

.btn-edit:hover {
  background: #fef3c7;
}

.btn-delete:hover {
  background: #fee2e2;
}
</style>

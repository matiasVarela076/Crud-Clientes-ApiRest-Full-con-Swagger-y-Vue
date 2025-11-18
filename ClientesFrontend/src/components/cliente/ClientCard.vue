<template>
  <div class="client-card">
    <div class="card-header">
      <h3>{{ getClienteDisplayName(cliente) }}</h3>
      <div class="card-actions">
        <button @click="$emit('view')" class="btn-icon" title="Ver detalles">👁️</button>
        <button @click="$emit('edit')" class="btn-icon" title="Editar">✏️</button>
        <button @click="$emit('delete')" class="btn-icon btn-danger" title="Eliminar">🗑️</button>
      </div>
    </div>
    <div class="card-body">
      <div class="info-row">
        <span class="label">CUIT:</span>
        <span class="value">{{ getClienteProperty(cliente, 'cuit') }}</span>
      </div>
      <div class="info-row">
        <span class="label">Email:</span>
        <span class="value">{{ getClienteProperty(cliente, 'email') }}</span>
      </div>
      <div class="info-row">
        <span class="label">Teléfono:</span>
        <span class="value">{{ getClienteProperty(cliente, 'telefono') }}</span>
      </div>
      <div class="info-row">
        <span class="label">Estado:</span>
        <span :class="['status', getClienteProperty(cliente, 'activo') ? 'active' : 'inactive']">
          {{ getClienteProperty(cliente, 'activo') ? 'Activo' : 'Inactivo' }}
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getClienteDisplayName, getClienteProperty } from '../../utils/formatters'

defineProps({
  cliente: { type: Object, required: true }
})

defineEmits(['view', 'edit', 'delete'])
</script>

<style scoped>
.client-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.client-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: start;
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f3f4f6;
}

.card-header h3 {
  margin: 0;
  font-size: 1.1rem;
  color: #111827;
  flex: 1;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
}

.btn-icon {
  background: none;
  border: none;
  font-size: 1.25rem;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 8px;
  transition: all 0.2s;
  opacity: 0.7;
}

.btn-icon:hover {
  opacity: 1;
  background: #f3f4f6;
}

.btn-icon.btn-danger:hover {
  background: #fee2e2;
}

.card-body {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.info-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.875rem;
}

.label {
  font-weight: 600;
  color: #6b7280;
}

.value {
  color: #111827;
  text-align: right;
}

.status {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-weight: 600;
  font-size: 0.75rem;
}

.status.active {
  background: #d1fae5;
  color: #065f46;
}

.status.inactive {
  background: #fee2e2;
  color: #991b1b;
}
</style>

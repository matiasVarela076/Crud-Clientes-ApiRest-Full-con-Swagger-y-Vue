<template>
  <transition name="modal">
    <div v-if="isOpen" class="modal-overlay" @click.self="$emit('close')">
      <div class="modal-content">
        <div class="modal-header">
          <h2>{{ isEdit ? 'Editar Cliente' : 'Nuevo Cliente' }}</h2>
          <button @click="$emit('close')" class="btn-close">×</button>
        </div>
        
        <form @submit.prevent="handleSubmit" class="modal-body">
          <div class="form-row">
            <div class="form-group">
              <label>Nombres <span class="required">*</span></label>
              <input v-model="formData.nombre" type="text" required placeholder="Juan" />
            </div>
            <div class="form-group">
              <label>Apellidos <span class="required">*</span></label>
              <input v-model="formData.apellido" type="text" required placeholder="Pérez" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>CUIT <span class="required">*</span></label>
              <input v-model="formData.cuit" type="text" required placeholder="20-12345678-9" />
            </div>
            <div class="form-group">
              <label>Teléfono <span class="required">*</span></label>
              <input v-model="formData.telefono" type="text" required placeholder="1123456789" />
            </div>
          </div>

          <div class="form-group">
            <label>Email <span class="required">*</span></label>
            <input v-model="formData.email" type="email" required placeholder="juan@email.com" />
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Fecha de Nacimiento</label>
              <input v-model="formData.fechaNacimiento" type="date" />
            </div>
            <div class="form-group">
              <label>Domicilio</label>
              <input v-model="formData.domicilio" type="text" placeholder="Calle 123" />
            </div>
          </div>

          <div class="modal-footer">
            <button type="button" @click="$emit('close')" class="btn btn-secondary">
              Cancelar
            </button>
            <button type="submit" class="btn btn-primary">
              {{ isEdit ? 'Actualizar' : 'Crear' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </transition>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  isOpen: Boolean,
  cliente: Object,
  isEdit: Boolean
})

const emit = defineEmits(['close', 'submit'])

const formData = ref({
  nombre: '',
  apellido: '',
  cuit: '',
  telefono: '',
  email: '',
  fechaNacimiento: '',
  domicilio: ''
})

watch(() => props.cliente, (newVal) => {
  if (newVal && props.isEdit) {
    formData.value = {
      id: newVal.ID || newVal.id,
      nombre: newVal.NOMBRE || newVal.nombre || '',
      apellido: newVal.APELLIDO || newVal.apellido || '',
      cuit: newVal.CUIT || newVal.cuit || '',
      telefono: newVal.TELEFONO || newVal.telefono || '',
      email: newVal.EMAIL || newVal.email || '',
      fechaNacimiento: newVal.FECHA_NACIMIENTO || newVal.fechaNacimiento || '',
      domicilio: newVal.DOMICILIO || newVal.domicilio || ''
    }
  } else {
    resetForm()
  }
}, { immediate: true })

function handleSubmit() {
  const apiData = {
    ID: formData.value.id || 0,
    NOMBRE: formData.value.nombre,
    APELLIDO: formData.value.apellido,
    CUIT: formData.value.cuit,
    TELEFONO: formData.value.telefono,
    EMAIL: formData.value.email,
    FECHA_NACIMIENTO: formData.value.fechaNacimiento || null,
    DOMICILIO: formData.value.domicilio || null
  }
  emit('submit', apiData)
}

function resetForm() {
  formData.value = {
    nombre: '',
    apellido: '',
    cuit: '',
    telefono: '',
    email: '',
    fechaNacimiento: '',
    domicilio: ''
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: white;
  border-radius: 16px;
  max-width: 600px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem 2rem;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h2 {
  margin: 0;
  font-size: 1.5rem;
  color: #111827;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  color: #6b7280;
  cursor: pointer;
  padding: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  transition: all 0.2s;
}

.btn-close:hover {
  background: #f3f4f6;
  color: #111827;
}

.modal-body {
  padding: 2rem;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #374151;
  font-size: 0.875rem;
}

.required {
  color: #ef4444;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.modal-footer {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding-top: 1.5rem;
  border-top: 1px solid #e5e7eb;
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
}

.btn-secondary:hover {
  background: #e5e7eb;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.modal-enter-active, .modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-active .modal-content, .modal-leave-active .modal-content {
  transition: transform 0.3s ease;
}

.modal-enter-from, .modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal-content, .modal-leave-to .modal-content {
  transform: scale(0.9);
}

@media (max-width: 640px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>

<template>
  <div class="clientes-view">
    <!-- Alert -->
    <transition name="alert">
      <div v-if="alert.show" :class="['alert', `alert-${alert.type}`]">
        <span>{{ alert.message }}</span>
        <button @click="alert.show = false" class="alert-close">×</button>
      </div>
    </transition>

    <!-- Toolbar -->
    <div class="toolbar">
      <div class="search-bar">
        <input
          v-model="searchQuery"
          @keyup.enter="handleSearch"
          type="text"
          placeholder="Buscar por nombre o apellido..."
          class="search-input"
        />
        <button @click="handleSearch" class="btn-search">🔍</button>
        <button v-if="searchQuery" @click="clearSearch" class="btn-clear">✕</button>
      </div>
      <button @click="openCreateModal" class="btn-new">
        ➕ Nuevo Cliente
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Cargando clientes...</p>
    </div>

    <!-- Clientes Grid -->
    <div v-else-if="clientes.length" class="clientes-grid">
      <ClientCard
        v-for="cliente in clientes"
        :key="cliente.id"
        :cliente="cliente"
        @view="viewCliente(cliente)"
        @edit="editCliente(cliente)"
        @delete="confirmDelete(cliente)"
      />
    </div>

    <!-- Empty State -->
    <div v-else class="empty-state">
      <div class="empty-icon">📋</div>
      <h3>No hay clientes registrados</h3>
      <p>Comienza agregando tu primer cliente</p>
      <button @click="openCreateModal" class="btn-new">
        ➕ Agregar Cliente
      </button>
    </div>

    <!-- Modal Crear/Editar -->
    <ClientModal
      :show="showModal"
      :cliente="selectedCliente"
      :isEdit="isEditMode"
      @close="closeModal"
      @submit="handleSubmit"
    />

    <!-- Modal Ver Detalles -->
    <transition name="modal">
      <div v-if="showViewModal" class="modal-overlay" @click.self="showViewModal = false">
        <div class="modal-view">
          <div class="view-header">
            <h2>Detalles del Cliente</h2>
            <button @click="showViewModal = false" class="btn-close">×</button>
          </div>
          <div class="view-body">
            <div class="detail-row">
              <strong>Nombres:</strong>
              <span>{{ selectedCliente.NOMBRE || selectedCliente.nombre }}</span>
            </div>
            <div class="detail-row">
              <strong>Apellidos:</strong>
              <span>{{ selectedCliente.APELLIDO || selectedCliente.apellido }}</span>
            </div>
            <div class="detail-row">
              <strong>CUIT:</strong>
              <span>{{ selectedCliente.CUIT || selectedCliente.cuit }}</span>
            </div>
            <div class="detail-row">
              <strong>Teléfono:</strong>
              <span>{{ selectedCliente.TELEFONO || selectedCliente.telefono }}</span>
            </div>
            <div class="detail-row">
              <strong>Email:</strong>
              <span>{{ selectedCliente.EMAIL || selectedCliente.email }}</span>
            </div>
            <div class="detail-row">
              <strong>Domicilio:</strong>
              <span>{{ selectedCliente.DOMICILIO || selectedCliente.domicilio || 'No especificado' }}</span>
            </div>
            <div class="detail-row" v-if="selectedCliente.FECHA_NACIMIENTO">
              <strong>Fecha de Nacimiento:</strong>
              <span>{{ new Date(selectedCliente.FECHA_NACIMIENTO).toLocaleDateString() }}</span>
            </div>
          </div>
          <div class="view-footer">
            <button @click="showViewModal = false" class="btn btn-secondary">Cerrar</button>
          </div>
        </div>
      </div>
    </transition>

    <!-- Modal Confirmar Eliminación -->
    <transition name="modal">
      <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
        <div class="modal-delete">
          <div class="delete-header">
            <div class="delete-icon">⚠️</div>
            <h2>Confirmar Eliminación</h2>
          </div>
          <div class="delete-body">
            <p>¿Estás seguro que deseas eliminar este cliente?</p>
            <div class="cliente-info">
              <strong>{{ clienteToDelete.NOMBRE || clienteToDelete.nombre }} {{ clienteToDelete.APELLIDO || clienteToDelete.apellido }}</strong>
              <span>{{ clienteToDelete.CUIT || clienteToDelete.cuit }}</span>
            </div>
            <p class="warning">Esta acción realizará un borrado lógico</p>
          </div>
          <div class="delete-footer">
            <button @click="showDeleteModal = false" class="btn btn-secondary">Cancelar</button>
            <button @click="handleDelete" class="btn btn-danger">Eliminar</button>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import ClientCard from '../components/ClientCard.vue'
import ClientModal from '../components/ClientModal.vue'
import clienteService from '../services/ClienteService'

const clientes = ref([])
const loading = ref(false)
const searchQuery = ref('')
const showModal = ref(false)
const showViewModal = ref(false)
const showDeleteModal = ref(false)
const isEditMode = ref(false)
const selectedCliente = ref({})
const clienteToDelete = ref({})
const alert = ref({ show: false, message: '', type: 'success' })

onMounted(() => {
  loadClientes()
})

async function loadClientes() {
  loading.value = true
  try {
    clientes.value = await clienteService.getAll()
  } catch (error) {
    showAlert('Error al cargar clientes', 'error')
  } finally {
    loading.value = false
  }
}

async function handleSearch() {
  if (!searchQuery.value.trim()) {
    await loadClientes()
    return
  }
  loading.value = true
  try {
    clientes.value = await clienteService.search(searchQuery.value)
    if (clientes.value.length === 0) {
      showAlert('No se encontraron clientes', 'warning')
    }
  } catch (error) {
    showAlert('Error al buscar', 'error')
  } finally {
    loading.value = false
  }
}

function clearSearch() {
  searchQuery.value = ''
  loadClientes()
}

function openCreateModal() {
  isEditMode.value = false
  selectedCliente.value = {}
  showModal.value = true
}

function viewCliente(cliente) {
  selectedCliente.value = { ...cliente }
  showViewModal.value = true
}

function editCliente(cliente) {
  isEditMode.value = true
  selectedCliente.value = { ...cliente }
  showModal.value = true
}

function confirmDelete(cliente) {
  clienteToDelete.value = cliente
  showDeleteModal.value = true
}

async function handleSubmit(formData) {
  try {
    if (isEditMode.value) {
      const id = formData.ID || formData.id
      await clienteService.update(id, formData)
      showAlert('Cliente actualizado exitosamente', 'success')
    } else {
      await clienteService.create(formData)
      showAlert('Cliente creado exitosamente', 'success')
    }
    closeModal()
    await loadClientes()
  } catch (error) {
    const message = error.response?.data?.error || error.response?.data?.detalle || 'Error al guardar cliente'
    showAlert(message, 'error')
  }
}

async function handleDelete() {
  try {
    const id = clienteToDelete.value.ID || clienteToDelete.value.id
    await clienteService.delete(id)
    showDeleteModal.value = false
    showAlert('Cliente eliminado correctamente', 'success')
    clientes.value = clientes.value.filter(c => (c.ID || c.id) !== id)
  } catch (error) {
    showAlert('Error al eliminar cliente', 'error')
  }
}

function closeModal() {
  showModal.value = false
  selectedCliente.value = {}
}

function showAlert(message, type = 'success') {
  alert.value = { show: true, message, type }
  setTimeout(() => {
    alert.value.show = false
  }, 4000)
}
</script>

<style scoped>
.clientes-view {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

/* Alert */
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

.alert-success {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

.alert-error {
  background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
}

.alert-warning {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.alert-close {
  background: none;
  border: none;
  color: white;
  font-size: 1.5rem;
  cursor: pointer;
  padding: 0;
  opacity: 0.8;
}

.alert-close:hover {
  opacity: 1;
}

/* Toolbar */
.toolbar {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.search-bar {
  flex: 1;
  display: flex;
  gap: 0.5rem;
  min-width: 300px;
}

.search-input {
  flex: 1;
  padding: 0.875rem 1rem;
  border: none;
  border-radius: 12px;
  font-size: 1rem;
  background: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.search-input:focus {
  outline: none;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.2);
}

.btn-search,
.btn-clear {
  padding: 0.875rem 1rem;
  border: none;
  border-radius: 12px;
  background: white;
  cursor: pointer;
  font-size: 1.25rem;
  transition: all 0.2s;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.btn-search:hover,
.btn-clear:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.btn-new {
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

.btn-new:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(102, 126, 234, 0.4);
}

/* Loading */
.loading {
  text-align: center;
  padding: 4rem 2rem;
  color: white;
}

.spinner {
  width: 50px;
  height: 50px;
  border: 4px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Grid */
.clientes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.5rem;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  color: white;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
  opacity: 0.7;
}

.empty-state h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.empty-state p {
  margin-bottom: 2rem;
  opacity: 0.9;
}

/* Modals */
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

.modal-view,
.modal-delete {
  background: white;
  border-radius: 16px;
  max-width: 500px;
  width: 100%;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
}

.view-header,
.delete-header {
  padding: 1.5rem 2rem;
  border-bottom: 1px solid #e5e7eb;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.view-header h2,
.delete-header h2 {
  margin: 0;
  font-size: 1.5rem;
  color: #111827;
}

.delete-header {
  flex-direction: column;
  text-align: center;
  gap: 1rem;
}

.delete-icon {
  font-size: 3rem;
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
}

.view-body,
.delete-body {
  padding: 2rem;
}

.detail-row {
  display: flex;
  padding: 0.75rem 0;
  border-bottom: 1px solid #f3f4f6;
}

.detail-row strong {
  flex: 0 0 120px;
  color: #6b7280;
}

.detail-row span {
  flex: 1;
  color: #111827;
}

.cliente-info {
  margin: 1.5rem 0;
  padding: 1rem;
  background: #f9fafb;
  border-radius: 8px;
  text-align: center;
}

.cliente-info strong {
  display: block;
  font-size: 1.1rem;
  color: #111827;
  margin-bottom: 0.25rem;
}

.cliente-info span {
  font-size: 0.875rem;
  color: #6b7280;
}

.warning {
  color: #ef4444;
  font-size: 0.875rem;
  text-align: center;
}

.view-footer,
.delete-footer {
  padding: 1.5rem 2rem;
  border-top: 1px solid #e5e7eb;
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-weight: 600;
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

.btn-danger {
  background: #ef4444;
  color: white;
}

.btn-danger:hover {
  background: #dc2626;
}

/* Transitions */
.alert-enter-active,
.alert-leave-active,
.modal-enter-active,
.modal-leave-active {
  transition: all 0.3s ease;
}

.alert-enter-from,
.alert-leave-to {
  opacity: 0;
  transform: translateY(-20px);
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal-view,
.modal-enter-from .modal-delete,
.modal-leave-to .modal-view,
.modal-leave-to .modal-delete {
  transform: scale(0.9);
}

@media (max-width: 768px) {
  .clientes-view {
    padding: 1rem;
  }

  .toolbar {
    flex-direction: column;
  }

  .search-bar {
    min-width: 100%;
  }

  .clientes-grid {
    grid-template-columns: 1fr;
  }
}
</style>

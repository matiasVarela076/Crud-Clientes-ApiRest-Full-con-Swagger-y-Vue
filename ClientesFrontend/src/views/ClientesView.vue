<template>
  <div class="clientes-view">
    <Alert :show="alert.show" :message="alert.message" :type="alert.type" @close="hideAlert" />

    <div class="toolbar">
      <div class="search-bar">
        <input v-model="searchQuery" @keyup.enter="handleSearch" type="text" placeholder="Buscar por nombre..." class="search-input" />
        <button @click="handleSearch" class="btn-search">🔍</button>
        <button v-if="searchQuery" @click="clearSearch" class="btn-clear">✕</button>
      </div>
      <button @click="openCreateModal" class="btn-new">➕ Nuevo Cliente</button>
    </div>

    <LoadingSpinner v-if="loading" message="Cargando clientes..." />

    <div v-else-if="hasClientes" class="clientes-grid">
      <ClientCard
        v-for="cliente in clientes"
        :key="cliente.id || cliente.ID"
        :cliente="cliente"
        @view="viewCliente(cliente)"
        @edit="editCliente(cliente)"
        @delete="confirmDelete(cliente)"
      />
    </div>

    <EmptyState
      v-else
      icon="📋"
      title="No hay clientes registrados"
      description="Comienza agregando tu primer cliente"
      :showButton="true"
      buttonText="➕ Agregar Cliente"
      @action="openCreateModal"
    />

    <ClientModal :isOpen="showModal" :isEdit="isEditMode" :cliente="selectedCliente" @close="closeModal" @submit="handleSubmit" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useClienteStore } from '../stores/clienteStore'
import { useAlert } from '../composables/useAlert'
import Alert from '../components/common/Alert.vue'
import LoadingSpinner from '../components/common/LoadingSpinner.vue'
import EmptyState from '../components/common/EmptyState.vue'
import ClientCard from '../components/cliente/ClientCard.vue'
import ClientModal from '../components/cliente/ClientModal.vue'

const store = useClienteStore()
const { alert, showAlert, hideAlert } = useAlert()

const searchQuery = ref('')
const showModal = ref(false)
const isEditMode = ref(false)
const selectedCliente = ref({})

const clientes = ref([])
const loading = ref(false)
const hasClientes = ref(false)

onMounted(() => {
  loadClientes()
})

async function loadClientes() {
  loading.value = true
  try {
    await store.loadClientes()
    clientes.value = store.clientes
    hasClientes.value = store.hasClientes
  } catch (error) {
    showAlert(error.message || 'Error al cargar clientes', 'error')
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
    await store.searchClientes(searchQuery.value)
    clientes.value = store.clientes
    hasClientes.value = store.hasClientes
    if (!hasClientes.value) {
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
}

function editCliente(cliente) {
  isEditMode.value = true
  selectedCliente.value = { ...cliente }
  showModal.value = true
}

function confirmDelete(cliente) {
  if (confirm(`¿Estás seguro que deseas eliminar a ${cliente.nombre || cliente.NOMBRE}?`)) {
    handleDelete(cliente)
  }
}

async function handleSubmit(formData) {
  try {
    if (isEditMode.value) {
      const id = formData.ID || formData.id
      await store.updateCliente(id, formData)
      showAlert('Cliente actualizado exitosamente', 'success')
    } else {
      await store.createCliente(formData)
      showAlert('Cliente creado exitosamente', 'success')
    }
    closeModal()
    await loadClientes()
  } catch (error) {
    showAlert(error.message || 'Error al guardar cliente', 'error')
  }
}

async function handleDelete(cliente) {
  try {
    const id = cliente.ID || cliente.id
    await store.deleteCliente(id)
    showAlert('Cliente eliminado correctamente', 'success')
    await loadClientes()
  } catch (error) {
    showAlert('Error al eliminar cliente', 'error')
  }
}

function closeModal() {
  showModal.value = false
  selectedCliente.value = {}
}
</script>

<style scoped>
.clientes-view {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

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

.btn-search, .btn-clear {
  padding: 0.875rem 1rem;
  border: none;
  border-radius: 12px;
  background: white;
  cursor: pointer;
  font-size: 1.25rem;
  transition: all 0.2s;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.btn-search:hover, .btn-clear:hover {
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

.clientes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.5rem;
}

@media (max-width: 768px) {
  .clientes-view { padding: 1rem; }
  .toolbar { flex-direction: column; }
  .search-bar { min-width: 100%; }
  .clientes-grid { grid-template-columns: 1fr; }
}
</style>

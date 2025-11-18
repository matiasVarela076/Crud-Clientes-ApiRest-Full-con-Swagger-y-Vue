import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { clienteService } from '../services/ClienteService'

export const useClienteStore = defineStore('cliente', () => {
  const clientes = ref([])
  const loading = ref(false)
  const error = ref(null)

  const hasClientes = computed(() => clientes.value.length > 0)

  async function loadClientes() {
    loading.value = true
    error.value = null
    try {
      clientes.value = await clienteService.getAll()
    } catch (err) {
      error.value = err.message || 'Error al cargar clientes'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function searchClientes(query) {
    loading.value = true
    error.value = null
    try {
      clientes.value = await clienteService.search(query)
    } catch (err) {
      error.value = err.message || 'Error al buscar clientes'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createCliente(data) {
    loading.value = true
    error.value = null
    try {
      const newCliente = await clienteService.create(data)
      clientes.value.push(newCliente)
      return newCliente
    } catch (err) {
      error.value = err.message || 'Error al crear cliente'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateCliente(id, data) {
    loading.value = true
    error.value = null
    try {
      const updated = await clienteService.update(id, data)
      const index = clientes.value.findIndex(c => c.id === id || c.ID === id)
      if (index !== -1) {
        clientes.value[index] = updated
      }
      return updated
    } catch (err) {
      error.value = err.message || 'Error al actualizar cliente'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteCliente(id) {
    loading.value = true
    error.value = null
    try {
      await clienteService.delete(id)
      clientes.value = clientes.value.filter(c => c.id !== id && c.ID !== id)
    } catch (err) {
      error.value = err.message || 'Error al eliminar cliente'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    clientes,
    loading,
    error,
    hasClientes,
    loadClientes,
    searchClientes,
    createCliente,
    updateCliente,
    deleteCliente
  }
})

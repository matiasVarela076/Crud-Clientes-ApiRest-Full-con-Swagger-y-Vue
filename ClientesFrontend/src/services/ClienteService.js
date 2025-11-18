import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001/api'

const clienteService = {
  // Obtener todos los clientes
  async getAll() {
    try {
      const response = await axios.get(`${API_URL}/clientes`)
      return response.data
    } catch (error) {
      console.error('Error al obtener clientes:', error)
      throw error
    }
  },

  // Obtener cliente por ID
  async getById(id) {
    try {
      const response = await axios.get(`${API_URL}/clientes/${id}`)
      return response.data
    } catch (error) {
      console.error(`Error al obtener cliente ${id}:`, error)
      throw error
    }
  },

  // Buscar clientes por nombre
  async search(nombre) {
    try {
      const response = await axios.get(`${API_URL}/clientes/search/${nombre}`)
      return response.data
    } catch (error) {
      console.error('Error al buscar clientes:', error)
      throw error
    }
  },

  // Crear nuevo cliente
  async create(cliente) {
    try {
      const response = await axios.post(`${API_URL}/clientes`, cliente)
      return response.data
    } catch (error) {
      console.error('Error al crear cliente:', error)
      throw error
    }
  },

  // Actualizar cliente
  async update(id, cliente) {
    try {
      const response = await axios.put(`${API_URL}/clientes/${id}`, cliente)
      return response.data
    } catch (error) {
      console.error('Error al actualizar cliente:', error)
      throw error
    }
  },

  // Eliminar cliente (borrado lógico)
  async delete(id) {
    try {
      const response = await axios.delete(`${API_URL}/clientes/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al eliminar cliente:', error)
      throw error
    }
  }
}

export { clienteService }
export default clienteService

import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000'

const httpClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Interceptor para manejar errores
httpClient.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Manejar autenticación si es necesario
    }
    return Promise.reject(error)
  }
)

export default httpClient

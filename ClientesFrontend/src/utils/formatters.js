export function formatDate(date) {
  if (!date) return ''
  return new Date(date).toLocaleDateString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

export function formatCUIT(cuit) {
  if (!cuit) return ''
  return cuit.toString().replace(/(\d{2})(\d{8})(\d{1})/, '$1-$2-$3')
}

export function formatPhone(phone) {
  if (!phone) return ''
  return phone.toString().replace(/(\d{2})(\d{4})(\d{4})/, '$1 $2-$3')
}

export function getClienteDisplayName(cliente) {
  const nombre = cliente.nombre || cliente.NOMBRE || ''
  const apellido = cliente.apellido || cliente.APELLIDO || ''
  return `${nombre} ${apellido}`.trim()
}

export function getClienteProperty(cliente, key) {
  return cliente[key] || cliente[key.toUpperCase()] || ''
}

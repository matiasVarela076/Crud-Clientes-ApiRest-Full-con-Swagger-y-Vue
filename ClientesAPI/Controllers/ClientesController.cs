using ClientesAPI.Data;
using ClientesAPI.Helpers;
using ClientesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ClientesContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los clientes activos del sistema
        /// </summary>
        /// <returns>Lista de clientes activos</returns>
        /// <response code="200">Retorna la lista de clientes activos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de todos los clientes activos");
                
                List<Cliente> clientes = await _context.Clientes
                    .Where(c => c.ACTIVO == true)
                    .ToListAsync();
                
                _logger.LogInformation("Consulta exitosa: {Count} clientes activos obtenidos", clientes.Count);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de clientes activos");
                return StatusCode(500, new { error = "Error al obtener los clientes", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID
        /// </summary>
        /// <param name="id">ID del cliente a buscar</param>
        /// <returns>Cliente encontrado</returns>
        /// <response code="200">Retorna el cliente solicitado</response>
        /// <response code="404">Cliente no encontrado o inactivo</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            try
            {
                _logger.LogInformation("Buscando cliente con ID: {ClienteId}", id);
                
                Cliente? cliente = await _context.Clientes
                    .Where(c => c.ID == id && c.ACTIVO == true)
                    .FirstOrDefaultAsync();

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado o está inactivo", id);
                    return NotFound(new { error = $"Cliente con ID {id} no encontrado o inactivo" });
                }

                _logger.LogInformation("Cliente con ID {ClienteId} encontrado exitosamente", id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con ID: {ClienteId}", id);
                return StatusCode(500, new { error = "Error al obtener el cliente", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Busca clientes activos por nombre o apellido
        /// </summary>
        /// <param name="nombre">Texto a buscar en nombre o apellido</param>
        /// <returns>Lista de clientes que coinciden con la búsqueda</returns>
        /// <response code="200">Retorna la lista de clientes encontrados</response>
        /// <response code="400">Parámetro de búsqueda vacío</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("search/{nombre}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Search(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return BadRequest(new { error = "El nombre de búsqueda no puede estar vacío" });
                }

                _logger.LogInformation("Buscando clientes con nombre/apellido que contenga: {Nombre}", nombre);
                
                List<Cliente> clientes = await _context.Clientes
                    .Where(c => (c.NOMBRE.Contains(nombre) || c.APELLIDO.Contains(nombre)) && c.ACTIVO == true)
                    .ToListAsync();

                if (clientes.Count == 0)
                {
                    _logger.LogWarning("No se encontraron clientes con el nombre/apellido: {Nombre}", nombre);
                    return NotFound(new { error = $"No se encontraron clientes con el nombre '{nombre}'" });
                }

                _logger.LogInformation("Búsqueda exitosa: {Count} clientes encontrados con nombre/apellido: {Nombre}", clientes.Count, nombre);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes por nombre: {Nombre}", nombre);
                return StatusCode(500, new { error = "Error al buscar clientes", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente en el sistema
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>Cliente creado con su ID asignado</returns>
        /// <response code="201">Cliente creado exitosamente</response>
        /// <response code="400">Datos inválidos o CUIT duplicado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        public async Task<ActionResult<Cliente>> Insert([FromBody] Cliente cliente)
        {
            try
            {
                _logger.LogInformation("Iniciando creación de nuevo cliente con CUIT: {CUIT}", cliente.CUIT);
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validación de modelo fallida al crear cliente");
                    return BadRequest(ModelState);
                }

                (bool esValido, string mensaje) validacion = ClientesHelper.ValidarCliente(cliente);
                if (!validacion.esValido)
                {
                    _logger.LogWarning("Validación de negocio fallida: {Mensaje}", validacion.mensaje);
                    return BadRequest(new { error = validacion.mensaje });
                }

                Cliente? clienteExistente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CUIT == cliente.CUIT && c.ACTIVO == true);
                if (clienteExistente != null)
                {
                    _logger.LogWarning("Intento de crear cliente con CUIT duplicado: {CUIT}", cliente.CUIT);
                    return BadRequest(new { error = "Ya existe un cliente activo con este CUIT" });
                }

                cliente.FECHA_CREACION = DateTime.Now;
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente creado exitosamente con ID: {ClienteId}, CUIT: {CUIT}", cliente.ID, cliente.CUIT);
                return CreatedAtAction(nameof(Get), new { id = cliente.ID }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente con CUIT: {CUIT}", cliente.CUIT);
                return StatusCode(500, new { error = "Error al crear el cliente", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente a actualizar</param>
        /// <param name="cliente">Datos actualizados del cliente</param>
        /// <returns>Cliente actualizado</returns>
        /// <response code="200">Cliente actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Cliente no encontrado o inactivo</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            try
            {
                _logger.LogInformation("Iniciando actualización de cliente con ID: {ClienteId}", id);
                
                if (id != cliente.ID)
                {
                    _logger.LogWarning("ID en URL ({UrlId}) no coincide con ID en body ({BodyId})", id, cliente.ID);
                    return BadRequest(new { error = "El ID no coincide" });
                }

                Cliente? clienteExistente = await _context.Clientes
                    .Where(c => c.ID == id && c.ACTIVO == true)
                    .FirstOrDefaultAsync();
                if (clienteExistente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado o está inactivo", id);
                    return NotFound(new { error = $"Cliente con ID {id} no encontrado o inactivo" });
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validación de modelo fallida al actualizar cliente ID: {ClienteId}", id);
                    return BadRequest(ModelState);
                }

                (bool esValido, string mensaje) validacion = ClientesHelper.ValidarCliente(cliente);
                if (!validacion.esValido)
                {
                    _logger.LogWarning("Validación de negocio fallida al actualizar cliente ID {ClienteId}: {Mensaje}", id, validacion.mensaje);
                    return BadRequest(new { error = validacion.mensaje });
                }

                Cliente? cuitDuplicado = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CUIT == cliente.CUIT && c.ID != id && c.ACTIVO == true);
                if (cuitDuplicado != null)
                {
                    _logger.LogWarning("Intento de actualizar cliente ID {ClienteId} con CUIT duplicado: {CUIT}", id, cliente.CUIT);
                    return BadRequest(new { error = "Ya existe otro cliente activo con este CUIT" });
                }

                clienteExistente.NOMBRE = cliente.NOMBRE;
                clienteExistente.APELLIDO = cliente.APELLIDO;
                clienteExistente.FECHA_NACIMIENTO = cliente.FECHA_NACIMIENTO;
                clienteExistente.CUIT = cliente.CUIT;
                clienteExistente.DOMICILIO = cliente.DOMICILIO;
                clienteExistente.TELEFONO = cliente.TELEFONO;
                clienteExistente.EMAIL = cliente.EMAIL;
                clienteExistente.FECHA_MODIFICACION = DateTime.Now;

                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente actualizado exitosamente - ID: {ClienteId}, CUIT: {CUIT}", id, cliente.CUIT);
                return Ok(new { mensaje = "Cliente actualizado correctamente", cliente = clienteExistente });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente con ID: {ClienteId}", id);
                return StatusCode(500, new { error = "Error al actualizar el cliente", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un cliente del sistema (borrado lógico)
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Cliente eliminado exitosamente</response>
        /// <response code="404">Cliente no encontrado o ya inactivo</response>
        /// <response code="500">Error interno del servidor</response>
        /// <remarks>
        /// Este endpoint realiza un borrado lógico, marcando el campo ACTIVO como false.
        /// El cliente no se elimina físicamente de la base de datos.
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando eliminación lógica de cliente con ID: {ClienteId}", id);
                
                Cliente? cliente = await _context.Clientes
                    .Where(c => c.ID == id && c.ACTIVO == true)
                    .FirstOrDefaultAsync();

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado o ya está inactivo", id);
                    return NotFound(new { error = $"Cliente con ID {id} no encontrado o ya está inactivo" });
                }

                cliente.ACTIVO = false;
                cliente.FECHA_MODIFICACION = DateTime.Now;
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente eliminado exitosamente (borrado lógico) - ID: {ClienteId}, CUIT: {CUIT}", id, cliente.CUIT);
                return Ok(new { mensaje = "Cliente eliminado correctamente", id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente con ID: {ClienteId}", id);
                return StatusCode(500, new { error = "Error al eliminar el cliente", detalle = ex.Message });
            }
        }
    }
}

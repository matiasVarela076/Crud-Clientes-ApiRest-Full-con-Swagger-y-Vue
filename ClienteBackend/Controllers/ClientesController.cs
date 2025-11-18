using ClientesAPI.Core.DTOs;
using ClientesAPI.Core.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IValidator<ClienteCreacionDto> _creacionValidator;
        private readonly IValidator<ClienteActualizacionDto> _actualizacionValidator;

        public ClientesController(
            IClienteService clienteService,
            IValidator<ClienteCreacionDto> creacionValidator,
            IValidator<ClienteActualizacionDto> actualizacionValidator)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _creacionValidator = creacionValidator ?? throw new ArgumentNullException(nameof(creacionValidator));
            _actualizacionValidator = actualizacionValidator ?? throw new ArgumentNullException(nameof(actualizacionValidator));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> ObtenerTodos()
        {
            try
            {
                var clientes = await _clienteService.ObtenerTodosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener clientes", detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> ObtenerPorId(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener cliente", detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDto>> Crear([FromBody] ClienteCreacionDto clienteDto)
        {
            try
            {
                // Validar
                var validationResult = await _creacionValidator.ValidateAsync(clienteDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { propiedad = e.PropertyName, mensaje = e.ErrorMessage });
                    return BadRequest(new { mensaje = "Errores de validación", errores = errors });
                }

                var cliente = await _clienteService.CrearAsync(clienteDto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear cliente", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDto>> Actualizar(int id, [FromBody] ClienteActualizacionDto clienteDto)
        {
            try
            {
                // Validar
                var validationResult = await _actualizacionValidator.ValidateAsync(clienteDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { propiedad = e.PropertyName, mensaje = e.ErrorMessage });
                    return BadRequest(new { mensaje = "Errores de validación", errores = errors });
                }

                var cliente = await _clienteService.ActualizarAsync(id, clienteDto);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar cliente", detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _clienteService.EliminarAsync(id);
                if (!resultado)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar cliente", detalle = ex.Message });
            }
        }
    }
}

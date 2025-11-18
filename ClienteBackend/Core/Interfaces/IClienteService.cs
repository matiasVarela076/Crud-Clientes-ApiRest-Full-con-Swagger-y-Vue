using ClientesAPI.Core.DTOs;

namespace ClientesAPI.Core.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<ClienteDto> CrearAsync(ClienteCreacionDto clienteDto);
        Task<ClienteDto?> ActualizarAsync(int id, ClienteActualizacionDto clienteDto);
        Task<bool> EliminarAsync(int id);
    }
}

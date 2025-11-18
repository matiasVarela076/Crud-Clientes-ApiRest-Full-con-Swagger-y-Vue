using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClientesAPI.Core.DTOs;
using ClientesAPI.Core.Entities;
using ClientesAPI.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClientesAPI.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IRepository<Cliente> clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync()
        {
            IEnumerable<Cliente> clientes = await _clienteRepository.GetAsync(c => c.ACTIVO);
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<IEnumerable<ClienteDto>> BuscarPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Enumerable.Empty<ClienteDto>();

            IEnumerable<Cliente> clientes = await _clienteRepository.GetAsync(c => 
                c.ACTIVO && 
                (c.NOMBRE.Contains(nombre, StringComparison.OrdinalIgnoreCase) || 
                 c.APELLIDO.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            );

            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            IEnumerable<Cliente> clientes = await _clienteRepository.GetAsync(c => c.ID == id && c.ACTIVO);
            Cliente? cliente = clientes.FirstOrDefault();
            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> CrearAsync(ClienteCreacionDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto));

            bool existeCuit = (await _clienteRepository.GetAsync(c => c.CUIT == clienteDto.Cuit)).Any();
            if (existeCuit)
                throw new InvalidOperationException("Ya existe un cliente con el mismo CUIT.");

            Cliente cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.FECHA_CREACION = DateTime.Now;
            cliente.ACTIVO = true;

            await _clienteRepository.AddAsync(cliente);
            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<bool> ActualizarAsync(int id, ClienteActualizacionDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto));

            Cliente? cliente = (await _clienteRepository.GetAsync(c => c.ID == id && c.ACTIVO)).FirstOrDefault();
            if (cliente == null)
                return false;

            _mapper.Map(clienteDto, cliente);
            await _clienteRepository.UpdateAsync(cliente);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            Cliente? cliente = (await _clienteRepository.GetAsync(c => c.ID == id && c.ACTIVO)).FirstOrDefault();
            if (cliente == null)
                return false;

            cliente.ACTIVO = false;
            await _clienteRepository.UpdateAsync(cliente);
            return true;
        }
    }
}

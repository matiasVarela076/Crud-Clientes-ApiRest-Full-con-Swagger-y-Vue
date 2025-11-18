using System;

namespace ClientesAPI.Core.DTOs
{
    public class ClienteDto : BaseDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Cuit { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class ClienteCreacionDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Cuit { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class ClienteActualizacionDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool? Activo { get; set; }
    }
}

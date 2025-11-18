using System;

namespace ClientesAPI.Core.Entities
{
    public class Cliente
    {
        public int ID { get; set; }
        public string? NOMBRE { get; set; }
        public string? APELLIDO { get; set; }
        public DateTime? FECHA_NACIMIENTO { get; set; }
        public string? CUIT { get; set; }
        public string? DOMICILIO { get; set; }
        public string? TELEFONO { get; set; }
        public string? EMAIL { get; set; }
        public bool ACTIVO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}

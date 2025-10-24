using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesAPI.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public required string NOMBRE { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public required string APELLIDO { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FECHA_NACIMIENTO { get; set; }

        [Required(ErrorMessage = "El CUIT es obligatorio")]
        [StringLength(20)]
        public required string CUIT { get; set; }

        [StringLength(200)]
        public string? DOMICILIO { get; set; }

        [Required(ErrorMessage = "El teléfono celular es obligatorio")]
        [StringLength(20)]
        public required string TELEFONO { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(100)]
        public required string EMAIL { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FECHA_CREACION { get; set; } = DateTime.Now;
        
        [DataType(DataType.DateTime)]
        public DateTime? FECHA_MODIFICACION { get; set; }
        
        public bool ACTIVO { get; set; } = true;
    }
}

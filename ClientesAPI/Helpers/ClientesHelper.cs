using ClientesAPI.Models;
using System.Text.RegularExpressions;

namespace ClientesAPI.Helpers
{
    /// <summary>
    /// Clase helper para validaciones de clientes
    /// </summary>
    public class ClientesHelper
    {
        /// <summary>
        /// Valida todos los campos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente a validar</param>
        /// <returns>Tupla con resultado de validación y mensaje de error</returns>
        public static (bool esValido, string mensaje) ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.NOMBRE))
                return (false, "El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.APELLIDO))
                return (false, "El apellido es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.CUIT))
                return (false, "El CUIT es obligatorio");

            if (!ValidarCuit(cliente.CUIT))
                return (false, "El CUIT no tiene un formato válido");

            if (string.IsNullOrWhiteSpace(cliente.TELEFONO))
                return (false, "El teléfono celular es obligatorio");

            if (!ValidarTelefono(cliente.TELEFONO))
                return (false, "El teléfono celular no tiene un formato válido");

            if (string.IsNullOrWhiteSpace(cliente.EMAIL))
                return (false, "El email es obligatorio");

            if (!ValidarEmail(cliente.EMAIL))
                return (false, "El email no tiene un formato válido");

            if (cliente.FECHA_NACIMIENTO.HasValue)
            {
                var validacionFecha = ValidarFechaNacimiento(cliente.FECHA_NACIMIENTO.Value);
                if (!validacionFecha.esValido)
                    return (false, validacionFecha.mensaje);
            }

            return (true, "");
        }

        /// <summary>
        /// Valida el formato del CUIT (debe tener 11 dígitos)
        /// </summary>
        /// <param name="cuit">CUIT a validar</param>
        /// <returns>True si el CUIT es válido</returns>
        public static bool ValidarCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            string cuitLimpio = Regex.Replace(cuit, @"[^\d]", "");
            return cuitLimpio.Length == 11;
        }

        /// <summary>
        /// Valida el formato del teléfono (mínimo 7 dígitos)
        /// </summary>
        /// <param name="telefono">Teléfono a validar</param>
        /// <returns>True si el teléfono es válido</returns>
        public static bool ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            string telefonoLimpio = Regex.Replace(telefono, @"[^\d\+\-\s]", "");
            return telefonoLimpio.Length >= 7;
        }

        /// <summary>
        /// Valida el formato del email
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si el email es válido</returns>
        public static bool ValidarEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida la fecha de nacimiento
        /// </summary>
        /// <param name="fechaNacimiento">Fecha de nacimiento a validar</param>
        /// <returns>Tupla con resultado de validación y mensaje de error</returns>
        public static (bool esValido, string mensaje) ValidarFechaNacimiento(DateTime fechaNacimiento)
        {
            // Validar que no sea una fecha futura
            if (fechaNacimiento > DateTime.Now)
                return (false, "La fecha de nacimiento no puede ser en el futuro");

            // Validar que no sea una fecha muy antigua (más de 150 años)
            if (fechaNacimiento < DateTime.Now.AddYears(-150))
                return (false, "La fecha de nacimiento no puede ser mayor a 150 años");

            // Validar que la persona sea mayor de edad (18 años) - opcional pero recomendado
            var edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;

            if (edad < 18)
                return (false, "El cliente debe ser mayor de 18 años");

            return (true, "");
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo")]
        public string Email { get; set; }
    }
}

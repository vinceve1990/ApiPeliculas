using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos {
    public class UsuarioRegistroDto {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}

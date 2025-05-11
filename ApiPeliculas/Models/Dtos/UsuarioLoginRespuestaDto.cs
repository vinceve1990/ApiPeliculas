namespace ApiPeliculas.Models.Dtos {
    public class UsuarioLoginRespuestaDto {
        public Usuario usuario {  get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}

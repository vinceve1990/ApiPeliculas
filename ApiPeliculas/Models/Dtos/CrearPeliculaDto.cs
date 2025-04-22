namespace ApiPeliculas.Models.Dtos {
    public class CrearPeliculaDto {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciséis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public int CategoriaId { get; set; }
    }
}

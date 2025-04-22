using ApiPeliculas.Models;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio {
        ICollection<Pelicula> GetPeliculas();

        ICollection<Pelicula> GetPeliculasCategoria(int catId);

        IEnumerable<Pelicula> BuscarPelicula(string nombre);

        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(int id);
        bool ExistePelicula(string nombre);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizaPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool Guardar();
    }
}

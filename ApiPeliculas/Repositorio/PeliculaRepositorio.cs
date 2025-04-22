using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool ActualizaPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;

            var pelExiste = _db.Peliculas.Find(pelicula.Id);

            if (pelExiste != null) {
                _db.Entry(pelExiste).CurrentValues.SetValues(pelicula);
            } else {
                _db.Peliculas.Update(pelicula);
            }

            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Remove(pelicula);

            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre) {
            IQueryable<Pelicula> query = _db.Peliculas;

            if (!string.IsNullOrEmpty(nombre)) {
                query = query.Where(c => c.Nombre.Contains(nombre) || c.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Peliculas.Add(pelicula);

            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
            return _db.Peliculas.Any(c=> c.Id == id);
        }

        public bool ExistePelicula(string nombre)
        {
            bool val = _db.Peliculas.Any(c => c.Nombre.ToLower().Trim()==nombre.ToLower().Trim());

            return val;
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _db.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _db.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasCategoria(int catId) {
            return _db.Peliculas.Include(c => c.Categoria).Where(c => c.CategoriaId == catId).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

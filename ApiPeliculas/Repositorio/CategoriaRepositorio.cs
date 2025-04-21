using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool ActualizaCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;

            var catExiste = _db.Categorias.Find(categoria.Id);

            if (catExiste != null)
            {
                _db.Entry(catExiste).CurrentValues.SetValues(categoria);
            }
            else { 
                _db.Categorias.Update(categoria);
            }

            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categorias.Remove(categoria);

            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Add(categoria);

            return Guardar();
        }

        public bool ExisteCategoria(int id)
        {
            return _db.Categorias.Any(c=> c.Id == id);
        }

        public bool ExisteCategoria(string nombre)
        {
            bool val = _db.Categorias.Any(c => c.Nombre.ToLower().Trim()==nombre.ToLower().Trim());

            return val;
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            return _db.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _db.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

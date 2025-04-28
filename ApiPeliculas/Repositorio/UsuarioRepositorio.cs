using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio {
    public class UsuarioRepositorio : IUsuarioRepositorio {
        private readonly ApplicationDbContext _db;

        public UsuarioRepositorio(ApplicationDbContext db) {
            _db = db;
        }

        public Usuario GetUsuario(int UsuarioId) {
            return _db.Usuarios.FirstOrDefault(c => c.Id == UsuarioId);
        }

        public ICollection<Usuario> GetUsuarios() {
            return _db.Usuarios.OrderBy(c => c.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string usuario) {
            var usuarioBD = _db.Usuarios.FirstOrDefault(c => c.NombreUsuario == usuario);
            
            if (usuarioBD != null) { 
                return true;
            }
            
            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto) {
            var passEncrip = obtenermd5(usuarioLoginDto.Password);
            var usuario = _db.Usuarios.FirstOrDefault(
                u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                && u.Password == passEncrip
                );

            if(usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    usuario = null
                };
            }

            var manejadoToken = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(claveSecreta);
            return new UsuarioLoginRespuestaDto();
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto) {
            var passEncrip = obtenermd5(usuarioRegistroDto.Password);

            Usuario usuario = new Usuario() { 
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passEncrip,
                Nombre = usuarioRegistroDto.Nombre,
                Rol = usuarioRegistroDto.Rol
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            usuario.Password = passEncrip;

            return usuario;
        }

        public static string obtenermd5(string val) {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(val);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++) {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
    }
}

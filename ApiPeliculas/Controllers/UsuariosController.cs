using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRep;
        private readonly IMapper _imap;
        protected RespuestaAPI _respuestaAPI;

        public UsuariosController(IUsuarioRepositorio usRep, IMapper imap)
        {
            _usRep = usRep;
            _imap = imap;
            this._respuestaAPI = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRep.GetUsuarios();

            var listaUsuarioDto = new List<UsuarioDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuarioDto.Add(_imap.Map<UsuarioDto>(lista));
            }

            return Ok(listaUsuarioDto);
        }

        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsu = _usRep.GetUsuario(usuarioId);

            if (itemUsu == null)
            {
                return NotFound();
            }

            var itemUsuDto = _imap.Map<UsuarioDto>(itemUsu);

            return Ok(itemUsuDto);
        }

        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool valNomUser = _usRep.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if (!valNomUser) {
                _respuestaAPI.statusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMensaje.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaAPI);
            }

            var usuario = await _usRep.Registro(usuarioRegistroDto);
            if (usuario == null) {
                _respuestaAPI.statusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMensaje.Add("Error en el registro");
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.statusCode = System.Net.HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;

            return Ok(_respuestaAPI);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var resLogin = await _usRep.Login(usuarioLoginDto);
            if (resLogin.usuario == null || string.IsNullOrEmpty(resLogin.Token))
            {
                _respuestaAPI.statusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMensaje.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_respuestaAPI);
            }
                        
            _respuestaAPI.statusCode = System.Net.HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            _respuestaAPI.Result = resLogin;
            
            return BadRequest(_respuestaAPI);
        }
    }
}

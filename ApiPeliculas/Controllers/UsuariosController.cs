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

        public UsuariosController(IUsuarioRepositorio usRep, IMapper imap)
        {
            _usRep = usRep;
            _imap = imap;
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
    }
}

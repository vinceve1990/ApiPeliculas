using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers {
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase {
        private readonly IPeliculaRepositorio _pelRep;
        private readonly IMapper _imap;

        public PeliculasController(IPeliculaRepositorio pelRep, IMapper imap) {
            _pelRep = pelRep;
            _imap = imap;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas() {
            var listaPeliculas = _pelRep.GetPeliculas();

            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var lista in listaPeliculas) {
                listaPeliculasDto.Add(_imap.Map<PeliculaDto>(lista));
            }

            return Ok(listaPeliculasDto);
        }

        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId) {
            var itemPel = _pelRep.GetPelicula(peliculaId);

            if (itemPel == null) {
                return NotFound();
            }

            var itemPelDto = _imap.Map<PeliculaDto>(itemPel);

            return Ok(itemPelDto);
        }

        [HttpPost]
        [ProducesResponseType(201,Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPelicula([FromBody] CrearPeliculaDto crearPeliculaDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (crearPeliculaDto == null) {
                return BadRequest(ModelState);
            }

            if (_pelRep.ExistePelicula(crearPeliculaDto.Nombre)) {
                ModelState.AddModelError("", "La Pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pel = _imap.Map<Pelicula>(crearPeliculaDto);

            if (!_pelRep.CrearPelicula(pel)) {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {pel.Nombre}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pel.Id }, pel);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null || peliculaId != peliculaDto.Id) {
                return BadRequest(ModelState);
            }

            var pelExiste = _pelRep.GetPelicula(peliculaId);

            if (pelExiste == null) {
                return NotFound($"No se encontro la pelicula con ID {peliculaId}");
            }

            var pel = _imap.Map<Pelicula>(peliculaDto);

            if (!_pelRep.ActualizaPelicula(pel)) {
                ModelState.AddModelError("", $"Algo salio mal Actualizando el registro {pel.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarPelicula(int peliculaId) {
            if (!_pelRep.ExistePelicula(peliculaId)) {
                return NotFound();
            }

            var pel = _pelRep.GetPelicula(peliculaId);

            if (!_pelRep.BorrarPelicula(pel)) {
                ModelState.AddModelError("", $"Algo salio mal Borrando el registro {pel.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetPaliculasCat/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPaliculasCat(int categoriaId) {
            var listPel = _pelRep.GetPeliculasCategoria(categoriaId);

            if(listPel == null) {
                return NotFound();
            }

            var itemPel = new List<PeliculaDto>();

            foreach (var pelicula in listPel) {
                itemPel.Add(_imap.Map<PeliculaDto>(pelicula));
            }

            return Ok(itemPel);
        }

        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string nombre) {
            try {
                var resultado = _pelRep.BuscarPelicula(nombre);
                
                if (resultado.Any()) {
                    return Ok(resultado);
                }

                return NotFound();
            } catch (Exception ex) { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

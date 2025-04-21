using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiPeliculas.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _catRep;
        private readonly IMapper _imap;

        public CategoriasController(ICategoriaRepositorio catRep, IMapper imap)
        {
            _catRep = catRep;
            _imap = imap;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias() { 
            var listaCategorias = _catRep.GetCategorias();

            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_imap.Map<CategoriaDto>(lista));
            }

            return Ok(listaCategoriasDto);
        }

        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCat = _catRep.GetCategoria(categoriaId);

            if (itemCat == null)
            {
                return NotFound();
            }

            var itemCatDto = _imap.Map<CategoriaDto>(itemCat);

            return Ok(itemCatDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_catRep.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La Categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var cat = _imap.Map<Categoria>(crearCategoriaDto);

            if (!_catRep.CrearCategoria(cat)) {
                ModelState.AddModelError("",$"Algo salio mal guardando el registro {cat.Nombre}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { categoriaId = cat.Id}, cat);
        }

        [HttpPatch("{categoriaId:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var cat = _imap.Map<Categoria>(categoriaDto);

            if (!_catRep.ActualizaCategoria(cat))
            {
                ModelState.AddModelError("", $"Algo salio mal Actualizando el registro {cat.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{categoriaId:int}", Name = "ActualizarCategorias")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategorias(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var catExiste = _catRep.GetCategoria(categoriaId);

            if(catExiste == null)
            {
                return NotFound($"No se encontro la categoria con ID {categoriaId}");
            }

            var cat = _imap.Map<Categoria>(categoriaDto);

            if (!_catRep.ActualizaCategoria(cat))
            {
                ModelState.AddModelError("", $"Algo salio mal Actualizando el registro {cat.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_catRep.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            var cat = _catRep.GetCategoria(categoriaId);

            if (!_catRep.BorrarCategoria(cat))
            {
                ModelState.AddModelError("", $"Algo salio mal Borrando el registro {cat.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

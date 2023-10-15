using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Dominio.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
	[Route("autor")]
	public class AutorController : ApiBaseController
	{
		private readonly IAutorRepositorio _autorRepositorio;
		public AutorController(IAutorRepositorio autorRepositorio)
		{
			_autorRepositorio = autorRepositorio;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string nome = "", int? pageIndex = null, int? pageSize = null)
		{
			var autores =  _autorRepositorio.ObterTodos(nome, pageIndex, pageSize);

			return Ok(autores);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] Guid id)
		{
			var autor = await _autorRepositorio.ObterPorId(id);

			if (autor == null)
			{
				return NotFoundResult();
			}

			return Ok(autor);
		}

		[HttpPost]
		public async Task<IActionResult> Inserir([FromBody] AutorViewModel autorViewModel)
		{
			var autor = new Autor()
			{
				IdAutor = autorViewModel.IdAutor,
				Nome = autorViewModel.Nome
			};

			await _autorRepositorio.Inserir(autor);

			return Created("/autor/" + autor.IdAutor, autor);
		}

		[HttpPut]
		public async Task<IActionResult> Editar([FromBody] AutorViewModel autorViewModel)
		{
			if (autorViewModel.IdAutor == null || autorViewModel.IdAutor == Guid.Empty)
			{
				return BadRequestResultIdInvalid();
			}

			var autor = new Autor()
			{
				IdAutor = autorViewModel.IdAutor,
				Nome = autorViewModel.Nome
			};

			await _autorRepositorio.Editar(autor);

			return Ok(autor);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Excluir([FromRoute] Guid? id)
		{
			if (id == null || id == Guid.Empty)
			{
				return BadRequestResultIdInvalid();
			}

			await _autorRepositorio.Excluir(id.Value);

			return NoContent();
		}
	}
}

using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Dominio.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
	[Route("editora")]
	public class EditoraController : ApiBaseController
	{
		private readonly IEditoraRepositorio _editoraRepositorio;
		public EditoraController(IEditoraRepositorio editoraRepositorio) 
		{ 
			_editoraRepositorio = editoraRepositorio;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var editoras = await _editoraRepositorio.ObterTodos();

			return Ok(editoras);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] Guid id)
		{
			var editora = await _editoraRepositorio.ObterPorId(id);

			if(editora == null)
			{
				return NotFoundResult();
			}

			return Ok(editora);
		}

		[HttpPost]
		public async Task<IActionResult> Inserir([FromBody] EditoraViewModel editoraViewModel)
		{
			var editora = new Editora()
			{
				IdEditora = editoraViewModel.IdEditora,
				Nome =  editoraViewModel.Nome
			};

			await _editoraRepositorio.Inserir(editora);

			return Created("/editora/"+editora.IdEditora, editora);
		}

		[HttpPut]
		public async Task<IActionResult> Editar([FromBody] EditoraViewModel editoraViewModel)
		{
			if(editoraViewModel.IdEditora == null || editoraViewModel.IdEditora == Guid.Empty)
			{
				return BadRequestResultIdInvalid();
			}

			var editora = new Editora()
			{
				IdEditora = editoraViewModel.IdEditora,
				Nome = editoraViewModel.Nome
			};

			await _editoraRepositorio.Editar(editora);

			return Ok(editora);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Excluir([FromRoute] Guid? id)
		{
			if(id == null || id == Guid.Empty)
			{
				return BadRequestResultIdInvalid();
			}

			await _editoraRepositorio.Excluir(id.Value);

			return NoContent();
		}
	}
}

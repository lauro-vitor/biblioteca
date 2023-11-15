using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/editora")]
    public class EditoraController : ApiBaseController
    {
        private readonly EditoraRepositorio _editoraRepositorio;

        public EditoraController(EditoraRepositorio editoraRepositorio)
        {
            _editoraRepositorio = editoraRepositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _editoraRepositorio.Obter());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _editoraRepositorio.ObterEditoraViewModelPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] EditoraViewModel editoraViewModel)
        {
            var resultado = await _editoraRepositorio.Inserir(editoraViewModel);

            return Created("/editora/" + resultado.IdEditora, resultado);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] EditoraViewModel editoraViewModel)
        {
            var resultado = await _editoraRepositorio.Editar(editoraViewModel);

            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            await _editoraRepositorio.Excluir(id);

            return NoContent();
        }
    }
}

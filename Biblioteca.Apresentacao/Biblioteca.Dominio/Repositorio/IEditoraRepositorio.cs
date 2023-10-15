﻿using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Repositorio
{
	public interface IEditoraRepositorio : IRepositorioBase<Editora>
	{
		public Task<List<Editora>> ObterTodos();
	}
}

﻿using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class Autor
	{
		private Guid _idAutor;
        private string? _nome;

        [Key]
		public Guid? IdAutor
		{
			get 
			{ 
				return _idAutor; 
			}

			set
			{
				if(value == null)
				{
					_idAutor = Guid.NewGuid();
					return;
				}

				if(value == Guid.Empty)
				{
					throw new BibliotecaException("IdAutor: Invalido");
				}

				_idAutor = value.Value;
			}
		}

		public string? Nome
		{
			get
			{
				return _nome;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new BibliotecaException("Nome: Obrigatorio");
				}

				_nome = value.Trim();
			}
		}
	}
}

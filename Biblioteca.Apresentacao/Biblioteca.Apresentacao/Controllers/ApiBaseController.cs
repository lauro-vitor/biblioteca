using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
	public class ApiBaseController : Controller
	{
		protected ObjectResult BadRequestResultIdInvalid()
		{
			var erroResponse = new ErroViewModel()
			{
				Status = StatusCodes.Status400BadRequest,

				Mensagem = "Requisição inválida",

				Erros = new Dictionary<string, List<string>>
				{
					{
						"id",
						new List<string>
						{
							"Id informado é inválido"
						}
					}
				}
			};

			return BadRequest(erroResponse);
		}

		protected ObjectResult BadRequestResult()
		{
			var erroResponse = new ErroViewModel()
			{
				Status = StatusCodes.Status400BadRequest,

				Mensagem = "Requisição inválida",

				Erros = new Dictionary<string, List<string>>
				{
					{
						"mensagem",
						new List<string>
						{
							"objeto no corpo da requisição é invalido"
						}
					}
				}
			};

			return BadRequest(erroResponse);
		}

		protected ObjectResult NotFoundResult()
		{
			var erroResponse = new ErroViewModel()
			{
				Status = StatusCodes.Status404NotFound,

				Mensagem = "Não encontrado",

				Erros = new Dictionary<string, List<string>>
				{
					{
						"id",
						new List<string>
						{
							"Não foi encontrado nehum registro para esse ID"
						}
					}
				}
			};

			return NotFound(erroResponse);
		}

	}
}

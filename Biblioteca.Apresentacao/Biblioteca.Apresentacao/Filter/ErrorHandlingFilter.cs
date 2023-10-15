using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Biblioteca.Apresentacao.Filter
{
	public class ErrorHandlingFilter : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			context.ExceptionHandled = true;

			var exception = context.Exception;

			if(exception is BibliotecaException)
			{
				context.Result = BadRequestResult((BibliotecaException) exception);
			}
			else
			{
				context.Result = InternalServerErrorResult(exception);
			}
		}

		private static ObjectResult InternalServerErrorResult(Exception exception)
		{
			var erroResponse = new ErroViewModel()
			{
				Status = StatusCodes.Status500InternalServerError,

				Mensagem = "Ocorreu algum erro interno",

				Erros = new Dictionary<string, List<string>>
				{
					{
						"message",
						new List<string> { exception.Message }
					},
					{
						"stackTrace",
						new List<string> { exception.StackTrace ?? "" }
					}
				}
			};

			var objectResult = new ObjectResult(erroResponse)
			{
				StatusCode = StatusCodes.Status500InternalServerError
			};

			return objectResult;
		}

		private static ObjectResult BadRequestResult(BibliotecaException exception)
		{
			var erroResponse = new ErroViewModel()
			{
				Mensagem = "Erros de validação",
				Status = StatusCodes.Status400BadRequest
			};

			erroResponse.AtribuirErro("mensagem", exception.Message);

			return new BadRequestObjectResult(erroResponse); 
		}

	}
}

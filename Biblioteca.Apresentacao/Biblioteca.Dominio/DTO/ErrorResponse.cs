using System.Net;

namespace Biblioteca.Dominio.DTO
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Mensagem { get; set; }

        public Dictionary<string, List<string>> Erros { get; set; }

        public ErrorResponse()
        {
            Status = 0;
            Mensagem = "";
            Erros = new Dictionary<string, List<string>>();
        }

        public ErrorResponse(int status, string mensagem, Dictionary<string, List<string>> erros)
        {
            Status = status;
            Mensagem = mensagem;
            Erros = erros;
        }
        public ErrorResponse(Exception exception)
        {
            Mensagem = "Ocorreu algum erro interno";
            Status = (int)HttpStatusCode.InternalServerError;
            Erros = new Dictionary<string, List<string>>();
            AtribuirErro("exception", exception.Message);
        }

        public void AtribuirErro(string campo, string mensagem)
        {
            List<string> erroCampo;

            if (string.IsNullOrEmpty(campo) || string.IsNullOrEmpty(mensagem))
            {
                return;
            }

            campo = campo.Trim().ToLower();


            try
            {
                erroCampo = Erros[campo];
            }
            catch (KeyNotFoundException)
            {
                Erros.Add(campo, new List<string>());
                erroCampo = Erros[campo];
            }

            erroCampo.Add(mensagem);
        }

        public void AtribuirErroBadRequest(Dictionary<string, List<string>> erros)
        {
            Status = (int)HttpStatusCode.BadRequest;
            Mensagem = "Ocorreram alguns erros de validação";
            Erros = erros;
        }
        public void AtribuirErroBadRequest(string campo, string mensagem)
        {
            Status = (int)HttpStatusCode.BadRequest;
            Mensagem = "Ocorreram alguns erros de validação";
            AtribuirErro(campo, mensagem);
        }

        public void AtribuirErroNotFound(string campo, string mensagem)
        {
            Status = (int)HttpStatusCode.NotFound;
            Mensagem = "Ocorreram alguns erros de validação";
            AtribuirErro(campo, mensagem);
        }

    }
}

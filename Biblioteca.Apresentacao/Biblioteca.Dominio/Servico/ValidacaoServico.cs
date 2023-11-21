using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Servico
{
    public static class ValidacaoServico
    {
        public static string ValidarNome(string propriedade, string? valor)
        {
            var valorAux = valor?.Trim();

            if (string.IsNullOrWhiteSpace(valorAux))
            {
                throw new BibliotecaException($"{propriedade}: Obrigatório");
            }

            if (valorAux.Length < 3)
            {
                throw new BibliotecaException($"{propriedade}: deve possuir ao menos 3 caracteres");
            }

            return valorAux.Trim();
        }


        public static DateOnly ValidarData(string propriedade, DateOnly data)
        {
            if (data == new DateOnly())
            {
                throw new BibliotecaException($"{propriedade}: Inválido");
            }

            if (data >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new BibliotecaException($"{propriedade}: Deve ser menor que a data atual");
            }

            return data;
        }


    }
}

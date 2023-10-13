namespace Biblioteca.Dominio.ViewModel
{
	public class ErroViewModel
	{
		public int Status { get; set; }
		public string Mensagem { get; set; }

		public Dictionary<string, List<string>> Erros { get; set; }

		public ErroViewModel()
		{
			Status = 0;
			Mensagem = "";
			Erros = new Dictionary<string, List<string>>();
		}

		public void AtribuirErro(string campo, string mensagem)
		{
			List<string> erroCampo;

			if (string.IsNullOrEmpty(campo) || string.IsNullOrEmpty(mensagem))
			{
				return;
			}

			campo = campo.Trim();


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
	}
}

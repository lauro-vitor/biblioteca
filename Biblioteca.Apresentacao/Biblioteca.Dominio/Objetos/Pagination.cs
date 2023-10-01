namespace Biblioteca.Dominio.Objetos
{
	public class Pagination<T>
	{
		public int PageIndex { get; }
		public int PageSize { get; }
		public int TotalCount { get; }
		public int TotalPages { get; }
		public List<T> Data { get; }

		public Pagination(IQueryable<T> source, int? pageIndex, int? pageSize)
		{
			if (pageIndex != null && pageIndex <= 0)
			{
				throw new BibliotecaException("pageIndex: Pagina atual deve ser um número positivo");
			}

			if (pageSize != null && pageSize <= 0)
			{
				throw new BibliotecaException("pageSize: Tamanho da página deve ser um número positivo");
			}

			if (pageIndex == null)
			{
				PageIndex = 1;
			}
			else
			{
				PageIndex = pageIndex.Value;
			}

			TotalCount = source.Count();

			if (pageSize == null)
			{
				PageSize = TotalCount;
			}
			else
			{
				PageSize = pageSize.Value;
			}

			TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

			Data = source
				.Skip((PageIndex - 1) * PageSize)
				.Take(PageSize)
				.ToList();
		}
		public bool HasPreviousPage
		{
			get
			{
				return (PageIndex > 1);
			}
		}

		public bool HasNextPage
		{
			get
			{
				return (PageIndex < TotalPages);
			}
		}

	}
}

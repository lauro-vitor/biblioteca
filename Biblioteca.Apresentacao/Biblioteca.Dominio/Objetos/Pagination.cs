namespace Biblioteca.Dominio.Objetos
{
    public class Pagination<T>
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public List<T> Data { get; }

        public Pagination(IQueryable<T> source, int? pageIndex, int? pageSize)
        {
            if (source == null)
                throw new BibliotecaException("Source: Fonte de dados não pode ser nula");

            TotalCount = source.Count();

            if (pageIndex != null && pageIndex >= 0 && pageSize != null && pageSize > 0)
            {
                PageIndex = pageIndex.Value;
                PageSize = pageSize.Value;
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                Data = source
                    .Skip(PageIndex * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            else
            {
                PageIndex = 0;
                TotalPages = 1;
                PageSize = TotalCount;
                Data = source.ToList();
            }

        }
    }
}

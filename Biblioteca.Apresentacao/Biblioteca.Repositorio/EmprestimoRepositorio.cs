using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.AlunoVM;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Dominio.ViewModel.Emprestimo;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Dominio.ViewModel.Livro;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class EmprestimoRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public EmprestimoRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task RealizarEmprestimo(Guid? idLivro, Guid? idAluno)
        {
            if (idLivro == null || idLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            if (idAluno == null || idAluno == Guid.Empty)
                throw new BibliotecaException("idAluno: inválido");

            var existeEmprestimo = await _context.Emprestimo
                .AnyAsync(e => e.IdLivro == idLivro
                            && e.IdAluno == idAluno
                            && e.DataDevolucao == null);

            if (existeEmprestimo)
                throw new BibliotecaException("Já existe um empréstimo em aberto deste livro para o aluno");

            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == idLivro);

            var aluno = await _context.Aluno.FirstOrDefaultAsync(a => a.IdAluno == idAluno);

            var emprestimo = new Emprestimo();

            emprestimo.RealizarEmprestimo(livro, aluno);

            await _context.Emprestimo.AddAsync(emprestimo);

            await _context.SaveChangesAsync();
        }

        public async Task RealizarDevolucao(Guid? idEmprestimo)
        {
            if (idEmprestimo == null || idEmprestimo == Guid.Empty)
                throw new BibliotecaException("idEmprestimo: inválido");

            var emprestimo = await _context.Emprestimo
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.IdEmprestimo == idEmprestimo);

            if (emprestimo == null)
                throw new BibliotecaException("Empréstimo não encontrado para realizar a devolução");

            emprestimo.RealizarDevolucao();

            await _context.SaveChangesAsync();
        }
#nullable disable
        public Pagination<EmprestimoViewModel> Obter(EmprestimoParametroViewModel parametro)
        {
            var query = _context.Emprestimo
                .Include(e => e.Livro).ThenInclude(l => l.Editora)
                .Include(e => e.Aluno)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parametro.AlunoNome))
            {
                var alunoNome = parametro.AlunoNome.ToLower().Trim();
                query = query.Where(e => e.Aluno.Nome.ToLower().Trim().Contains(alunoNome));
            }

            if (!string.IsNullOrWhiteSpace(parametro.AlunoMatricula))
            {
                var alunoMatricula = parametro.AlunoMatricula.ToLower().Trim();
                query = query.Where(e => e.Aluno.Matricula.ToLower().Trim().Contains(alunoMatricula));
            }

            if (!string.IsNullOrWhiteSpace(parametro.LivroTitulo))
            {
                var livroTitulo = parametro.LivroTitulo.ToLower().Trim();
                query = query.Where(e => e.Livro.Titulo.ToLower().Trim().Contains(livroTitulo));
            }

            if (!string.IsNullOrWhiteSpace(parametro.DataEmprestimoInicio))
            {
                var dataEmprestimoInicio = DateTime.MinValue;

                if (DateTime.TryParse(parametro.DataEmprestimoInicio, out dataEmprestimoInicio))
                    query = query.Where(e => e.DataEmprestimo >= dataEmprestimoInicio);
            }

            if (!string.IsNullOrWhiteSpace(parametro.DataEmprestimoFim))
            {
                var dataEmprestimoFim = DateTime.MinValue;

                if (DateTime.TryParse(parametro.DataEmprestimoFim, out dataEmprestimoFim))
                    query = query.Where(e => e.DataEmprestimo <= dataEmprestimoFim);    
            }

            if (parametro.SomenteEmAbertos ?? false)
            {
                query = query.Where(e => e.DataDevolucao == null);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(parametro.DataDevolucaoInicio))
                {
                    var dataDevolucaoInicio = DateTime.MinValue;

                    if (DateTime.TryParse(parametro.DataDevolucaoInicio, out dataDevolucaoInicio))
                        query = query.Where(e => e.DataDevolucao >= dataDevolucaoInicio);
                }

                if (!string.IsNullOrWhiteSpace(parametro.DataDevolucaoFim))
                {
                    var dataDevolucaoFim = DateTime.MinValue;

                    if (DateTime.TryParse(parametro.DataDevolucaoFim, out dataDevolucaoFim))
                        query = query.Where(e => e.DataDevolucao <= dataDevolucaoFim);
                }
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<Emprestimo, object>> funcSort = null;

                switch (parametro.SortProp)
                {
                    case "alunoNome":
                        funcSort = e => e.Aluno.Nome;
                        break;
                    case "alunoMatricula":
                        funcSort = e => e.Aluno.Matricula;
                        break;
                    case "livroTitulo":
                        funcSort = e => e.Livro.Titulo;
                        break;
                    case "dataDevolucao":
                        funcSort = e => e.DataDevolucao;
                        break;
                    default:
                        funcSort = e => e.DataEmprestimo;
                        break;
                }

                if ("desc".StartsWith(parametro.SortDirection?.ToLower()?.Trim() ?? ""))
                    query = query.OrderByDescending(funcSort);
                else
                    query = query.OrderBy(funcSort);
            }

            var queryEmprestimoVM = query.Select(e => new EmprestimoViewModel
            {
                IdEmprestimo = e.IdEmprestimo,
                DataDevolucao = e.DataDevolucao.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                DataEmprestimo = e.DataEmprestimo.ToString("dd/MM/yyyy HH:mm:ss"),
                AlunoMatricula = e.Aluno.Matricula,
                AlunoNome = e.Aluno.Nome,
                LivroTitulo = e.Livro.Titulo,
                LivroEditora = e.Livro.Editora.Nome
            });

            var paginacao = new Pagination<EmprestimoViewModel>(queryEmprestimoVM, parametro.PageSize, parametro.PageIndex);

            return paginacao;
        }

        public async Task<EmprestimoDetalheViewModel> ObterPorId(Guid idEmprestimo)
        {
            var result = await _context.Emprestimo
                .Select(e => new EmprestimoDetalheViewModel
                {
                    IdEmprestimo = e.IdEmprestimo,
                    DataEmprestimo = e.DataEmprestimo.ToString("dd/MM/yyyy HH:mm:ss"),
                    DataDevolucao = e.DataDevolucao.Value.ToString("dd/MM/yyyy HH:mm:ss"),

                    Aluno = new AlunoQueryViewModel
                    {
                        IdAluno = e.Aluno.IdAluno,
                        Nome = e.Aluno.Nome,
                        Matricula = e.Aluno.Matricula,
                        DataNascimento = e.Aluno.DataNascimento,
                        Desativado = e.Aluno.Desativado,
                        Sexo = e.Aluno.Sexo
                    },

                    Livro = new LivroViewModel
                    {
                        IdLivro = e.Livro.IdLivro,
                        Titulo = e.Livro.Titulo,
                        DataPublicacao = e.Livro.DataPublicacao.Value.ToString("dd/MM/yyyy"),
                        QuantidadeEstoque = e.Livro.QuantidadeEstoque,
                        Edicao = e.Livro.Edicao,
                        Volume = e.Livro.Volume,

                        Editora = new EditoraViewModel
                        {
                            IdEditora = e.Livro.IdEditora,
                            Nome = e.Livro.Editora.Nome,
                        },

                        Autores = e.Livro.LivroAutores.Select(la => new AutorViewModel
                        {
                            IdAutor = la.Autor.IdAutor,
                            Nome = la.Autor.Nome,
                        }).ToList(),

                        Generos = e.Livro.LivroGeneros.Select(lg => new GeneroViewModel()
                        {
                            IdGenero = lg.IdGenero,
                            Nome = lg.Genero.Nome,
                        }).ToList()
                    }
                })
                .FirstOrDefaultAsync(e => e.IdEmprestimo == idEmprestimo);

            return result;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

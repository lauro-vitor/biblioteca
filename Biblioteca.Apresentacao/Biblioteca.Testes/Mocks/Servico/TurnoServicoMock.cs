using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Servico;
using Moq;

namespace Biblioteca.Testes.Mocks.Servico
{
    public class TurnoServicoMock
    {
        private readonly Mock<ITurnoServico> _mock;

        public TurnoServicoMock()
        {
            _mock = new Mock<ITurnoServico>();
        }

        public ITurnoServico Build()
        {
            return _mock.Object;
        }

        public TurnoServicoMock Configurar_ValidarInserirTurno(ErrorResponse? errorResponse)
        {
            _mock.Setup(x => x.ValidarInserirTurno(It.IsAny<Turno>(), It.IsAny<IList<Turno>>()))
                .Returns(errorResponse);

            return this;
        }

        public TurnoServicoMock Configurar_ValidarEditarTurno(ErrorResponse? errorResponse)
        {
            _mock.Setup(x => x.ValidarEditarTurno(It.IsAny<Turno>(), It.IsAny<IList<Turno>>()))
                .Returns(errorResponse);

            return this;
        }

        public TurnoServicoMock Configurar_ValidarTurnoNaoExiste(ErrorResponse? errorResponse)
        {
            _mock.Setup(x => x.ValidarTurnoNaoExiste(It.IsAny<Guid>(), It.IsAny<IList<Turno>>()))
                .Returns(errorResponse);

            return this;
        }
    }
}

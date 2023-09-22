﻿using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Testes.Mocks.Repositorio
{
    public class TurnoRepositorioMock
    {
        private readonly Mock<ITurnoRepositorio> _mock;
        public TurnoRepositorioMock()
        {
            _mock = new Mock<ITurnoRepositorio>();  
        }

        public static TurnoRepositorioMock Instance()
        {
            return new TurnoRepositorioMock();
        }

        public ITurnoRepositorio Build()
        {
            return _mock.Object;
        }

        public TurnoRepositorioMock Configurar_Obter(IList<Turno> turnos)
        {
            _mock.Setup(x => x.Obter().Result)
                .Returns(turnos);

            return this;
        }

        public TurnoRepositorioMock Configurar_ObterComExcessao()
        {
            _mock.Setup(x => x.Obter())
                .ThrowsAsync(new Exception("Excessão ao obter os turnos"));

            return this;
        }


        public TurnoRepositorioMock Configurar_ObterPorId(Turno turno)
        {
            _mock.Setup(x => x.ObterPorId(It.IsAny<Guid>()).Result)
                .Returns(turno);

            return this;
        }

        public TurnoRepositorioMock Configurar_Inserir()
        {
            _mock.Setup(x => x.Inserir(It.IsAny<Turno>()));

            return this;
        }

        public TurnoRepositorioMock Configurar_Editar()
        {
            _mock.Setup(x => x.Editar(It.IsAny<Turno>()));

            return this;
        }

        public TurnoRepositorioMock Configurar_Excluir()
        {
            _mock.Setup(x => x.Excluir(It.IsAny<Guid>()));

            return this;
        }


    }
}

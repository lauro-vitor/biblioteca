(function () {
  obterTurno();
})();

function obterTurno() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.obterIdPathParameter();

  loading.bloquear();

  turnoService
    .obterPorId(idTurno)
    .then((value) => {
      const turno = value;
      $("#idSpanTurnoNome").text(turno.nome);
    })
    .catch(() => {
      Swal.fire({
        icon: "error",
        title: "Erro",
        text: "Ocorreu algum erro ao recuperar o turno",
      });
    })
    .finally(() => {
      loading.desbloquear();
    });
}

function excluirTurnoButtonClick() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.obterIdPathParameter();

  loading.bloquear();

  turnoService
    .excluir(idTurno)
    .then(() => {
      Swal.fire({
        icon: "success",
        title: "Sucesso",
        text: "Turno excluido com sucesso",
      }).then(() => {
        window.location.href = "/turno/views/index";
      });
    })
    .catch(() => {
      Swal.fire({
        icon: "error",
        title: "Erro",
        text: "Ocorreu algum erro ao excluir o turno",
      });
    })
    .finally(() => {
      loading.desbloquear();
    });
}

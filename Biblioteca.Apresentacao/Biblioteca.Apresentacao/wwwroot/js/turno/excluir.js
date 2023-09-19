(function () {
  obterTurno();
})();

function obterTurno() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.ObterIdPathParameter();

  loading.bloquear();

  turnoService
    .obterPorId(idTurno)
    .then((value) => {
      const turno = value;
      $("#idSpanTurnoNome").text(turno.nome);
    })
    .catch((reason) => {
      const error = reason.responseJSON;

      Swal.fire({
        icon: "error",
        title: "Erro",
        text: error.mensagem,
      });
    })
    .finally(() => {
      loading.desbloquear();
    });
}

function excluirTurnoButtonClick() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.ObterIdPathParameter();

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
    .catch((reason) => {
      const error = reason.responseJSON;

      Swal.fire({
        icon: "error",
        title: "Erro",
        text: error.mensagem,
      });
    })
    .finally(() => {
      loading.desbloquear();
    });
}

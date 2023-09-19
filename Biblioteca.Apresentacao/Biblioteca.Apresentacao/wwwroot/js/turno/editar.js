(function () {
  obterTurno();
})();

function obterTurno() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.obterIdPathParameter();
  loading.bloquear();

  turnoService
    .obterPorId(idTurno)
    .then((turno) => {
      $("#idInputTurnoNome").val(turno.nome);
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

function editarTurnoButtonClick() {
  const turnoService = new TurnoService();
  const idTurno = turnoService.obterIdPathParameter();

  turnoService
    .validarTurno()
    .then((turno) => {
      loading.bloquear();

      turno.idTurno = idTurno;

      turnoService
        .editar(turno)
        .then(() => {
          Swal.fire({
            icon: "success",
            title: "Sucesso",
            text: "Turno editado com sucesso!",
          }).then(() => {
            window.location.href = "/turno/views/index";
          });
        })
        .catch(() => {
          Swal.fire({
            icon: "error",
            title: "Error",
            text: "Ocorreu algum erro ao tentar editar o turno",
          });
        })
        .finally(() => {
          loading.desbloquear();
        });
    })
    .catch(() => {});
}

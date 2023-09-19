function inserirTurnoClick() {
  const turnoService = new TurnoService();
  const inputTurnoNome = document.getElementById("idInputTurnoNome");
  const spanTurnoNome = document.getElementById("idSpanTurnoNome");

  if (inputTurnoNome.value === null || inputTurnoNome.value === "") {
    spanTurnoNome.innerHTML = "Nome do turno é obrigatório";
    return;
  }
  const turno = {
    nome: inputTurnoNome.value.trim(),
  };

  loading.bloquear();

  turnoService
    .inserirTurno(turno)
    .then((value) => {
      swal
        .fire({
          icon: "success",
          title: "Sucesso",
          text: "Turno inserido com sucesso",
        })
        .then((result) => {
          window.location.href = "/turno/views/index";
        });

      loading.desbloquear();
    })
    .catch((err) => {
      const erro = err.responseJSON;

      Swal.fire({
        icon: "error",
        title: "Erro",
        text: erro.mensagem,
      });

      loading.desbloquear();
    });
}

(function () {
    carregarTurnos();
})();

function carregarTurnos() {
  var turnoService = new TurnoService();

  loading.bloquear();

  turnoService
    .obterTurnos()
    .then((turnos) => {
      const tbody = $("#idTableTurno > tbody");

      turnos.map((turnoItem) => {
        const tr = `
                <tr>
                    <td>${turnoItem.nome}</td>
                    <td>
                        <a href="/turno/views/editar/${turnoItem.idTurno}">
                            <img src="/icons/edit.svg" />
                        </a>
                        <a href="/turno/views/excluir/${turnoItem.idTurno}">
                            <img src="/icons/delete.svg" />
                        </a>
                    </td>
                </tr>`;
        tbody.append(tr);
      });

      loading.desbloquear();
    })
    .catch((error) => {
      console.log(error);
      loading.desbloquear();
    });
}

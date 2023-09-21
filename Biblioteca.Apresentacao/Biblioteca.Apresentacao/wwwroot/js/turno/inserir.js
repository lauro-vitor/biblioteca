function inserirTurnoButtonClick() {
    const turnoService = new TurnoService();

    loading.bloquear();

    turnoService
        .validarTurno()
        .then((turno) => {
            turnoService
                .inserir(turno)
                .then(() => {
                    Swal.fire({
                        icon: "success",
                        title: "Sucesso",
                        text: "Turno inserido com sucesso",
                    }).then(() => {
                        window.location.href = "/turno/views/index";
                    });
                    loading.desbloquear();
                })
                .catch(() => {
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: "Erro ao  inserir turno",
                    });

                    loading.desbloquear();
                });
        })
        .catch(() => { });
}

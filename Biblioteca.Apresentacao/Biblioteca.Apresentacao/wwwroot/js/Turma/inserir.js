$(document).ready(function () {

    const turnoService = new TurnoService();

    loading.bloquear();

    turnoService
        .obter()
        .then(result => {
            const turnos = result;

            Turma.constuirTurnoSelectList(turnos);
        })
        .catch(reason => {
            console.error(reason);
        })
        .finally(() => {
            loading.desbloquear();
        });
});


function inserirTurmaButtonClick() {
    const turma = new Turma();

    if (turma.Valida()) {
        const turmaHttpService = new TurmaHttpService();

        loading.bloquear();

        turmaHttpService.Inserir(turma)
            .then(result => {

                Swal.fire({
                    icon: "success",
                    title: "Sucesso",
                    text: "Turma inserida com sucesso"
                }).then(() => {
                    window.location.href = "/turma/views/index";
                });
            })
            .catch(reason => {
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    Text: "Ocorreu algum erro ao inserir a turma"
                });

                console.error(reason);

            }).finnaly(() => {
                loading.desbloquear();
            });
    }

    console.log(turma);
}
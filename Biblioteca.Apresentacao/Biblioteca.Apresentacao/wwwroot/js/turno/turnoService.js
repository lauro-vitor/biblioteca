class TurnoService {
    obter() {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                accepts: "application/json",
                url: "/turno/obter",
                cache: false,
            })
                .done((value) => {
                    resolve(value);
                })
                .fail((reason) => {
                    console.error(reason);
                    reject(reason);
                });
        });
    }

    inserir(turno) {
        return new Promise((resolve, reject) => {
            $.ajax({
                headers: {
                    "Content-Type": "application/json",
                },
                type: "POST",
                url: "/turno/inserir",
                cache: false,
                data: JSON.stringify(turno),
            })
                .done((value) => {
                    resolve(value);
                })
                .fail((reason) => {
                    console.error(reason);
                    reject(reason);
                });
        });
    }

    obterPorId(idTurno) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                url: `/turno/obter/${idTurno}`,
                accepts: "application/json",
                cache: false,
            })
                .done((value) => {
                    resolve(value);
                })
                .fail((reason) => {
                    console.error(reason);
                    reject(reason);
                });
        });
    }

    excluir(idTurno) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "DELETE",
                url: `/turno/excluir/${idTurno}`,
                cache: false,
            })
                .done((value) => {
                    resolve(value);
                })
                .fail((reason) => {
                    reject(reason);
                });
        });
    }

    editar(turno) {
        return new Promise((resolve, reject) => {
            $.ajax({
                headers: {
                    "Content-Type": "application/json",
                },
                type: "PUT",
                url: "/turno/editar/",
                cache: false,
                data: JSON.stringify(turno),
            })
                .done((value) => {
                    resolve(value);
                })
                .fail((reason) => {
                    console.error(reason);
                    reject(reason);
                });
        });
    }

    obterIdPathParameter() {
        const id = window.location.pathname.split("/")[4];

        if (id == undefined || id == "")
            throw "Não foi possível encontrar id do turno";

        return id;
    }

    validarTurno() {
        return new Promise((resolve, reject) => {

            const inputTurnoNome = $("#idInputTurnoNome");

            const spanTurnoNome = $("#idSpanTurnoNome");

            if (inputTurnoNome.val() === null || inputTurnoNome.val() === "") {
                spanTurnoNome.text("Nome do turno é obrigatório");
                inputTurnoNome.css("border","1px solid red");
                return reject();
            }

            spanTurnoNome.text("");
            inputTurnoNome.css("border", "1px solid #ced4da");


            const turno = {
                nome: inputTurnoNome.val().trim(),
            };

            return resolve(turno);
        });
    }
}

class Turma {

    IdTurma = null;
    IdTurno = null;
    Nome = null;
    Sigla = null;
    Periodo = null;

    #valida = true;

    constructor() {
        this.#setTurno();
        this.#setNome();
        this.#setSigla();
        this.#setPeriodo();
    }

    Valida() {
        return this.#valida;
    }

    static constuirTurnoSelectList(turnos) {

        $("#idSelectTurno").empty();

        $("#idSelectTurno").append("<option value=''>(Selecione o turno)</option>");

        turnos.map(t => {

            const option = `<option value="${t.idTurno}">${t.nome}</option>`;

            $("#idSelectTurno").append(option);
        });
    }

    #setNome() {
        const _input = $("#idInputNome");

        const _span = $("#idSpanNome");

        const _nome = _input.val();

        let _mensagem = null;

        if (_nome === null || _nome === undefined || _nome === "") {
            _mensagem = "nome da turma &eacute; obrigat&oacute;rio";
        }
        else if (_nome.length <= 3) {
            _mensagem = "nome da turma deve possuir mais do que 3 caract&eacute;res";
        }

        if (_mensagem != null) {
            this.#inserirMensagemErro(_input, _span, _mensagem);
            this.#valida = false;
            return;
        }

        this.#limparMensagemErro(_input, _span);

        this.Nome = _nome;
    }

    #setSigla() {
        let _sigla = $("#idInputSigla").val();

        if (_sigla === undefined || _sigla === "") {
            _sigla = null;
        }

        this.Sigla = _sigla;
    }

    #setPeriodo() {
        const _input = $("#idInputPeriodo");
        const _span = $("#idSpanPeriodo");
        let _mensagem = null;

        if (_input.val() === undefined || _input.val() === null || _input.val() === "") {
            _mensagem = "per&iacute;odo &eacute obrigat&oacute;rio";
        }
        else {
            const periodoInt = parseInt(_input.val());

            if (isNaN(periodoInt)) {
                _mensagem = "per&iacute;odo deve ser um n&uacute;mero";
            }

            if (periodoInt <= 0) {
                _mensagem = "per&iacute;odo deve ser positivo";
            }
        }

        if (_mensagem !== null) {
            this.#inserirMensagemErro(_input, _span, _mensagem);
            this.#valida = false;
            return;
        }

        this.#limparMensagemErro(_input, _span);

        this.Periodo = parseInt(_input.val());
    }

    #setTurno() {
        const _idTurno = $("#idSelectTurno").val();

        if (_idTurno === "" || _idTurno === undefined || _idTurno === null) {
            this.#inserirMensagemErro($("#idSelectTurno"), $("#idSpanTurno"), "turno &eacute; obrigat&oacute;rio");
            this.#valida = false;
            return;
        }

        this.IdTurno = _idTurno;
    }

    #inserirMensagemErro(input, span, mensagem) {
        input.css("border", "1px solid red");
        span.html(mensagem).text();
    }

    #limparMensagemErro(input, span) {
        input.css("border", "1px solid #ced4da");
        span.text("");
    }

  
}

class TurmaHttpService {

    Obter() {
        return new Promise(
            function (resolve, reject) {
                $.ajax({
                    type: "GET",
                    url: "/turma/obter",
                    accepts: "application/json",
                    cache: false
                })
                    .done(result => {
                        resolve(result);
                    })
                    .fail(reason => {
                        reject(reason);
                    });
            }
        );
    }

    Inserir(turma) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "POST",
                url: "/turma/inserir",
                contentType: "application/json",
                data: JSON.stringify(turma)
            }).done(result => {
                resolve(result);
            }).fail(reason => {
                reject(reason);
            });
        });
    }

    ObterPorId() {
        return new Promise((resolve, reject) => {
            $.ajax({

            }).done(result => {

            }).fail(reason => {

            });
        });
    }
}

(function () {
    carregarTurmas(null);
})();

async function carregarTurmas(nome) {

    const url = new URLSearchParams();

    if (nome !== null) {
        url.append("nome", nome);
    }

    $("#paginacao_turma").pagination({
        locator: "data",
        pageSize: 5,
        dataSource: "/turma/obter?" + url.toString(),

        totalNumberLocator: function (response) {
            return response.totalCount;
        },

        ajax: {
            beforeSend: function () {
                loading.bloquear();
            }
        },
        callback: function (data, pagination) {
            const response = pagination.originalResponse;

            if (data.length > 0) {

                construirTabelaDeTurmas(data);

                $("#paginacao_turma_totalVisto").text(response.totalItensViewed);

                $("#paginacao_turma_totalResultado").text(response.totalCount);

                $("#tabelaTurmaPanel").css("display", "block");
                $("#tabelaTurmaSemConteudoPanel").css("display", "none");
            } else {
                $("#tabelaTurmaSemConteudoPanel").css("display", "block");
                $("#tabelaTurmaPanel").css("display", "none");
            }

            loading.desbloquear();
        },

        formatAjaxError: function (jqXhr, textStatus, errorThrown) {
            console.error(jqXhr);
            loading.desbloquear();
        }
    });
}

function pesquisarTurmaButtonClick() {

    const valorDigitado = $("#idInputTurmaNome").val();

    carregarTurmas(valorDigitado);
}

function construirTabelaDeTurmas(turmas) {
    const tbody = $("#idTableTurma > tbody");

    tbody.empty();

    turmas.map(t => {
        const tr = `
                <tr>
                    <td>${t.nome}</td>
                    <td>${t.periodo}</td>
                    <td>${t.sigla ?? "-"}</td>
                    <td>${t.turno.nome}</td>
                    <td>
                        <a href="/turma/views/editar/${t.idTurma}">
                            <img src="/icons/edit.svg" />
                        </a>
                        <a href="/turma/views/excluir/${t.idTurma}">
                            <img src="/icons/delete.svg" />
                        </a>
                    </td>
                </tr>
            `;

        tbody.append(tr);
    });
}

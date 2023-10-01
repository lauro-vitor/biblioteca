(function () {
    carregarTurmas();
})();

async function carregarTurmas() {

    const turmaService = new TurmaService();

    loading.bloquear();

    try {
        const turmasResponse = await turmaService.Obter();

        if (Array.isArray(turmasResponse.data) && turmasResponse.data.length > 0) {
            const tabelaTurma = turmaHTML.ObterTabelaTurma(turmasResponse);

            $("#tableTurmaContainer")
                .empty()
                .append(tabelaTurma);
        }

    } catch (err) {
        console.error(err);
    } finally {
        loading.desbloquear();
    }
}


const turmaHTML = {
    ObterTabelaTurma(turmas) {
        const table = $("<table class='table mt-50'></table>");

        const thead = $(`
            <thead>
                <tr>
                    <th>Nome</th>
                    <th>per&iacute;odo</th>
                    <th>Sigla</th>
                    <th>Turno</th>
                    <th></th>
                </tr>
            </thead>`);

        const tbody = $("<tbody></tbody>");

        turmas.data.map(t => {
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

        table.append(thead);
        table.append(tbody);

        return table;
    }
};
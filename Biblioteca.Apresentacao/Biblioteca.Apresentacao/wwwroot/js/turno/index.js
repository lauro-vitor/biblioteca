(function () {
    carregarTurnos();
})();

function carregarTurnos() {
    var turnoService = new TurnoService();

    loading.bloquear();

    turnoService
        .obter()
        .then((turnos) => {

            let componente = null;

            if (Array.isArray(turnos) && turnos.length > 0) {
                componente = turnoHTML.obterTabelaTurno(turnos);
            } else {
                componente = turnoHTML.obterDivTurnoVazio();
            }

            $("#tableContainer")
                .empty()
                .append(componente);

            loading.desbloquear();
        })
        .catch(() => {
            Swal.fire({
                icon: "error",
                title: "Erro",
                text: "Ocorreu algum erro ao recuperar os dados do servidor",
            });

            loading.desbloquear();
        });
}

const turnoHTML = {
    obterTabelaTurno(turnos) {
        const table = $('<table class="table mt-50"> </table>')

        const thead = $(`
            <thead>
	            <tr>
		            <th>Nome</th>
				    <th></th>
			    </tr>
	        </thead>`);

        const tbody = $("<tbody></tbody>");

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

        table.append(thead);
        table.append(tbody);

        return table;
    },

    obterDivTurnoVazio() {
        return $("<div class='alert alert-secondary mt-50'> Nehum turno foi encontrado</div>")
    }
}



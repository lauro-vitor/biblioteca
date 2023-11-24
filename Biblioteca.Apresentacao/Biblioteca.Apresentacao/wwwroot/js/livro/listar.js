
window.onload = async function () {
    await carregarLivros();
}

async function filtrarLivros() {
    await carregarLivros();
    $("#modalFiltrarLivros").modal("hide");
}

async function limparFiltroLivros() {
    document.querySelector("#input-livro-titulo").value = "";
    document.querySelector("#input-livro-editora").value = "";
    document.querySelector("#input-livro-autor").value = "";
    document.querySelector("#input-livro-genero").value = "";
    await carregarLivros();
    $("#modalFiltrarLivros").modal("hide");
}

async function carregarLivros() {
    loading.bloquear();
    try {
        const livros = await obterLivrosHttpService();
        renderizarAppCardLivro(livros.data);
    } catch (err) {
        Swal.fire({
            icon: "error",
            title: "Erro",
            text: err
        });
    } finally {
        loading.desbloquear();
    }
}

function obterLivrosHttpService() {

    return new Promise(async (resolve, reject) => {
        try {

            const params = new URLSearchParams();
            const titulo = document.querySelector("#input-livro-titulo").value;
            const editora = document.querySelector("#input-livro-editora").value;
            const autor = document.querySelector("#input-livro-autor").value;
            const genero = document.querySelector("#input-livro-genero").value;

            if (titulo)
                params.append("titulo", titulo);

            if (editora)
                params.append("editora", editora);

            if (autor)
                params.append("autor", autor);

            if (genero)
                params.append("genero", genero);

            let queryString = params.toString() ?? "";

            const response = await fetch("/api/livro?" + queryString);

            const dados = await response.json();

            resolve(dados);

        } catch (err) {
            reject(err);
        }
    });
}


function renderizarAppCardLivro(livros) {

    document.querySelector("app-card-livro").innerHTML = "";

    if (!livros || livros.length <= 0) {

        document.querySelector("app-card-livro").innerHTML = `
            <div class="card">
                <div class="card-header">
                    Livros
                </div>
                <div class="card-body">
                    <h5 class="card-title"> Nehum resultado de livros foi encontrado </h5>
                </div>
            </div>
        `;

        return;
    }

    const row = document.createElement("div");
    row.classList.add("row");

    livros.map(livro => {

        const cardContainer = document.createElement("div");
        cardContainer.classList.add("col-xs-12");
        cardContainer.classList.add("col-sm-4");
        cardContainer.classList.add("card-item");

        let autoresHtml;
        let generosHtml;

        livro.autores.map(autorItem => {

            if (!autoresHtml)
                autoresHtml = "";

            autoresHtml += `<span class="badge bg-secondary" >${autorItem.nome}</span> `;
        });

        livro.generos.map(generoItem => {

            if (!generosHtml)
                generosHtml = "";

            generosHtml += `<span class="badge bg-info text-dark">${generoItem.nome}</span> `;
        });


        cardContainer.innerHTML = `
<div class="card card-livro">
    <div class="card-body">
        
        <div class="livro-row">
            <h5 class="text-center">
                <b> ${livro.titulo} </b>
            </h5>
        </div>

        <div class="livro-row">
            <b>Editora:</b> ${livro.editora.nome}
        </div>

        <div class="livro-row">
            <b>Publicação:</b> ${livro.dataPublicacao}
        </div>

        <div class="livro-row">
            <b>Quantidade:</b> ${livro.quantidadeEstoque}
        </div>

        <div class="livro-row">
            <span class="livro-row__edicao">
                <b>Edição:</b> ${(livro.edicao ?? "")}
            </span>
            <span>
                <b>Volume:</b> ${(livro.volume ?? "")}
            </span>
        </div>

        <div class="livro-row">
            <b>Autores:</b>
           ${(autoresHtml ?? "")}
        </div>

        <div class="livro-row">
            <b>Gêneros:</b>
            ${(generosHtml ?? "")}
        </div>

    </div>
</div>`;

        cardContainer.addEventListener("click", () => {
            window.location.href = "/views/livro/" + livro.idLivro;
        });

        row.appendChild(cardContainer);
    });

    document.querySelector("app-card-livro").appendChild(row);
}
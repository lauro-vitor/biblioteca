window.onload = async function () {
    await carregarLivros();
};

async function carregarLivros() {

    const cardLivroComponente = await fetch("/componentes/livro/card-livro.html").then(data => data.text());

    const response = await fetch("/api/livro").then(result => result.json());

    const livros = response.data;

    let html = "<div class='row'> ";

    livros.map(livro => {

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

        let card = cardLivroComponente
            .replace("{{titulo}}", livro.titulo)
            .replace("{{editora}}", livro.editora.nome)
            .replace("{{dataPublicacao}}", livro.dataPublicacao)
            .replace("{{quantidade}}", livro.quantidadeEstoque)
            .replace("{{edicao}}", (livro.edicao ?? "-"))
            .replace("{{volume}}", (livro.volume ?? "-"))
            .replace("{{autores}}", (autoresHtml ?? "-"))
            .replace("{{generos}}", (generosHtml ?? "-"));

        html += card;
    });

    html += "</div>";

    document.querySelector("[componente=card-livro]").innerHTML = html;
}

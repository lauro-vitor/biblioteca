BEGIN TRANSACTION;

    DROP TABLE IF EXISTS Livro;
    
COMMIT TRANSACTION;

BEGIN TRANSACTION;

    CREATE TABLE Livro(
        IdLivro VARCHAR(32) NOT NULL,
        IdEditora VARCHAR(32) NOT NULL,
        Titulo VARCHAR(255) NOT NULL,
        DataPublicacao TEXT NOT NULL,
        QuantidadeEstoque INTEGER NOT NULL,
        Edicao INTEGER NULL,
        Volume INTEGER NULL,
        PRIMARY KEY(IdLivro),
        FOREIGN KEY(IdEditora) REFERENCES Editora(IdEditora)
    );

COMMIT TRANSACTION;
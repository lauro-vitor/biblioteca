BEGIN TRANSACTION;

    DROP TABLE IF EXISTS LivroGenero;
    
COMMIT TRANSACTION;

BEGIN TRANSACTION;

    CREATE TABLE LivroGenero(
        IdLivroGenero VARCHAR(32) NOT NULL,
        IdLivro VARCHAR(32) NOT NULL,
        IdGenero VARCHAR(32) NOT NULL,
        PRIMARY KEY(IdLivroGenero),
        FOREIGN KEY(IdLivro) REFERENCES Livro(IdLivro),
        FOREIGN KEY(IdGenero) REFERENCES Genero(IdGenero)
    );

COMMIT TRANSACTION;



BEGIN TRANSACTION;

    DROP TABLE IF EXISTS Autor;
    
COMMIT TRANSACTION;

BEGIN TRANSACTION;

    CREATE TABLE Autor(
        IdAutor CHAR(32) NOT NULL,
        Nome VARCHAR(255) NOT NULL,
        PRIMARY KEY(IdAutor)
    );

COMMIT TRANSACTION;
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
          reject(reason);
        });
    });
  }

  inserir(turno) {
    return new Promise((resolve, reject) => {
      $.ajax({
        headers: {
          Accept: "application/json",
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
    
  }

  ObterIdPathParameter() {
    const id = window.location.pathname.split("/")[4];

    if (id == undefined || id == "")
      throw "Não foi possível encontrar id do turno";

    return id;
  }
}

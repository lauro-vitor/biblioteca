class TurnoService {
  obterTurnos() {
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

  inserirTurno(turno) {
    return new Promise((resolve, reject) => {
      $.ajax({
        headers:{
          'Accept': 'application/json',
          'Content-Type': 'application/json' 
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
}

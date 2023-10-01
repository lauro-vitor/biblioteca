
class TurmaService {
    Obter() {
        return new Promise(
            function (resolve, reject) {
                $.ajax({
                    type: "GET",
                    url: "/turma/obter",
                    accepts: "application/json",
                    cache: false
                })
                    .done(result => {
                        resolve(result);
                    })
                    .fail(reason => {
                        reject(reason);
                    });
            }
        );
    }


}
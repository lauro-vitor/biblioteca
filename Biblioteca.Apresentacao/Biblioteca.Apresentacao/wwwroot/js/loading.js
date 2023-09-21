const loading = {
    bloquear: function () {
        const spinner = $(
             `<div id="idLoadingSpinner" class="d-flex justify-content-center loading-container">
               <div class="spinner-border" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
             </div>`
        );
        $("#idContainerLayout").append(spinner);
    },

    desbloquear: function () {
        $("#idLoadingSpinner").remove();
    },
};

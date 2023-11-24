
carregarScripts();


async function carregarScripts() {

    try {
        const urls = [
            '/lib/jquery/dist/jquery.min.js',
            '/lib/bootstrap/dist/js/bootstrap.bundle.min.js',
            '/lib/sweet-alert.js',

            '/js/components/appNavBar.js',
            '/js/loading.js'
        ];

        for (var url of urls) {
            const script = document.createElement("script");
            script.src = url;
            script.setAttribute("defer", "");

            document.head.appendChild(script);
        }

    } catch (err) {
        console.error(err);
    }
}
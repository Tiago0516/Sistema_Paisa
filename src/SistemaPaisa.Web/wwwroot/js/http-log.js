/**
 * Registra body de petición y cuerpo de respuesta en la consola del navegador (DevTools).
 */
(function () {
    if (window.__fetchHttpLogInstalled) return;
    window.__fetchHttpLogInstalled = true;

    const nativeFetch = window.fetch.bind(window);

    function formatBody(body) {
        if (body == null) return null;
        if (body instanceof FormData) return Object.fromEntries(body.entries());
        if (typeof body === 'string') {
            try {
                return JSON.parse(body);
            } catch {
                return body;
            }
        }
        return body;
    }

    async function readResponseBody(response) {
        const contentType = response.headers.get('content-type') || '';
        try {
            if (contentType.includes('application/json')) {
                return await response.json();
            }
            return await response.text();
        } catch {
            return '(no se pudo leer el cuerpo)';
        }
    }

    window.fetch = async function (input, init) {
        const url = typeof input === 'string' ? input : input.url;
        const method = (init && init.method) || 'GET';
        const requestBody = formatBody(init && init.body);

        console.log('[HTTP Request]', { url, method, body: requestBody });

        const response = await nativeFetch(input, init);
        const responseBody = await readResponseBody(response.clone());

        console.log('[HTTP Response]', {
            url,
            status: response.status,
            statusText: response.statusText,
            body: responseBody
        });

        return response;
    };
})();

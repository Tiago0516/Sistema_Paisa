/**
 * Cliente HTTP JSON para APIs REST del mismo origen (cookies de sesión).
 */
(function () {
    function escapeHtml(text) {
        return String(text)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function applyValidationErrors(form, errors) {
        if (!form || !errors) return;
        form.querySelectorAll('[data-api-error]').forEach(el => { el.textContent = ''; });
        for (const [key, messages] of Object.entries(errors)) {
            const field = form.querySelector(`[name="${key}"], [name="${key.charAt(0).toUpperCase()}${key.slice(1)}"]`);
            const span = field?.closest('.mb-3')?.querySelector('[data-api-error]')
                || form.querySelector(`[data-api-error-for="${key}"]`);
            if (span && messages?.length) {
                span.textContent = messages.join(' ');
            }
        }
    }

    async function readJsonSafe(response) {
        const contentType = response.headers.get('content-type') || '';
        if (!contentType.includes('application/json')) {
            return null;
        }
        try {
            return await response.json();
        } catch {
            return null;
        }
    }

    async function apiRequest(url, options = {}) {
        const headers = {
            Accept: 'application/json',
            ...(options.headers || {})
        };

        let body = options.body;
        if (body != null && typeof body === 'object' && !(body instanceof FormData)) {
            headers['Content-Type'] = 'application/json';
            body = JSON.stringify(body);
        }

        const response = await fetch(url, {
            ...options,
            headers,
            body,
            credentials: 'same-origin'
        });

        const data = await readJsonSafe(response);
        return { response, data };
    }

    window.SistemaPaisaApi = {
        escapeHtml,
        applyValidationErrors,
        apiRequest
    };
})();

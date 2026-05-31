
(function () {
    const api = () => window.SistemaPaisaApi;

    function formatUrl(template, id) {
        return template.replace(/\{id\}/gi, String(id));
    }

    function normalizeSupplier(item) {
        if (!item || typeof item !== 'object') {
            return { id: 0, name: '', email: '', isActive: false };
        }
        const isActiveRaw = item.isActive ?? item.IsActive;
        return {
            id: item.id ?? item.Id ?? 0,
            name: String(item.name ?? item.Name ?? '').trim(),
            email: String(item.email ?? item.Email ?? '').trim(),
            isActive: isActiveRaw === true || isActiveRaw === 'true'
        };
    }

    function parseListResponse(data) {
        if (Array.isArray(data)) return data;
        if (data && Array.isArray(data.items)) return data.items;
        if (data && Array.isArray(data.data)) return data.data;
        return [];
    }

    function getColumnKeys(table) {
        const keysAttr = (table.dataset.columnKeys || '').trim();
        if (keysAttr) {
            return keysAttr.split(',').map(k => k.trim()).filter(Boolean);
        }
        return ['name', 'email', 'active'];
    }

    function formatCellValue(row, key) {
        switch (key) {
            case 'name':
                return api().escapeHtml(row.name);
            case 'email':
                return api().escapeHtml(row.email);
            case 'active':
                return row.isActive ? 'Sí' : 'No';
            default: {
                const val = row[key];
                if (val == null || val === '') return '—';
                return api().escapeHtml(String(val));
            }
        }
    }

    function buildActionsMenu(table, row) {
        const allowView = table.dataset.allowView === 'true';
        const allowEdit = table.dataset.allowEdit === 'true';
        const allowDelete = table.dataset.allowDelete === 'true';
        const detailsTpl = table.dataset.urlDetails;
        const editTpl = table.dataset.urlEdit;
        const deleteTpl = table.dataset.urlDelete;

        const items = [];
        if (allowView && detailsTpl) {
            items.push(`<li><a class="dropdown-item" href="${formatUrl(detailsTpl, row.id)}">Ver</a></li>`);
        }
        if (allowEdit && editTpl) {
            items.push(`<li><a class="dropdown-item" href="${formatUrl(editTpl, row.id)}">Editar</a></li>`);
        }
        if (allowDelete && deleteTpl) {
            items.push(`<li><a class="dropdown-item text-danger" href="${formatUrl(deleteTpl, row.id)}">Eliminar</a></li>`);
        }

        if (items.length === 0) return '';

        return `
            <div class="dropdown">
                <button class="btn btn-module-actions"
                        type="button"
                        data-bs-toggle="dropdown"
                        data-bs-display="static"
                        data-bs-popper-config='{"strategy":"fixed"}'
                        aria-expanded="false"
                        aria-label="Acciones">
                    <i class="bi bi-three-dots-vertical"></i>
                </button>
                <ul class="dropdown-menu dropdown-menu-end module-row-menu">${items.join('')}</ul>
            </div>`;
    }

    function buildRow(table, item, columnKeys, showActions) {
        const row = normalizeSupplier(item);
        const cells = columnKeys.map(key => `<td>${formatCellValue(row, key)}</td>`).join('');
        const actionCell = showActions
            ? `<td class="module-actions-cell text-end">${buildActionsMenu(table, row)}</td>`
            : '';
        return `<tr data-row-id="${row.id}">${cells}${actionCell}</tr>`;
    }

    function columnCount(table) {
        return table.querySelectorAll('thead th').length;
    }

    async function loadApiModuleGrid() {
        const table = document.getElementById('moduleDataTable');
        const apiBase = (table?.dataset.apiBase || '').trim();
        if (!table || !apiBase) return;

        if (!window.SistemaPaisaApi?.apiRequest) {
            console.error('SistemaPaisaApi no está disponible.');
            return;
        }

        const tbody = table.querySelector('tbody');
        const cols = columnCount(table);
        const showActions = table.dataset.allowView === 'true'
            || table.dataset.allowEdit === 'true'
            || table.dataset.allowDelete === 'true';
        const columnKeys = getColumnKeys(table);

        tbody.innerHTML = `<tr id="moduleGridLoadingRow"><td colspan="${cols}" class="text-center text-muted py-4">Cargando...</td></tr>`;

        try {
            const { response, data } = await api().apiRequest(apiBase);
            if (!response.ok) {
                throw new Error(data?.error || data?.title || `Error ${response.status}`);
            }

            const items = parseListResponse(data).map(normalizeSupplier);

            if (items.length === 0) {
                tbody.innerHTML = `<tr><td colspan="${cols}" class="text-center text-muted py-4">Sin registros</td></tr>`;
            } else {
                tbody.innerHTML = items
                    .map(item => buildRow(table, item, columnKeys, showActions))
                    .join('');
            }

            tbody.querySelectorAll('.module-row-menu').forEach(menu => {
                menu.addEventListener('click', (e) => e.stopPropagation());
            });

            if (typeof window.refreshModuleGrid === 'function') {
                window.refreshModuleGrid();
            }
        } catch (err) {
            tbody.innerHTML = `<tr><td colspan="${cols}" class="text-center text-danger py-4">${api().escapeHtml(err.message || 'Error al cargar.')}</td></tr>`;
            if (typeof window.refreshModuleGrid === 'function') {
                window.refreshModuleGrid();
            }
        }
    }

    window.reloadApiModuleGrid = loadApiModuleGrid;
})();

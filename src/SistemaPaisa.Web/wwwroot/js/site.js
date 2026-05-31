(function () {
    function updateDateTime() {
        const now = new Date();
        const dateEl = document.getElementById('topbarDate');
        const timeEl = document.getElementById('topbarTime');
        if (dateEl) {
            dateEl.textContent = now.toLocaleDateString();
        }
        if (timeEl) {
            timeEl.textContent = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        }
    }

    updateDateTime();
    setInterval(updateDateTime, 30000);

    initSidebarHover();
    initModuleGrid();
    initModuleCreateModal();
})();

function initSidebarHover() {
    const sidebar = document.getElementById('appSidebar');
    const trigger = document.getElementById('sidebarMenuTrigger');
    if (!sidebar || !trigger) return;

    const setExpanded = (expanded) => {
        trigger.setAttribute('aria-expanded', expanded ? 'true' : 'false');
    };

    sidebar.addEventListener('mouseenter', () => setExpanded(true));
    sidebar.addEventListener('mouseleave', () => setExpanded(false));
    sidebar.addEventListener('focusin', () => setExpanded(true));
    sidebar.addEventListener('focusout', (e) => {
        if (!sidebar.contains(e.relatedTarget)) {
            setExpanded(false);
        }
    });
}

function initModuleCreateModal() {
    const createBtn = document.getElementById('btnModuleCreate');
    const modalEl = document.getElementById('moduleCreateModal');
    const modalBody = document.getElementById('moduleCreateModalBody');
    const modalTitle = document.getElementById('moduleCreateModalLabel');

    if (!createBtn || !modalEl || !modalBody) return;

    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    createBtn.addEventListener('click', async () => {
        const baseUrl = createBtn.dataset.createUrl;
        if (!baseUrl) return;

        const supportsModal = /\/(Products|Categories|Suppliers|Modules|AppActions|Roles|Profiles)\/Create|\/Account\/Register/i.test(baseUrl);
        if (!supportsModal) {
            window.location.href = baseUrl;
            return;
        }

        const title = createBtn.dataset.createTitle || 'Crear';
        if (modalTitle) modalTitle.textContent = title;

        modalBody.innerHTML = '<div class="text-center py-4 text-muted">Cargando...</div>';
        modal.show();

        const url = baseUrl.includes('?') ? `${baseUrl}&modal=1` : `${baseUrl}?modal=1`;

        try {
            const response = await fetch(url, {
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });
            if (!response.ok) throw new Error('No se pudo cargar el formulario.');

            const html = await response.text();
            modalBody.innerHTML = html;
            bindModalForm(modalBody, modal);
            if (typeof initProfileForms === 'function') {
                initProfileForms(modalBody);
            }
        } catch {
            modalBody.innerHTML = '<p class="text-danger mb-0">No se pudo cargar el formulario.</p>';
        }
    });
}

function bindModalForm(container, modal) {
    const form = container.querySelector('form[data-modal-form]');
    if (!form) return;

    if (window.jQuery && window.jQuery.validator) {
        window.jQuery.validator.unobtrusive.parse(form);
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const submitBtn = form.querySelector('[type="submit"]');
        if (submitBtn) submitBtn.disabled = true;

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: {
                    'X-Modal-Form': 'true',
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            const contentType = response.headers.get('content-type') || '';

            if (contentType.includes('application/json')) {
                const data = await response.json();
                if (data.success) {
                    modal.hide();
                    window.location.reload();
                    return;
                }
            }

            const html = await response.text();
            container.innerHTML = html;
            bindModalForm(container, modal);
        } catch {
            container.innerHTML = '<p class="text-danger mb-0">Error al guardar. Intente de nuevo.</p>';
        } finally {
            if (submitBtn) submitBtn.disabled = false;
        }
    });
}

function initModuleGrid() {
    const table = document.getElementById('moduleDataTable');
    if (!table) return;

    const tbody = table.querySelector('tbody');
    const searchInput = document.getElementById('moduleQuickSearch');
    const pageSizeSelect = document.getElementById('modulePageSize');
    const summaryEl = document.getElementById('moduleRecordSummary');
    const pageLabelEl = document.getElementById('modulePageLabel');
    const btnFirst = document.getElementById('modulePageFirst');
    const btnPrev = document.getElementById('modulePagePrev');
    const btnNext = document.getElementById('modulePageNext');
    const btnLast = document.getElementById('modulePageLast');

    const allRows = Array.from(tbody.querySelectorAll('tr'));
    let filteredRows = [...allRows];
    let currentPage = 1;

    function getPageSize() {
        return parseInt(pageSizeSelect?.value || '20', 10);
    }

    function render() {
        const pageSize = getPageSize();
        const total = filteredRows.length;
        const totalPages = Math.max(1, Math.ceil(total / pageSize));
        currentPage = Math.min(currentPage, totalPages);
        if (currentPage < 1) currentPage = 1;

        const start = (currentPage - 1) * pageSize;
        const end = start + pageSize;

        allRows.forEach(row => { row.style.display = 'none'; });
        filteredRows.forEach((row, index) => {
            row.style.display = index >= start && index < end ? '' : 'none';
        });

        const from = total === 0 ? 0 : start + 1;
        const to = Math.min(end, total);
        if (summaryEl) {
            summaryEl.textContent = total === 0
                ? '0 registros'
                : `${from} a ${to} de ${total}`;
        }
        if (pageLabelEl) {
            pageLabelEl.textContent = `Página ${currentPage} de ${totalPages}`;
        }

        const disableNav = total === 0 || totalPages <= 1;
        if (btnFirst) btnFirst.disabled = disableNav || currentPage === 1;
        if (btnPrev) btnPrev.disabled = disableNav || currentPage === 1;
        if (btnNext) btnNext.disabled = disableNav || currentPage === totalPages;
        if (btnLast) btnLast.disabled = disableNav || currentPage === totalPages;
    }

    function applySearch() {
        const term = (searchInput?.value || '').trim().toLowerCase();
        filteredRows = allRows.filter(row =>
            row.textContent.toLowerCase().includes(term));
        currentPage = 1;
        render();
    }

    searchInput?.addEventListener('input', applySearch);
    pageSizeSelect?.addEventListener('change', () => {
        currentPage = 1;
        render();
    });

    btnFirst?.addEventListener('click', () => { currentPage = 1; render(); });
    btnPrev?.addEventListener('click', () => { currentPage -= 1; render(); });
    btnNext?.addEventListener('click', () => { currentPage += 1; render(); });
    btnLast?.addEventListener('click', () => {
        const pageSize = getPageSize();
        currentPage = Math.max(1, Math.ceil(filteredRows.length / pageSize));
        render();
    });

    document.querySelectorAll('.module-row-menu').forEach(menu => {
        menu.addEventListener('click', (e) => e.stopPropagation());
    });

    render();
}

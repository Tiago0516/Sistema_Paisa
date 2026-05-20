function initProfileForms(root) {
    const scope = root || document;
    scope.querySelectorAll('.profile-form').forEach(form => {
        const moduleCheckboxes = form.querySelectorAll('.profile-module-checkbox');
        const actionsList = form.querySelector('.profile-actions-list');
        const actionsUrl = form.dataset.actionsUrl;
        if (!actionsList || !actionsUrl) return;

        const getSelectedModuleIds = () =>
            [...moduleCheckboxes]
                .filter(cb => cb.checked)
                .map(cb => cb.value);

        const loadActions = async () => {
            const moduleIds = getSelectedModuleIds();
            if (!moduleIds.length) {
                actionsList.innerHTML = '<p class="text-muted small mb-0">Seleccione al menos un módulo.</p>';
                return;
            }

            const selected = (actionsList.dataset.selectedActions || '')
                .split(',')
                .filter(Boolean)
                .map(id => parseInt(id, 10));

            const sections = [];
            for (const moduleId of moduleIds) {
                const response = await fetch(`${actionsUrl}?moduleId=${moduleId}`);
                const actions = await response.json();
                const moduleLabel = form.querySelector(`label[for="module_${moduleId}"]`)?.textContent?.trim()
                    || `Módulo ${moduleId}`;

                if (!actions.length) {
                    sections.push(`<p class="text-muted small mb-2"><strong>${moduleLabel}</strong>: sin acciones.</p>`);
                    continue;
                }

                const items = actions.map(a => `
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" name="ActionIds" value="${a.id}" id="action_${a.id}"
                            ${selected.includes(a.id) ? 'checked' : ''} />
                        <label class="form-check-label" for="action_${a.id}">
                            ${a.name} <span class="text-muted">(${a.code})</span>
                        </label>
                    </div>`).join('');

                sections.push(`
                    <div class="mb-3">
                        <div class="fw-semibold small mb-2">${moduleLabel}</div>
                        ${items}
                    </div>`);
            }

            actionsList.innerHTML = sections.join('');
        };

        moduleCheckboxes.forEach(cb => {
            cb.addEventListener('change', () => {
                actionsList.dataset.selectedActions = [...actionsList.querySelectorAll('input[name="ActionIds"]:checked')]
                    .map(input => input.value)
                    .join(',');
                loadActions();
            });
        });

        loadActions();
    });
}

document.addEventListener('DOMContentLoaded', () => initProfileForms());

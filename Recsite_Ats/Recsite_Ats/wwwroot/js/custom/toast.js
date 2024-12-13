function showToast(message, type = 'success', title = '') {
    const toastId = `toast-${Date.now()}`;
    const toastTypeClass = type === 'success' ? 'bg-success' : (type === 'error' ? 'bg-danger' : 'bg-info');

    const toastHTML = `
        <div id="${toastId}" class="toast ${toastTypeClass} text-white" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="3000">
            <div class="toast-header">
                <strong class="me-auto">${title || (type.charAt(0).toUpperCase() + type.slice(1))}</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;

    // Append toast to container
    $('#toast-container').append(toastHTML);

    // Initialize and show the toast
    const toastElement = new bootstrap.Toast(document.getElementById(toastId));
    toastElement.show();

    // Remove the toast from the DOM after it hides
    $(`#${toastId}`).on('hidden.bs.toast', function () {
        $(this).remove();
    });
}

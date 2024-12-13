// Initialize Select2 inside the modal when the modal is opened
$('#createCompanyModal').on('shown.bs.modal', function () {
    $('#multiSelectInModal').select2({
        dropdownParent: $('#createCompanyModal'),  // Attach dropdown to modal to avoid overlap
        closeOnSelect: false,  // Keep the dropdown open when selecting options
        templateResult: formatResult,  // Custom template for results
        templateSelection: formatSelection,  // Custom template for selected items
        placeholder: "Please Select"
    });

    // Handle events to update checkboxes
    $('#multiSelectInModal').on('select2:open', updateCheckboxes);
    $('#multiSelectInModal').on('select2:select select2:unselect', updateCheckboxes);
});

// Custom rendering for dropdown options with checkboxes
function formatResult(item) {
    if (!item.id) {
        return item.text; // Return original item if it's not a valid option
    }

    // Create a custom template for the dropdown items with checkboxes
    return $('<span> ' + item.text + '</span>');
}

// Custom rendering for selected items (no change in display)
function formatSelection(item) {
    return item.text; // Keep original item text in the selection
}


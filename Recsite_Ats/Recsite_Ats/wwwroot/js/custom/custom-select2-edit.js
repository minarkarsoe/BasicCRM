$(document).ready(function () {
    // Initialize Select2 inside the modal when it is opened
    $('#createCompanyModal').on('shown.bs.modal', function () {
        // Apply Select2 to all select boxes inside the modal
        $('.select2-checkbox').each(function () {
            $(this).select2({
                dropdownParent: $('#createCompanyModal'),
                closeOnSelect: false,
                templateResult: formatResult,  // Custom template for checkboxes in dropdown
                templateSelection: formatSelection,  // Custom selected items
                placeholder: "Please Select"
            });
        });

        // Sync checkboxes when opening the dropdown
        $('.select2-checkbox').on('select2:open', function () {
            updateCheckboxes($(this));
        });

        // Handle select/unselect event to toggle checkbox state
        $('.select2-checkbox').on('select2:select select2:unselect', function () {
            updateCheckboxes($(this));
        });
    });

    // Custom template for dropdown options with checkboxes
    function formatResult(item) {
        if (!item.id) {
            return item.text; // Return item text if no id
        }

        // Check if option is selected
        var isSelected = $(item.element).prop('selected') ? 'checked' : '';

        // Return a custom template with checkbox
        return $('<span><input type="checkbox" ' + isSelected + '> ' + item.text + '</span>');
    }

    // Custom template for selected items
    function formatSelection(item) {
        return item.text;  // Display the selected item text
    }

    // Function to update checkboxes based on selection
    function updateCheckboxes($select) {
        var selectedValues = $select.val();
        $select.find('option').each(function () {
            var $option = $(this);
            var $checkbox = $option.parent().find('input[type="checkbox"]');

            // Toggle the checkbox based on selected state
            if (selectedValues.includes($option.val())) {
                $checkbox.prop('checked', true);
            } else {
                $checkbox.prop('checked', false);
            }
        });
    }
});

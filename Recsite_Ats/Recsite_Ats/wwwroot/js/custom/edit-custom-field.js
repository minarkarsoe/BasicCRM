document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var parentRow = this.closest('tr'); // Get the closest row
            var fieldNameTd = parentRow.querySelector('.field-name'); // Find the FieldName td
            var viewValueTd = parentRow.querySelector('.view-value'); // Find the ViewValue td

            // Store the original values for later use
            parentRow.dataset.originalFieldName = fieldNameTd.textContent.trim();
            parentRow.dataset.originalViewValue = viewValueTd.textContent.trim();

            // Replace td content with input fields
            fieldNameTd.innerHTML = `<input type="text" class="form-control" value="${parentRow.dataset.originalFieldName}" />`;
            viewValueTd.innerHTML = `<input type="text" class="form-control" value="${parentRow.dataset.originalViewValue}" />`;

            // Hide edit and delete buttons, show Ok and Cancel buttons
            Array.from(parentRow.querySelectorAll('.edit-btn, .delete-custom-field')).forEach(btn => btn.style.display = 'none');
            Array.from(parentRow.querySelectorAll('.ok-btn, .cancel-btn')).forEach(btn => btn.style.display = 'inline');
        });
    });

    document.querySelectorAll('.ok-btn').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const url = this.href;
            var parentRow = this.closest('tr');
            var fieldNameInput = parentRow.querySelector('.field-name input'); // Get the FieldName input
            var viewValueInput = parentRow.querySelector('.view-value input'); // Get the ViewValue input

            // Get the new values
            var newFieldName = fieldNameInput.value.trim();
            var newViewValue = viewValueInput.value.trim();

            // Fetch API call to submit the changes
            fetch(url, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    customFieldId: parentRow.dataset.customFieldId, // assuming there's a data attribute with the field ID
                    fieldName: newFieldName,
                    viewValue: newViewValue
                })
            })
                .then(response => response.json())
                .then(data => {
                    // Replace input fields with the updated text
                    parentRow.querySelector('.field-name').textContent = data.data.updatedFieldName;
                    parentRow.querySelector('.view-value').textContent = data.data.updatedViewValue;

                    // Hide Ok and Cancel buttons, show Edit and Delete buttons
                    Array.from(parentRow.querySelectorAll('.ok-btn, .cancel-btn')).forEach(btn => btn.style.display = 'none');
                    Array.from(parentRow.querySelectorAll('.edit-btn, .delete-custom-field')).forEach(btn => btn.style.display = 'inline');

                    // Optionally show a success message
                    showToast('Field updated successfully!', 'success');
                    location.reload();
                })
                .catch(error => {
                    console.error('Error:', error);
                    // Optionally show an error message
                    showToast('Failed to update the field.', 'error');
                });
        });
    });

    document.querySelectorAll('.cancel-btn').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var parentRow = this.closest('tr');

            // Revert the changes
            var originalFieldName = parentRow.dataset.originalFieldName;
            var originalViewValue = parentRow.dataset.originalViewValue;

            parentRow.querySelector('.field-name').textContent = originalFieldName;
            parentRow.querySelector('.view-value').textContent = originalViewValue;

            // Hide Ok and Cancel buttons, show Edit and Delete buttons
            Array.from(parentRow.querySelectorAll('.ok-btn, .cancel-btn')).forEach(btn => btn.style.display = 'none');
            Array.from(parentRow.querySelectorAll('.edit-btn, .delete-custom-field')).forEach(btn => btn.style.display = 'inline');
        });
    });
});

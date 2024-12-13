document.addEventListener('DOMContentLoaded', function () {
    const saveButton = document.getElementById('saveOrderButton');

    if (!saveButton) {
        return;
    }

    saveButton.addEventListener('click', function () {
        const data = [];
        document.querySelectorAll('.sortable-table tbody tr').forEach(function (row) {
            var IsRequiredInuput = row.querySelector(`input[name="order[${row.dataset.index}].IsRequired"]:checked`);
            const item = {
                TableName: row.dataset.tableName,
                AccountId: row.dataset.accountId,
                Id: row.dataset.id,
                FieldName: row.dataset.fieldName,
                CustomFieldTypeName: row.dataset.customFieldTypeName,
                CustomFieldTypeId: row.dataset.customFieldTypeId,
                SectionLayoutId: row.dataset.sectionLayoutId,
                IsNullable: row.dataset.isNullable === 'true', // Ensure boolean value
                IsLocked: row.dataset.isLocked === 'true', // Ensure boolean value
                IsRequired: IsRequiredInuput ? IsRequiredInuput.value === 'true' : false, // Ensure boolean value
                IsVisible: row.dataset.isVisible === 'true', // Ensure boolean value
                SortOrder: row.dataset.sortOrder,
                IsCustomField: row.dataset.isCustomField === 'true' // Ensure boolean value
            };
            data.push(item);
        });
        fetch('/FormSetting/UpdateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(result => {
                location.reload();
                alert("Update Successfull.");
                // Handle success (e.g., show a message, disable button, etc.)
            })
            .catch(error => {
                console.error('Error:', error);
                alert(error);
                // Handle error (e.g., show an error message)
            });
    });
});

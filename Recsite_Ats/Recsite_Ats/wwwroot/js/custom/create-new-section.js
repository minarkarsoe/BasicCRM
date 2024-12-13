document.addEventListener('DOMContentLoaded', function () {
    // Show the New Section form when the "New Section" button is clicked
    document.getElementById('showNewSectionForm').addEventListener('click', function (e) {
        e.preventDefault();
        document.querySelector('.newSectionFormContainer').style.display = 'block';
    });

    // Hide the New Section form when the "Cancel" button is clicked
    document.getElementById('cancelNewSection').addEventListener('click', function (e) {
        e.preventDefault();
        document.querySelector('.newSectionFormContainer').style.display = 'none';
    });

    // Handle the form submission with Fetch API
    document.getElementById('saveNewSection').addEventListener('click', function () {
        const form = document.getElementById('newSectionForm');
        const url = form.dataset.url;
        const data = {
            SectionName: document.querySelector('#sectionName').value,
            TableName: document.querySelector('#tableName').value,
            // Any other fields from your view model
        };

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(result => {
                showToast('success', data.message);
                location.reload();
            })
            .catch(error => {
                showToast('error', 'Failed to create section');
            });
    });

    document.querySelectorAll('.delete-section').forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            const url = this.href; // Use the href attribute from the link
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            if (confirm('Are you sure you want to delete this section?')) {
                fetch(url, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': token
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            showToast('success', data.message);
                            // Optionally, remove the deleted section from the DOM
                            this.closest('.sectionRow').remove();
                            location.reload();
                        } else {
                            showToast('error', 'Failed to delete section');
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        showToast('error', 'Failed to delete section');
                    });
            }
        });
    });

    document.querySelectorAll('.edit-section').forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            const url = this.href; // Use the href attribute from the link
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            fetch(url, {
                method: 'PUT',
                headers: {
                    'RequestVerificationToken': token
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        location.reload();
                        showToast(data.message ,'success' , 'Success');
                    } else {
                        showToast('Failed to edit section' , 'error', 'Error');
                    }
                })
                .catch(error => {
                    showToast('Failed to edit section', 'error', 'Error');
                });

        });
    });

    document.querySelectorAll('.delete-custom-field').forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            const url = this.href; // Use the href attribute from the link
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            fetch(url, {
                method: 'DELETE',
                headers: {
                    'RequestVerificationToken': token
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        location.reload();
                        showToast(data.message, 'success', 'Success');
                    } else {
                        showToast('Failed to edit section', 'error', 'Error');
                    }
                })
                .catch(error => {
                    showToast('Failed to edit section', 'error', 'Error');
                });

        });
    });

});

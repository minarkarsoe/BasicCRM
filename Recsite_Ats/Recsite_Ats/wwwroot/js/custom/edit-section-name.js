document.addEventListener('DOMContentLoaded', function () {
    const editButton = document.querySelector('.edit-btn');
    const okButton = document.querySelector('.ok-btn');
    const cancelButton = document.querySelector('.cancel-btn');
    const deleteButton = document.querySelector('.delete-btn');
    const sectionNameDisplay = document.querySelector('.section-name-display');
    const sectionNameEdit = document.querySelector('.section-name-edit');

    editButton.addEventListener('click', function () {
        sectionNameDisplay.style.display = 'none';
        sectionNameEdit.style.display = 'inline-block';
        editButton.style.display = 'none';
        deleteButton.style.display = 'none';
        okButton.style.display = 'inline-block';
        cancelButton.style.display = 'inline-block';
    });

    cancelButton.addEventListener('click', function () {
        sectionNameEdit.value = sectionNameDisplay.textContent;
        sectionNameDisplay.style.display = 'inline-block';
        sectionNameEdit.style.display = 'none';
        editButton.style.display = 'inline-block';
        deleteButton.style.display = 'inline-block';
        okButton.style.display = 'none';
        cancelButton.style.display = 'none';
    });

    sectionNameEdit.addEventListener('input', function () {
        const sectionName = sectionNameEdit.value;
        const baseUrl = okButton.getAttribute('href').split('&sectionName=')[0];
        const updatedUrl = `${baseUrl}&sectionName=${encodeURIComponent(sectionName)}`;
        okButton.setAttribute('href', updatedUrl);
    });
});

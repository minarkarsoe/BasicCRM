﻿"use strict";

$(function () {
    // Enable sortable for all tables with class 'sortable-table'
    $(".sortable-table tbody").sortable({
        connectWith: ".sortable-table tbody",
        placeholder: "sortable-placeholder",
        stop: function stop(event, ui) {
            // Remove placeholder if present
            $('.sortable-table tbody .placeholder').remove();

            // Update data-index and SortOrder attributes after sorting
            $('.sortable-table').each(function () {
                var sectionId = $(this).data('section-id');
                $(this).find('tbody tr.no-fields').remove();
                var rows = $(this).find('tbody tr');
                rows.each(function (index, element) {
                    $(element).attr('data-sort-order', index + 1); // Update the sort order
                    $(element).attr('data-section-layout-id', sectionId); // Update the section ID
                });

                if (rows.length === 0) {
                    $(this).find('tbody').append('<tr class="no-fields"><td colspan="10">No fields available</td></tr>');
                }
            });
        },
        receive: function receive(event, ui) {
            // Remove placeholder if present in the new section
            $(this).find('.placeholder').remove();
        }
    }).disableSelection();
});

document.addEventListener('DOMContentLoaded', function () {
    var saveButton = document.getElementById('saveOrderButton');

    if (!saveButton) {
        return;
    }

    saveButton.addEventListener('click', function () {
        var data = [];

        // Loop through each row in the sortable table
        document.querySelectorAll('.sortable-table tbody tr.item').forEach(function (row) {
            var order = row.dataset.orderCount; // Get the order count from the row
            var itemId = row.dataset.item; // Get the item ID from the row

            var isRequiredInput = document.querySelector("#cka_" + order);
            var isVisibleInput = document.querySelector("#flexSwitchCheckDefault_" + order);

            var isRequired = isRequiredInput ? isRequiredInput.checked : false;
            var isVisible = isVisibleInput ? isVisibleInput.checked : false;

            var item = {
                TableName: row.dataset.tableName,
                AccountId: row.dataset.accountId,
                FieldName: row.dataset.fieldName,
                Id: itemId,
                CustomFieldTypeName: row.dataset.customFieldTypeName,
                CustomFieldTypeId: row.dataset.customFieldTypeId,
                SectionLayoutId: row.dataset.sectionLayoutId,
                IsNullable: row.dataset.isNullable === 'True',
                IsLocked: row.dataset.isLocked === 'True',
                IsRequired: isRequired,
                IsVisible: isVisible,
                SortOrder: row.dataset.sortOrder,
                IsCustomField: row.dataset.isCustomField === 'True'
            };

            data.push(item);
        });

        if (confirm('Are you sure you want to update?')) {
            fetch('/AdminSetting/UpdateOrder', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(data)
            }).then(function (response) {
                return response.json();
            }).then(function (result) {
                alert("Update Successful.");
                location.reload();
            })["catch"](function (error) {
                console.error('Error:', error);
                alert("An error occurred while updating.");
            });
        }
    });
});

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
        var form = document.getElementById('newSectionForm');
        var url = form.dataset.url;
        var data = {
            SectionName: document.querySelector('#sectionName').value,
            TableName: document.querySelector('#tableName').value
        };

        // Any other fields from your view model
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.json();
        }).then(function (result) {
            showToast('success', data.message);
            location.reload();
        })["catch"](function (error) {
            showToast('error', 'Failed to create section');
        });
    });

    document.querySelectorAll('.delete-section').forEach(function (link) {
        link.addEventListener('click', function (event) {
            var _this = this;

            event.preventDefault();

            var url = this.href; // Use the href attribute from the link
            var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            if (confirm('Are you sure you want to delete this section?')) {
                fetch(url, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': token
                    }
                }).then(function (response) {
                    return response.json();
                }).then(function (data) {
                    if (data.success) {
                        showToast('success', data.message);
                        // Optionally, remove the deleted section from the DOM
                        _this.closest('.sectionRow').remove();
                        location.reload();
                    } else {
                        showToast('error', 'Failed to delete section');
                    }
                })["catch"](function (error) {
                    console.error('Error:', error);
                    showToast('error', 'Failed to delete section');
                });
            }
        });
    });

    document.querySelectorAll('.edit-section').forEach(function (link) {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            var url = this.href; // Use the href attribute from the link
            var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            fetch(url, {
                method: 'PUT',
                headers: {
                    'RequestVerificationToken': token
                }
            }).then(function (response) {
                return response.json();
            }).then(function (data) {
                if (data.success) {
                    location.reload();
                    showToast(data.message, 'success', 'Success');
                } else {
                    showToast('Failed to edit section', 'error', 'Error');
                }
            })["catch"](function (error) {
                showToast('Failed to edit section', 'error', 'Error');
            });
        });
    });

    document.querySelectorAll('.delete-custom-field').forEach(function (link) {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            var url = this.href; // Use the href attribute from the link
            var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            fetch(url, {
                method: 'DELETE',
                headers: {
                    'RequestVerificationToken': token
                }
            }).then(function (response) {
                return response.json();
            }).then(function (data) {
                if (data.success) {
                    location.reload();
                    showToast(data.message, 'success', 'Success');
                } else {
                    showToast('Failed to edit section', 'error', 'Error');
                }
            })["catch"](function (error) {
                showToast('Failed to edit section', 'error', 'Error');
            });
        });
    });
});


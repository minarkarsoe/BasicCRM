$(function () {
    // Enable sortable for all tables with class 'sortable-table'
    $(".sortable-table tbody").sortable({
        connectWith: ".sortable-table tbody",
        placeholder: "sortable-placeholder",
        stop: function (event, ui) {
            // Remove placeholder if present
            $('.sortable-table tbody .placeholder').remove();

            // Update data-index and SortOrder attributes after sorting
            $('.sortable-table').each(function () {
                var sectionId = $(this).data('section-id');
                var rows = $(this).find('tbody tr');
                $(this).find('tbody tr.no-fields').remove();

                rows.each(function (index, element) {
                    $(element).attr('data-sort-order' , index + 1); // Update the hidden SortOrder input field
                    $(element).find('.sectionLayoutIdInput').val(sectionId); // Update the hidden SectionLayoutId input field
                    $(element).attr('data-section-id', sectionId); // Update the data-section-id attribute
                });

                if (rows.length === 0) {
                    $(this).find('tbody').append('<tr class="no-fields"><td colspan="10">No fields available</td></tr>');
                }
            });
        },
        receive: function (event, ui) {
            // Remove placeholder if present in the new section
            $(this).find('.placeholder').remove();
        }
    }).disableSelection();
});

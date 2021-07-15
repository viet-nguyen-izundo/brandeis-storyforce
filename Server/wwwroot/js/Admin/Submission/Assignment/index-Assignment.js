﻿var indexAssignment = function () {

    this.initialize = function () {
        registerEvents();
    };
    function registerEvents() {
        $('#selectAll').click(function () {
            if (document.getElementById('all').checked) {
                $(".singleCheckbox").prop("checked", true);
            } else {
                $(".singleCheckbox").prop("checked", false);
            }
        });

        $("#assignment").submit(function (e) {
            e.preventDefault(); // avoid to execute the actual submit of the form.
            var checkBoxs = $('.checkbox-form');
            var textNotes = $('.note-form');
            var results = [];

            var form = $(this);

            checkBoxs.each((checkBoxIndex, checkBox) => {
                if (checkBox.checked) {
                    results.push({
                        Note: textNotes[checkBoxIndex].value || '',
                        StoryFileId: +checkBox.getAttribute('data-file-id')
                    });
                }
            });
            var Assignment = {
                AssignedToId: document.getElementById('staff-select-box').value ? parseInt(document.getElementById('staff-select-box').value) : 0,
                AssignmentFiles: results
            }

            if (Assignment.AssignedToId > 0) {
                $('.error-Text').hide();
                var url = form.attr('action');
                if (Assignment.AssignmentFiles.length !== 0) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        contentType: "application/json;charset=utf-8",
                        data: JSON.stringify(Assignment),
                        success: function (data) {
                            window.location.href = '/';
                        },
                        cache: false,
                        error: function (err) {

                        }
                    });
                }
            } else {
                $('.error-Text').show();
            }
        });
    }
};
$(document).ready(function () {
    $('.error-Text').hide();
    $(".select2").select2({
        dropdownAutoWidth: true,
        width: '100%',
        placeholder: "Choose Staff",
        allowClear: true,

        ajax: {
            url: '/api/people/select2Filter',
            type: 'GET',
            data: function (params) {
                var query = {
                    search: params.term
                }
                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: data
                };
            }
        }
    });
   

});
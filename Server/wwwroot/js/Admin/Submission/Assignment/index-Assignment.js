var indexAssignment = function () {

    this.initialize = function () {
        registerEvents();
    };
    function registerEvents() {
        $('.error-Text').hide();
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
            var title = $('.as-Title');
            var description = $('.input-description');
            var results = [];

            var form = $(this);

            checkBoxs.each((checkBoxIndex, checkBox) => {
                if (checkBox.checked) {
                    results.push({
                        Note: textNotes[checkBoxIndex].value || '',
                        StoryFileId: +checkBox.getAttribute('data-file-id'),
                        TitleAssignment: title[checkBoxIndex].innerHTML,
                        DescriptionAssignment: description[checkBoxIndex].value || ''
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
                        success: function () {
                            M.toast({
                                html: 'Success'
                            });
                            $(".singleCheckbox").prop("checked", false);
                        },
                        cache: false,
                        error: function () {
                            M.toast({
                                html: 'False!'
                            });
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
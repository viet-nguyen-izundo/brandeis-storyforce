var indexAssignment = function () {

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

            var form = $(this);



            var url = form.attr('action');
            var formData = false;
            if (window.FormData) {
                formData = new FormData(form[0]);
            }

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                success: function (data) {
                    window.location.href = '/';
                },
                enctype: 'multipart/form-data',
                processData: false,  // Important!
                contentType: false,
                beforeSend: function () {

                },
                cache: false,
                error: function (err) {
                    //hide loading
                    $('#contact-loader').hide();
                    $('#btn-login').show();
                    // end hide loading
                    $('#message-result').html('');
                    if (err.status === 400 && err.responseText) {
                        var errMsgs = JSON.parse(err.responseText);
                        $(".kt-error-btm").remove();

                        for (field in errMsgs) {
                            switch (field) {
                                case 'UserName':
                                    $('<span class="text-danger d-block mt-2 kt-error-btm">' +
                                        errMsgs[field] +
                                        '</span>').insertAfter("#loginName");

                                    break;
                                case 'Password':
                                    $('<span class="text-danger d-block mt-2 kt-error-btm">' +
                                        errMsgs[field] +
                                        '</span>').insertAfter("#inputPassword");
                                    break;
                                default:
                                    $('#message-result').append(errMsgs[field] + '<br>');
                                    break;
                            }
                        }

                    }
                }
            });

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
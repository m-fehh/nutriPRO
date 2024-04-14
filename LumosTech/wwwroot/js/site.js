function GetBearerToken() {
    return 'Bearer ' + sessionStorage.getItem('jwtToken');
}
function AjaxGetByIdDefault(controller, id, callback) {
    $('#preloader').show();

    var url = `/${controller}/GetById/${id}`; 

    $.ajax({
        url: url,
        type: 'GET',
        headers: {
            'Authorization': GetBearerToken(),
        },
        success: function (response) {
            if (typeof callback === 'function') {
                callback(response);
            }
        },
        error: function (xhr, status, error) {
            var errorMessage = "Ocorreu um erro ao processar a solicitação.";

            if (xhr.status === 400) {
                var errorResponse = JSON.parse(xhr.responseText);
                if (errorResponse) {
                    var errorMessage = '';

                    for (var key in errorResponse) {
                        if (errorResponse[key].length > 0) {
                            errorMessage = errorResponse[key][0];
                            break;
                        }
                    }

                    if (errorMessage) {
                        $('#toastError .toast-body').text(errorMessage);
                        $('#toastError').toast('show');
                    }
                }
            } else {
                $('#toastError .toast-body').text(errorMessage);
                $('#toastError').toast('show');
            }
        },
        complete: function () {
            $('#preloader').hide();
        }
    });
}
function AjaxGetAllDefault(controller, callback) {
    $('#preloader').show();

    var url = `/${controller}/GetAll`;

    $.ajax({
        url: url,
        type: 'GET',
        headers: {
            'Authorization': GetBearerToken(),
        }, 
        success: function (response) {
            if (typeof callback === 'function') {
                callback(response);
            }
        },
        error: function (xhr, status, error) {
            var errorMessage = "Ocorreu um erro ao processar a solicitação.";

            if (xhr.status === 400) {
                var errorResponse = JSON.parse(xhr.responseText);
                if (errorResponse) {
                    var errorMessage = '';

                    for (var key in errorResponse) {
                        if (errorResponse[key].length > 0) {
                            errorMessage = errorResponse[key][0];
                            break;
                        }
                    }

                    if (errorMessage) {
                        $('#toastError .toast-body').text(errorMessage);
                        $('#toastError').toast('show');
                    }
                }
            } else {
                $('#toastError .toast-body').text(errorMessage);
                $('#toastError').toast('show');
            }
        },
        complete: function () {
            $('#preloader').hide();
        }
    });
}
function AjaxDeleteDefault(button, url) {
    var submitButton = $(button);

    submitButton.prop('disabled', true);

    $('#preloader').show();

    $.ajax({
        url: url,
        type: 'DELETE',
        headers: {
            'Authorization': GetBearerToken(),
        },
        success: function (response) {
            $('#toastSuccess .toast-body').text("O registro foi excluído com êxito.");
            $('#toastSuccess').toast('show');
            _$table.ajax.reload();
        },
        error: function (xhr, status, error) {
            var errorMessage = "Ocorreu um erro ao processar a solicitação.";

            if (xhr.status === 400) {
                var errorResponse = JSON.parse(xhr.responseText);
                if (errorResponse) {
                    var errorMessage = '';

                    for (var key in errorResponse) {
                        if (errorResponse[key].length > 0) {
                            errorMessage = errorResponse[key][0];
                            break;
                        }
                    }

                    if (errorMessage) {
                        $('#toastError .toast-body').text(errorMessage);
                        $('#toastError').toast('show');
                    }
                }
            } else {
                $('#toastError .toast-body').text(errorMessage);
                $('#toastError').toast('show');
            }
        },
        complete: function () {
            submitButton.prop('disabled', false);
            $('#preloader').hide();
        }
    });
}
function AjaxInsertDefault(button, url, form) {
    var submitButton = $(button);

    submitButton.prop('disabled', true);

    $('#preloader').show();

    $.ajax({
        url: url,
        method: 'POST',
        headers: {
            'Authorization': GetBearerToken(),
        },
        data: form,
        processData: false,
        contentType: false,
        success: function (data) {
            $('#toastSuccess .toast-body').text("Dados salvos com sucesso!");
            $('#toastSuccess').toast('show');
            setTimeout(function () {
                window.location.href = data.redirectTo;
            }, 3000);
        },
        error: function (xhr, status, error) {
            submitButton.prop('disabled', false);

            var errorMessage = "Ocorreu um erro ao processar a solicitação.";

            if (xhr.status === 400) {
                var errorResponse = JSON.parse(xhr.responseText);
                if (errorResponse) {
                    var errorMessage = '';

                    for (var key in errorResponse) {
                        if (errorResponse[key].length > 0) {
                            errorMessage = errorResponse[key][0];
                            break;
                        }
                    }

                    if (errorMessage) {
                        $('#toastError .toast-body').text(errorMessage);
                        $('#toastError').toast('show');
                    }
                }
            } else {
                $('#toastError .toast-body').text(errorMessage);
                $('#toastError').toast('show');
            }
        },
        complete: function () {
            submitButton.prop('disabled', false);
            $('#preloader').hide();
        }
    });
}
function AjaxUpdateDefault(button, url, form, modalName)
{
    var submitButton = $(button);

    submitButton.prop('disabled', true);

    $('#preloader').show();

    $.ajax({
        url: url,
        method: 'PUT',
        headers: {
            'Authorization': GetBearerToken(),
        },
        data: form,
        processData: false,
        contentType: false,
        success: function (data) {
            $(modalName).modal('hide');

            window.location.reload();
        },
        error: function (xhr, status, error) {
            submitButton.prop('disabled', false);

            var errorMessage = "Ocorreu um erro ao processar a solicitação.";
            $(modalName).modal('hide');
            if (xhr.status === 400) {
                var errorResponse = JSON.parse(xhr.responseText);
                if (errorResponse) {
                    var errorMessage = '';

                    for (var key in errorResponse) {
                        if (errorResponse[key].length > 0) {
                            errorMessage = errorResponse[key][0];
                            break;
                        }
                    }

                    if (errorMessage) {
                        $('#toastError .toast-body').text(errorMessage);
                        $('#toastError').toast('show');
                    }
                }
            } else {
                $('#toastError .toast-body').text(errorMessage);
                $('#toastError').toast('show');
            }
        },
        complete: function () {
            submitButton.prop('disabled', false);
            $('#preloader').hide();
        }
    });
}
function InitializeSelect2(selector, placeholderText) {
    $(selector).select2({
        placeholder: placeholderText,
        allowClear: true
    });
}
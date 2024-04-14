$(document).ready(function () {
    VMasker($("#Cpf")).maskPattern('999.999.999-99');

    var tenantId = $("#sessionTenantId").val();
    if (!tenantId) {
        $("#lblLoadingUnits").show();
        AjaxGetAllDefault("Tenants", function (response) {
            if (response && response.length > 0) {
                var $tenantSelect = $("#tenantSelect");
                $tenantSelect.empty();


                response.forEach(function (tenant) {
                    var unitsJson = JSON.stringify(tenant.units);

                    var option = $("<option>", {
                        value: tenant.id,
                        'data-units': unitsJson
                    }).text(tenant.name + " | " + tenant.city + "-" + tenant.state);
                    $tenantSelect.append(option);
                });


                InitializeSelect2("#tenantSelect", "Selecione o tenant");
                $("#tenantModal").modal("show");
            }
        });
    } else {
        $("#lblLoadingUnits").hide();

        AjaxGetByIdDefault("Tenants", tenantId, function (response) {
            if (response) {
                loadUnits(response.units);

                $("#sessionTenantId").val(tenantId);
            }
        });
    }

    $("#confirm-tenant").click(function () {
        $("#lblLoadingUnits").hide();
        var selectedTenantId = $("#tenantSelect").val();

        units = $("#tenantSelect option:selected").data('units');

        loadUnits(units);

        $("#sessionTenantId").val(selectedTenantId);
        $("#tenantModal").modal("hide");
    });


    var loadUnits = function (units) {
        var $unitsSelect = $("#SelectedUnits");
        $unitsSelect.empty();

        units.forEach(function (unit) {
            var option = $("<option>", {
                value: unit.id,
                selected: unit.levelName === "Matriz"
            }).text(unit.name);

            $unitsSelect.append(option);
        });

        InitializeSelect2("#SelectedUnits", "Selecione as Unidades");
    };

    $('#registerUser').on('submit', function (e) {
        e.preventDefault();

        var selectedUnits = $("#SelectedUnits").val();

        var formData = new FormData($('#registerUser')[0]);
        formData.append('SerializedUnitsList', JSON.stringify(selectedUnits));

        var url = '/Users/Insert';

        AjaxInsertDefault('#submitButton', url, formData);
    });

    $('.toggle-password').click(function () {
        var $input = $(this).closest('.input-group').find('input');
        var type = $input.attr('type') === 'password' ? 'text' : 'password';
        $input.attr('type', type);
        $(this).find('i').toggleClass('fa-eye fa-eye-slash');
    });

});

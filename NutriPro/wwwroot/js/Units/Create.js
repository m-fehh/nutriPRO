$(document).ready(function () {
    InitializeSelect2('#TenantId', 'Selecione um Tenant');

    var tenantId = $("#sessionTenantId").val();
    if (!tenantId) {
        AjaxGetAllDefault("Tenants", function (response) {
            if (response && response.length > 0) {
                var $tenantSelect = $("#tenantSelect");
                $tenantSelect.empty();

                response.forEach(function (tenant) {

                    var option = $("<option>", {
                        value: tenant.id
                    }).text(tenant.name + " | " + tenant.city + "-" + tenant.state);
                    $tenantSelect.append(option);
                });

                InitializeSelect2("#tenantSelect", "Selecione o tenant");
                $("#tenantModal").modal("show");
            }
        });
    }

    $("#confirm-tenant").click(function () {
        var selectedTenantId = $("#tenantSelect").val();

        console.log($("#tenantSelect").val());

        $("#sessionTenantId").val(selectedTenantId);
        $("#tenantModal").modal("hide");
    });

    function UpdateCnpjField() {
        var typeSelect = $("#Level");
        var cnpjInput = $("#CpfCnpj");
        var cnpjLabel = $("label[for='CpfCnpj']");

        typeSelect.val(1);
        typeSelect.prop("disabled", true);

        cnpjLabel.html('CNPJ<span class="input-label-required"></span>');
        VMasker(cnpjInput).maskPattern('99.999.999/9999-99');
    }

    UpdateCnpjField();


    $('#registerUnit').on('submit', function (e) {
        e.preventDefault();
        var formData = new FormData($('#registerUnit')[0]);
        formData.set('Level', 'Filial');

        var url = '/Units/Insert';

        AjaxInsertDefault('#submitButton', url, formData);
    });
});
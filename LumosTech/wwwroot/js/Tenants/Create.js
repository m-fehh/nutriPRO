$(document).ready(function () {
    InitializeSelect2('#Tenant_Type', 'Selecione o Nível');
    InitializeSelect2('#Tenant_State', 'Selecione o Estado');
    InitializeSelect2('#Unit_Level', 'Selecione o Nível');

    function UpdateCnpjField() {
        var selectedType = $("#select2-Tenant_Type-container").attr("title");

        $("#Unit_Level").prop("disabled", true);
        $("#Unit_Level").val("Matriz");
        if (selectedType === "PJ") {

            $("label[for='Unit_CpfCnpj']").html('CNPJ<span class="input-label-required"></span>');
            VMasker($("#Unit_CpfCnpj")).maskPattern('99.999.999/9999-99');
        } else {
            $("label[for='Unit_CpfCnpj']").html('CPF<span class="input-label-required"></span>');
            VMasker($("#Unit_CpfCnpj")).maskPattern('999.999.999-99');
        }
    }

    UpdateCnpjField();

    $("#Tenant_Type").on("change", function () {
        $("#Unit_CpfCnpj").val("");
        UpdateCnpjField();
    });

    $('#registerTenant').on('submit', function (e) {
        e.preventDefault();
        $('#preloader').hide();

        var formData = new FormData($('#registerTenant')[0]);
        var url = '/Tenants/InsertTenant';

        AjaxInsertDefault('#submitButton', url, formData);
    });
});

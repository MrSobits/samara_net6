Ext.define('B4.controller.report.AccountOperationsReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AccountOperationsReportPanel',
    mainViewSelector: '#accountOperationsReportPanel',

    requires: [
        'B4.ux.button.Update'
    ],

    refs: [
        {
            ref: 'StartDateField',
            selector: '#accountOperationsReportPanel #dfStartDate'
        },
        {
            ref: 'EndDateField',
            selector: '#accountOperationsReportPanel #dfEndDate'
        },
        {
            ref: 'MunicipalitySelectField',
            selector: '#accountOperationsReportPanel #fiasMunicipalitiesTrigerField'
        },
        {
            ref: 'AddressSelectField',
            selector: '#accountOperationsReportPanel #sfAddress'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "#accountOperationsReportPanel #fiasMunicipalitiesTrigerField": {
                change: this.onMrChange
            },
            "#accountOperationsReportPanel #sfAddress": {
                beforeload: this.beforeAddressLoad
            }
        });

        me.callParent(arguments);
    },

    onMrChange: function (field, newValue) {
        var adressField = this.getAddressSelectField();

        adressField.setDisabled(!newValue);
        adressField.setValue(null);
    },

    beforeAddressLoad: function (field, operation, store) {
        var mrField = this.getMunicipalitySelectField();

        operation.params = operation.params || {};
        operation.params.settlementId = mrField.getValue();
    },

    validateParams: function () {
        var dateStartField = this.getStartDateField().getValue();
        var dateEndField = this.getEndDateField().getValue();

        if (dateStartField == null || dateStartField == Date.min) {
            return "Не указан параметр \"Дата с\"";
        }

        if (dateEndField === null || dateEndField == Date.min) {
            return "Не указан параметр \"Дата по\"";
        }

        if (dateEndField < dateStartField) {
            return "Конечная дата должна быть позже начальной";
        }

        if (this.getMunicipalitySelectField().getValue() == null) {
            return "Не указан параметр \"Муниципальное образование\"";
        }
        
        if (this.getAddressSelectField().getValue() == null) {
            return "Не указан параметр \"Адрес\"";
        }

        return true;
    },

    getParams: function () {

        var dateStartField = this.getStartDateField();
        var dateEndField = this.getEndDateField();

        var address = this.getAddressSelectField().getValue();

        return {
            startDate: (dateStartField ? dateStartField.getValue() : null),
            endDate: (dateEndField ? dateEndField.getValue() : null),
            roId: address
        };
    }
});
Ext.define('B4.controller.report.PaymentAndBalanceReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PaymentAndBalanceReportPanel',
    mainViewSelector: '#paymentReportPanel',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    refs: [
        {
            ref: 'StartDateField',
            selector: '#paymentReportPanel #dfStartDate'
        },
        {
            ref: 'EndDateField',
            selector: '#paymentReportPanel #dfEndDate'
        },
        {
            ref: 'MunicipalitySelectField',
            selector: '#paymentReportPanel #fiasMunicipalitiesTrigerField'
        },
        {
            ref: 'AddressSelectField',
            selector: '#paymentReportPanel #sfAddress'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            "#paymentReportPanel #fiasMunicipalitiesTrigerField": {
                change: this.onMrChange
            },
            "#paymentReportPanel #sfAddress": {
                beforeload: this.beforeAddressLoad
            }
        });

        me.callParent(arguments);
    },

    onMrChange: function(field, newValue) {
        var adressField = this.getAddressSelectField();

        adressField.setDisabled(!newValue);
        adressField.setValue(null);
    },

    beforeAddressLoad: function(field, operation, store) {
        var mrField = this.getMunicipalitySelectField();

        operation.params = operation.params || {};
        operation.params.settlementId = mrField.getValue();
    },
    
    validateParams: function() {
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

    getParams: function() {

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
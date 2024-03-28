Ext.define('B4.controller.report.RepairPaymentByOwnerReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RepairPaymentByOwnerReportPanel',
    mainViewSelector: '#repairPaymentByOwnerReportPanel',

    requires: [
        'B4.ux.button.Update'
    ],

    refs: [
        {
            ref: 'StartDateField',
            selector: '#repairPaymentByOwnerReportPanel #dfStartDate'
        },
        {
            ref: 'EndDateField',
            selector: '#repairPaymentByOwnerReportPanel #dfEndDate'
        },
        {
            ref: 'MunicipalitySelectField',
            selector: '#repairPaymentByOwnerReportPanel #fiasMunicipalitiesTrigerField'
        },
        {
            ref: 'AddressSelectField',
            selector: '#repairPaymentByOwnerReportPanel #sfAddress'
        },
        {
            ref: 'FioSelectField',
            selector: '#repairPaymentByOwnerReportPanel #sfFio'
        },
        {
            ref: 'NumberSelectField',
            selector: '#repairPaymentByOwnerReportPanel #sfNumber'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "#repairPaymentByOwnerReportPanel #fiasMunicipalitiesTrigerField": {
                change: this.onMrChange
            },
            "#repairPaymentByOwnerReportPanel #sfAddress": {
                beforeload: this.beforeAddressLoad,
                change: this.onAddressChange
            },
            "#repairPaymentByOwnerReportPanel #sfFio": {
                beforeload: this.beforeFioLoad,
                change: this.onFioChange
            },
            "#repairPaymentByOwnerReportPanel #sfNumber": {
                beforeload: this.beforeNumberLoad
            }
        });

        me.callParent(arguments);
    },

    onFioChange: function (field, newValue) {
        var numberField = this.getNumberSelectField();

        numberField.setDisabled(!newValue);
        numberField.setValue(null);
    },

    onAddressChange: function (field, newValue) {
        var fioField = this.getFioSelectField();

        fioField.setDisabled(!newValue);
        fioField.setValue(null);
    },

    onMrChange: function (field, newValue) {
        var adressField = this.getAddressSelectField();

        adressField.setDisabled(!newValue);
        adressField.setValue(null);
    },

    beforeNumberLoad: function (field, operation, store) {
        var fioField = this.getFioSelectField();

        operation.params = operation.params || {};
        operation.params.accId = fioField.getValue();
    },

    beforeFioLoad: function (field, operation, store) {
        var addressField = this.getAddressSelectField();

        operation.params = operation.params || {};
        operation.params.roId = addressField.getValue();
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

        if (this.getFioSelectField().getValue() == null) {
            return "Не указан параметр \"ФИО\"";
        }

        if (this.getNumberSelectField().getValue() == null) {
            return "Не указан параметр \"№лс\"";
        }

        return true;
    },

    getParams: function () {

        var dateStartField = this.getStartDateField();
        var dateEndField = this.getEndDateField();

        var personalId = this.getNumberSelectField().getValue();

        return {
            startDate: (dateStartField ? dateStartField.getValue() : null),
            endDate: (dateEndField ? dateEndField.getValue() : null),
            personalId: personalId
        };
    }
});
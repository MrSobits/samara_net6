Ext.define('B4.controller.report.TurnoverBalanceSheet', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TurnoverBalanceSheetPanel',
    mainViewSelector: 'turnoverBalanceSheetPanel',

    init: function () {
        var me = this;

        me.control({
            "turnoverBalanceSheetPanel #municipalityR": {
                change: me.onMrChange
            },
            "turnoverBalanceSheetPanel #municipalityO": {
                beforeload: me.beforeMoLoad,
                change: me.onMoChange
            },
            "turnoverBalanceSheetPanel #locality": {
                beforeload: me.beforeLocalityLoad,
                change: me.onLocalityChange
            },
            "turnoverBalanceSheetPanel #address": {
                beforeload: me.beforeAddressLoad
            }
        });

        me.callParent(arguments);
    },

    onMrChange: function (field, newValue) {
        var me = this,
            moField = me.getMunicipalityOTriggerField(),
            localityField = me.getLocalityTriggerField(),
            addressField = me.getAddressTriggerField();

        moField.setDisabled(!newValue);
        moField.onTrigger2Click();
        localityField.onTrigger2Click();
        addressField.onTrigger2Click();
    },

    onMoChange: function (field, newValue) {
        var me = this,
            localityField = me.getLocalityTriggerField(),
            addressField = me.getAddressTriggerField();

        me.getLocalityTriggerField().setDisabled(!newValue);
        localityField.onTrigger2Click();
        addressField.onTrigger2Click();
    },

    onLocalityChange: function (field, newValue) {
        var me = this,
            addressField = me.getAddressTriggerField();

        me.getAddressTriggerField().setDisabled(!newValue);
        addressField.onTrigger2Click();
    },

    beforeMoLoad: function (field, operation) {
        var mrField = this.getMunicipalityRTriggerField();
        operation.params = operation.params || {};
        operation.params.parentId = mrField.getValue();
    },

    beforeLocalityLoad: function (field, operation) {
        var moField = this.getMunicipalityOTriggerField();
        operation.params = operation.params || {};
        operation.params.moId = moField.getValue();
    },

    beforeAddressLoad: function (field, operation) {
        var locality = this.getLocalityTriggerField();
        var moField = this.getMunicipalityOTriggerField();
        operation.params = operation.params || {};
        operation.params.locality = locality.getValue();
        operation.params.moId = moField.getValue();
    },

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'B4.view.report.TurnoverBalanceSheetPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityRTriggerField',
            selector: 'turnoverBalanceSheetPanel #municipalityR'
        },
        {
            ref: 'MunicipalityOTriggerField',
            selector: 'turnoverBalanceSheetPanel #municipalityO'
        },
        {
            ref: 'LocalityTriggerField',
            selector: 'turnoverBalanceSheetPanel #locality'
        },
        {
            ref: 'AddressTriggerField',
            selector: 'turnoverBalanceSheetPanel #address'
        },
        {
            ref: 'PeriodTriggerField',
            selector: 'turnoverBalanceSheetPanel #period'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var municipalityR = this.getMunicipalityRTriggerField();
        var municipalityO = this.getMunicipalityOTriggerField();
        var locality = this.getLocalityTriggerField();
        var address = this.getAddressTriggerField();
        var period = this.getPeriodTriggerField();

        return {
            municipalityParent: (municipalityR ? municipalityR.getValue() : null),
            municipality: (municipalityO ? municipalityO.getValue() : null),
            locality: (locality ? locality.getValue() : null),
            roIds: (address ? address.getValue() : null),
            period: (period ? period.getValue() : null)
        };
    }
});
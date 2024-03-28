Ext.define('B4.controller.report.CollectionPercentReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CollectionPercentReportPanel',
    mainViewSelector: 'collectionPercentReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField'
    ],

    stores: [
        'regop.ChargePeriod',
        'regop.ChargePeriodForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'B4.view.report.CollectionPercentReportPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityRTriggerField',
            selector: 'collectionPercentReportPanel #municipalityR'
        },
        {
            ref: 'MunicipalityOTriggerField',
            selector: 'collectionPercentReportPanel #municipalityO'
        },
        {
            ref: 'LocalityTriggerField',
            selector: 'collectionPercentReportPanel #locality'
        },
        {
            ref: 'AddressTriggerField',
            selector: 'collectionPercentReportPanel #address'
        },
        {
            ref: 'PeriodMultiSelectField',
            selector: 'collectionPercentReportPanel #period'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'collectPercReportPeriodMultiselectwindowaspect',
            fieldSelector: 'collectionPercentReportPanel #period',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#collectPercReportPeriodMunicipalitySelectWindow',
            storeSelect: 'regop.ChargePeriod',
            storeSelected: 'regop.ChargePeriodForSelected',
            columnsGridSelect: [
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "collectionPercentReportPanel #municipalityR": {
                change: me.onMrChange
            },
            "collectionPercentReportPanel #municipalityO": {
                beforeload: me.beforeMoLoad,
                change: me.onMoChange
            },
            "collectionPercentReportPanel #locality": {
                beforeload: me.beforeLocalityLoad,
                change: me.onLocalityChange
            },
            "collectionPercentReportPanel #address": {
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

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var municipalityR = this.getMunicipalityRTriggerField();
        var municipalityO = this.getMunicipalityOTriggerField();
        var locality = this.getLocalityTriggerField();
        var address = this.getAddressTriggerField();
        var period = this.getPeriodMultiSelectField();

        return {
            municipalityParent: (municipalityR ? municipalityR.getValue() : null),
            municipality: (municipalityO ? municipalityO.getValue() : null),
            locality: (locality ? locality.getValue() : null),
            roIds: (address ? address.getValue() : null),
            periodIds: (period ? period.getValue() : null)
        };
    }
});
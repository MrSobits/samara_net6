Ext.define('B4.controller.report.TurnoverBalance', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TurnoverBalancePanel',
    mainViewSelector: 'turnoverbalancepanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'regop.ClosedChargePeriod'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    municipalityTriggerFieldSelector: 'turnoverbalancepanel #tfMunicipality',
    startPeriodFieldSelector: 'turnoverbalancepanel #sfStartPeriod',
    endtPeriodFieldSelector: 'turnoverbalancepanel #sfEndPeriod',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'muncipalityMultiSelectWindowAspect',
            fieldSelector: 'turnoverbalancepanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#turnoverbalancepanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }}
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.levelMun = 1;
            }
        }
    ],
    
    validateParams: function () {
        var startPeriodField = Ext.ComponentQuery.query(this.startPeriodFieldSelector)[0];
        
        return startPeriodField && startPeriodField.isValid();
        
    },

    getParams: function () {
        var municipalityIdField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var startPeriodField = Ext.ComponentQuery.query(this.startPeriodFieldSelector)[0];
        var endtPeriodField = Ext.ComponentQuery.query(this.endtPeriodFieldSelector)[0];

        return {
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            startPeriod: (startPeriodField ? startPeriodField.getValue() : null),
            endPeriod: (endtPeriodField ? endtPeriodField.getValue() : null)
        };
    }
});
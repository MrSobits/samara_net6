Ext.define('B4.controller.report.HousesExcessMargSum', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.HousesExcessMargSumPanel',
    mainViewSelector: 'reportHousesExcessMargSumPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update',
        'dict.municipality.ByParam'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'reportHousesExcessMargSumPanel [name=Municipalities]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportHousesExcessMargSumPanelMultiselectwindowaspect',
            fieldSelector: 'reportHousesExcessMargSumPanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportPubDpkrExtPanelMuSelectWindow',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        store: Ext.create('B4.store.dict.municipality.ByParam'),
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' }
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var mcpField = this.getMunicipalityTriggerField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null)
        };
    }
});
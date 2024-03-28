Ext.define('B4.controller.report.ProtocolTotalTable', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ProtocolTotalTablePanel',
    mainViewSelector: '#protocolTotalTablePanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    municipalityTriggerFieldSelector: '#protocolTotalTablePanel #tfMunicipality',
    dateStartFieldSelector: '#protocolTotalTablePanel #dfDateStart',
    dateEndFieldSelector: '#protocolTotalTablePanel #dfDateEnd',
    canceledFieldSelector: '#protocolTotalTablePanel #cbCanceled',
    returnedFieldSelector: '#protocolTotalTablePanel #cbReturned',
    remarkedFieldSelector: '#protocolTotalTablePanel #cbRemarked',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocolTotalTableMultiselectwindowaspect',
            fieldSelector: '#protocolTotalTablePanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolTotalTablePanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
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

    validateParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.dateStartFieldSelector)[0];
        if (!dateStartField.isValid()) {
            return "Не указан параметр \"Начало периода\"";
        }

        var dateEndField = Ext.ComponentQuery.query(this.dateEndFieldSelector)[0];
        if (!dateEndField.isValid()) {
            return "Не указан параметр \"Окончание периода\"";
        }
        
        return true;
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var dateStartField = Ext.ComponentQuery.query(this.dateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.dateEndFieldSelector)[0];

        var canceledField = Ext.ComponentQuery.query(this.canceledFieldSelector)[0];
        var returnedField = Ext.ComponentQuery.query(this.returnedFieldSelector)[0];
        var remarkedField = Ext.ComponentQuery.query(this.remarkedFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            returned: (returnedField ? returnedField.getValue() : null),
            canceled: (canceledField ? canceledField.getValue() : null),
            remarked: (remarkedField ? remarkedField.getValue() : null)
        };
    }
});
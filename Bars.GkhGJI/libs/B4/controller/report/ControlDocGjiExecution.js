Ext.define('B4.controller.report.ControlDocGjiExecution', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ControlDocGjiExecutionPanel',
    mainViewSelector: '#controlDocExecPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],


    MunicipalityTriggerFieldSelector: '#controlDocExecPanel #tfMunicipality',
    InspectorsTriggerFieldSelector: '#controlDocExecPanel #tfInspectors',
    DateStartFieldSelector: '#controlDocExecPanel #dfDateStart',
    DateEndFieldSelector: '#controlDocExecPanel #dfDateEnd',
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'controlDocExecMultiselectwindowaspect',
            fieldSelector: '#controlDocExecPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlDocExecPanelMunicipalitySelectWindow',
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
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'controlDocExecInspMultiselectwindowaspect',
            fieldSelector: '#controlDocExecPanel #tfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlDocExecPanelInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            textProperty: 'Fio'
        }
    ],

    validateParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid());
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.MunicipalityTriggerFieldSelector)[0];
        var inspField = Ext.ComponentQuery.query(this.InspectorsTriggerFieldSelector)[0];
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            inspectorIds: (inspField ? inspField.getValue() : null)
        };
    }
});
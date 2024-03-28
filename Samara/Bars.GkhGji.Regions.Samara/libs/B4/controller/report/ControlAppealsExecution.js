Ext.define('B4.controller.report.ControlAppealsExecution', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ControlAppealsExecutionPanel',
    mainViewSelector: '#controlAppealsExecutionPanel',

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

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#controlAppealsExecutionPanel #tfMunicipality'
        },
        {
            ref: 'InspectorsTriggerField',
            selector: '#controlAppealsExecutionPanel #tfInspectors'
        },
        {
            ref: 'DateStartField',
            selector: '#controlAppealsExecutionPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#controlAppealsExecutionPanel #dfDateEnd'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'controlAppealsExecutionMultiselectwindowaspect',
            fieldSelector: '#controlAppealsExecutionPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlAppealsExecutionPanelMunicipalitySelectWindow',
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
            name: 'controlAppealsExecutionInspMultiselectwindowaspect',
            fieldSelector: '#controlAppealsExecutionPanel #tfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlAppealsExecutionPanelInspectorSelectWindow',
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
        var muId = this.getMunicipalityTriggerField();
        var inspId = this.getInspectorsTriggerField();
        var start = this.getDateStartField();
        var end = this.getDateEndField();
        return (muId && muId.isValid() && inspId && inspId.isValid() && start && start.isValid() && end && end.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var inspField = this.getInspectorsTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            inspectorIds: (inspField ? inspField.getValue() : null)
        };
    }
});
Ext.define('B4.controller.report.ShortProgramByTypeWork', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ShortProgramByTypeWorkPanel',
    mainViewSelector: 'shortprogbytypeworkpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        { ref: 'municipalityTriggerField', selector: 'shortprogbytypeworkpanel gkhtriggerfield[name="Municipalities"]' },
        { ref: 'dateTimeReportField', selector: 'shortprogbytypeworkpanel datefield[name="DateTimeReport"]' },
        { ref: 'typeWorksField', selector: 'shortprogbytypeworkpanel gkhtriggerfield[name="TypeWorks"]' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'shortProgramByTypeWorkMultiselectwindowaspect',
            fieldSelector: 'shortprogbytypeworkpanel gkhtriggerfield[name="Municipalities"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#shortProgramByTypeWorkMuSelectWin',
            storeSelect: 'dict.MunicipalityForSelect',
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
            name: 'shortProgramByTypeWorkTpMultiselectwindowaspect',
            fieldSelector: 'shortprogbytypeworkpanel gkhtriggerfield[name="TypeWorks"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#shortProgramByTypeWorkTpMuSelectWin',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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
        var mcpField = this.getMunicipalityTriggerField(),
            dateTimeReportField = this.getDateTimeReportField(),
            typeWorksField = this.getTypeWorksField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateTimeReport: (dateTimeReportField ? dateTimeReportField.getValue() : null),
            typeWorks: (typeWorksField ? typeWorksField.getValue() : null)
        };
    }
});
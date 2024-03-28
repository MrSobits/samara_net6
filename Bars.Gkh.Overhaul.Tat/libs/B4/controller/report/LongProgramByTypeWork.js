Ext.define('B4.controller.report.LongProgramByTypeWork', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.LongProgramByTypeWorkPanel',
    mainViewSelector: 'longprogbytypeworkpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.WorkSelect',
        'dict.WorkSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        { ref: 'municipalityTriggerField', selector: 'longprogbytypeworkpanel gkhtriggerfield[name="Municipalities"]' },
        { ref: 'dateTimeReportField', selector: 'longprogbytypeworkpanel datefield[name="DateTimeReport"]' },
        { ref: 'startYearField', selector: 'longprogbytypeworkpanel numberfield[name="StartYear"]' },
        { ref: 'endYearField', selector: 'longprogbytypeworkpanel numberfield[name="EndYear"]' },
        { ref: 'typeWorksField', selector: 'longprogbytypeworkpanel gkhtriggerfield[name="TypeWorks"]' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'longProgramByTypeWorkMultiselectwindowaspect',
            fieldSelector: 'longprogbytypeworkpanel gkhtriggerfield[name="Municipalities"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#longProgramByTypeWorkMuSelectWin',
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
            name: 'longProgramByTypeWorkTpMultiselectwindowaspect',
            fieldSelector: 'longprogbytypeworkpanel gkhtriggerfield[name="TypeWorks"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#longProgramByTypeWorkTpMuSelectWin',
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
            startYearField = this.getStartYearField(),
            endYearField = this.getEndYearField(),
            typeWorksField = this.getTypeWorksField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateTimeReport: (dateTimeReportField ? dateTimeReportField.getValue() : null),
            startYear: (startYearField ? startYearField.getValue() : null),
            endYear: (endYearField ? endYearField.getValue() : null),
            typeWorks: (typeWorksField ? typeWorksField.getValue() : null)
        };
    }
});
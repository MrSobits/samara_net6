Ext.define('B4.controller.report.LongProgramReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.LongProgramReportPanel',
    mainViewSelector: 'longprogramreportpanel',

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
        'report.LongProgramReportPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        { ref: 'fieldMunicipality', selector: 'longprogramreportpanel [name=Municipalities]' },
        { ref: 'fieldDateReport', selector: 'longprogramreportpanel [name=DateReport]' },
        { ref: 'fieldYearReport', selector: 'longprogramreportpanel [name=YearProgram]' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'longprogramreportMultiselectwindowaspect',
            fieldSelector: 'longprogramreportpanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#longprogramreportMunicipalitySelectWindow',
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
        return true;
    },

    getParams: function () {
        var mcpField = this.getFieldMunicipality(),
            dateReportField = this.getFieldDateReport();
        var yearField = this.getFieldYearReport();

        return {
            muIds: (mcpField ? mcpField.getValue() : null),
            dateReport: (dateReportField ? dateReportField.getValue() : null),
            year: (yearField ? yearField.getValue() : null)
        };
    }
});
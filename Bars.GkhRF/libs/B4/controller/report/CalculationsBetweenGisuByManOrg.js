Ext.define('B4.controller.report.CalculationsBetweenGisuByManOrg', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CalculationsBetweenGisuByManOrgPanel',
    mainViewSelector: '#calculationsBetweenGisuByManOrgPanel',

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

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#calculationsBetweenGisuByManOrgPanel #tfMunicipality'
        },
        {
            ref: 'ReportDateField',
            selector: '#calculationsBetweenGisuByManOrgPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'MunMultiselectwindowaspect',
            fieldSelector: '#calculationsBetweenGisuByManOrgPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#calculationsBetweenGisuByManOrgPanelMunicipalitySelectWindow',
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
        var reportDate = this.getReportDateField().getValue();
        if (reportDate == null || reportDate == Date.min) {
            return "Не указан параметр \"Дата отчета\"";
        }

        return true;
    },

    getParams: function () {
        var municipalityIdField = this.getMunicipalityTriggerField();
        var reportDateField = this.getReportDateField();

        return {
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
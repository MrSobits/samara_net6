﻿Ext.define('B4.controller.report.FillFinanceDataRatingCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.FillFinanceDataRatingCrPanel',
    mainViewSelector: '#fillFinanceDataRatingCrPanel',

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
        {
            ref: 'MunicipalityTriggerField',
            selector: '#fillFinanceDataRatingCrPanel #tfMunicipality'
        },
        {
            ref: 'PeriodDiSelectField',
            selector: '#fillFinanceDataRatingCrPanel #sfPeriodDi'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'fillFinanceDataRatingCrMultiselectwindowaspect',
            fieldSelector: '#fillFinanceDataRatingCrPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#fillFinanceDataRatingCrMunicipalitySelectWindow',
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

        var mcpField = this.getMunicipalityTriggerField();
        var periodDiSelectField = this.getPeriodDiSelectField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            periodDi: (periodDiSelectField ? periodDiSelectField.getValue() : null)
        };
    }
});
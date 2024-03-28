Ext.define('B4.controller.report.WorkWinterInfo', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.WorkWinterInfoPanel',
    mainViewSelector: '#reportWorkWinterInfoPanel',

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

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    municipalityTriggerFieldSelector: '#reportWorkWinterInfoPanel #tfMunicipality',
    reportDateFieldSelector: '#reportWorkWinterInfoPanel #dfReportDate',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportWorkWinterInfoMultiselectwindowaspect',
            fieldSelector: '#reportWorkWinterInfoPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportWorkWinterInfoPanelMunicipalitySelectWindow',
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

        var heatSeasonPeriodField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];
        if (!heatSeasonPeriodField.isValid()) {
            return "Не указан параметр \"Дата отчета\"";
        }

        return true;
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
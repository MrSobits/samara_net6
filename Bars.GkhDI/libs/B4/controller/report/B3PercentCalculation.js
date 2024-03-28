Ext.define('B4.controller.report.B3PercentCalculation', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.B3PercentCalculationPanel',
    mainViewSelector: '#b3PercentCalculationReportPanel',

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
            selector: '#b3PercentCalculationReportPanel #tfMunicipality'
        },
        {
            ref: 'DateReportField',
            selector: '#b3PercentCalculationReportPanel #dfDate'
        },
        {
            ref: 'PeriodDiSelectField',
            selector: '#b3PercentCalculationReportPanel #sfPeriodDi'
        },
        {
            ref: 'TranssferredManagCombobox',
            selector: '#b3PercentCalculationReportPanel #cbTranssferredManag'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'percentCalculationMultiselectwindowaspect',
            fieldSelector: '#b3PercentCalculationReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#b3PercentCalculationPanelMunicipalitySelectWindow',
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

    init: function () {
        var actions = {};
        actions['#b3PercentCalculationReportPanel #sfPeriodDi'] = { 'beforeload': { fn: this.onBeforePeriodDi, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforePeriodDi: function (field, options) {
        options.params = {};
        options.params.endDate = new Date(2012,11,31);
    },

    validateParams: function () {
        return true;
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var dateReportField = this.getDateReportField();
        var periodDiSelectField = this.getPeriodDiSelectField();
        var transsferredManagCombobox = this.getTranssferredManagCombobox();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateReport: (dateReportField ? dateReportField.getValue() : null),
            periodDi: (periodDiSelectField ? periodDiSelectField.getValue() : null),
            transsferredManag: (transsferredManagCombobox ? transsferredManagCombobox.getValue() : null)
        };
    }
});
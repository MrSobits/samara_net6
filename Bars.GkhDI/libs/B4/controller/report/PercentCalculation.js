Ext.define('B4.controller.report.PercentCalculation', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.PercentCalculationPanel',
    mainViewSelector: '#percentCalculationReportPanel',

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
            selector: '#percentCalculationReportPanel #tfMunicipality'
        },
        {
            ref: 'DateReportField',
            selector: '#percentCalculationReportPanel #dfDate'
        },
        {
            ref: 'PeriodDiSelectField',
            selector: '#percentCalculationReportPanel #sfPeriodDi'
        },
        {
            ref: 'TranssferredManagCombobox',
            selector: '#percentCalculationReportPanel #cbTranssferredManag'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'percentCalculationMultiselectwindowaspect',
            fieldSelector: '#percentCalculationReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#percentCalculationPanelMunicipalitySelectWindow',
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
        actions['#percentCalculationReportPanel #sfPeriodDi'] = { 'beforeload': { fn: this.onBeforePeriodDi, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforePeriodDi: function (field, options) {
        options.params = {};
        options.params.startDate = new Date(2013, 0, 1);
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
Ext.define('B4.controller.report.OverhaulMkdIndicators', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OverhaulMkdIndicatorsPanel',
    mainViewSelector: 'overhaulmkdindicatorspanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.municipality.ByParam',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'overhaulmkdindicatorspanel #tfMunicipality'
        },
        {
            ref: 'StartYearField',
            selector: 'overhaulmkdindicatorspanel #numStartYear'
        },
        {
            ref: 'EndYearField',
            selector: 'overhaulmkdindicatorspanel #numFinishYear'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'overhaulMkdIndicatorsMultiselectwindowaspect',
            fieldSelector: 'overhaulmkdindicatorspanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#overhaulMkdIndicatorsPanelMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ByParam',
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
        var startYearField = this.getStartYearField().getValue();
        var endYearField = this.getEndYearField().getValue();

        if (startYearField > endYearField) {
            return "Значение начала периода не может быть больше значения конца!";
        }

        return true;
    },

    getParams: function () {
        var mcpField = this.getMunicipalityField();
        var startYearField = this.getStartYearField();
        var finishYearField = this.getEndYearField();

        return {
            muIds: (mcpField ? mcpField.getValue() : null),
            startDate: (startYearField ? startYearField.getValue() : null),
            finishDate: (finishYearField ? finishYearField.getValue() : null)
        };
    }
});
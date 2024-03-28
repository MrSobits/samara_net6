Ext.define('B4.controller.report.ProgramCrByDpkrForm1', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ProgramCrByDpkrForm1Panel',
    mainViewSelector: 'programCrByDpkrForm1Panel',

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
            selector: 'programCrByDpkrForm1Panel #tfMunicipality'
        },
        {
            ref: 'StartYearField',
            selector: 'programCrByDpkrForm1Panel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: 'programCrByDpkrForm1Panel [name=EndYear]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrByDpkrForm1Multiselectwindowaspect',
            fieldSelector: 'programCrByDpkrForm1Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrByDpkrForm1PanelMunicipalitySelectWindow',
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
        var endYearField = this.getStartYearField().getValue();

        if (startYearField > endYearField) {
            return "Значение начала периода не может быть больше значения конца!";
        }

        return true;
    },

    getParams: function () {
        var startYearField = this.getStartYearField();
        var endYearField = this.getEndYearField();
        var moField = this.getMunicipalityField();

        return {
            startYear: (startYearField ? startYearField.getValue() : null),
            endYear: (endYearField ? endYearField.getValue() : null),
            municipalityIds: (moField ? moField.getValue() : null)
        };
    }
});
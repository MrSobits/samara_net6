Ext.define('B4.controller.report.ProgramCrByDpkrForm2', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ProgramCrByDpkrForm2Panel',
    mainViewSelector: 'reportprogcrbydpkrform2panel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.municipality.ByParam',
        'dict.MunicipalityForSelected',
         'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.ProgramCrByDpkrForm2Panel'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'reportprogcrbydpkrform2panel [name=Municipality]'
        },
        {
            ref: 'StartYearField',
            selector: 'reportprogcrbydpkrform2panel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: 'reportprogcrbydpkrform2panel [name=EndYear]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportProgramCrByDpkrForm2MuMultiselectwinaspect',
            fieldSelector: 'reportprogcrbydpkrform2panel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportProgramCrByDpkrForm2MuSelectWin',
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
                }
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
        var muId = this.getMunicipalityField();
        return muId && muId.isValid();
    },

    getParams: function () {
        var moField = this.getMunicipalityField();
        var startYearField = this.getStartYearField();
        var endYearField = this.getEndYearField();
        
        return {
            municipalityIds: (moField ? moField.getValue() : null),
            startYear: (startYearField ? startYearField.getValue() : null),
            endYear: (endYearField ? endYearField.getValue() : null)
        };
    }
});
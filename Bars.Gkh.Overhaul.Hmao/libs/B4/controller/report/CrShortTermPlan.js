Ext.define('B4.controller.report.CrShortTermPlan', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CrShortTermPlanPanel',
    mainViewSelector: 'reportcrshorttermplanpanel',

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
        'SelectWindow.MultiSelectWindow',
        'report.CrShortTermPlanPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'reportcrshorttermplanpanel [name=Municipality]'
        },
        {
            ref: 'StartYearField',
            selector: 'reportcrshorttermplanpanel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: 'reportcrshorttermplanpanel [name=EndYear]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportCrShortTermPlanMunicipalityMultiselectwindowaspect',
            fieldSelector: 'reportcrshorttermplanpanel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportCrShortTermPlanMunicipalitySelectWindow',
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
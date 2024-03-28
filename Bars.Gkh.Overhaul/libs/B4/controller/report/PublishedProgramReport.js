Ext.define('B4.controller.report.PublishedProgramReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PublishedProgramReportPanel',
    mainViewSelector: 'reportPublishedProgramReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.municipality.ByParam',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.PublishedProgramReportPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'reportPublishedProgramReportPanel #tfMunicipality'
        },
        {
            ref: 'StartYearField',
            selector: 'reportPublishedProgramReportPanel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: 'reportPublishedProgramReportPanel [name=EndYear]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportPublishedProgramReportMunicipalityMultiSelectWindowAspect',
            fieldSelector: 'reportPublishedProgramReportPanel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportPublishedProgramReportMunicipalityMultiSelectWindow',
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
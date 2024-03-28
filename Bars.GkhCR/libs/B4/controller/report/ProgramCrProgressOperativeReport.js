Ext.define('B4.controller.report.ProgramCrProgressOperativeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ProgramCrProgressOperativeReportPanel',
    mainViewSelector: '#programCrProgressOperativeReportPanel',

    views: [
        'report.ListByManyApartmentsHousesPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],
    
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    refs: [
        {
            ref: 'SfProgramCr',
            selector: '#programCrProgressOperativeReportPanel #sfProgramCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#programCrProgressOperativeReportPanel #tfMunicipality'
        },
        {
            ref: 'ReportDateField',
            selector: '#programCrProgressOperativeReportPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrProgressOperativeReportMultiselectwindowaspect',
            fieldSelector: '#programCrProgressOperativeReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrProgressOperativeReportMunicipalitySelectWindow',
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
        var prCr = this.getSfProgramCr();
        var date = this.getReportDateField();
        return (prCr && prCr.isValid() && date && date.isValid());
    },

    init: function () {
        this.control({
            '#listByManyApartmentsHousesPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getSfProgramCr();
        var municipalityIdField = this.getMunicipalityTriggerField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
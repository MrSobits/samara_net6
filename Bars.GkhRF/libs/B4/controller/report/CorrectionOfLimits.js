Ext.define('B4.controller.report.CorrectionOfLimits', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CorrectionOfLimitsPanel',
    mainViewSelector: '#correctionOfLimitsPanel',

    views: [
        'report.EconomyByTypeWorkPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    refs: [
        {
            ref: 'SfProgramCr1',
            selector: '#correctionOfLimitsPanel #sfProgramCr1'
        },
        {
            ref: 'SfProgramCr2',
            selector: '#correctionOfLimitsPanel #sfProgramCr2'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#correctionOfLimitsPanel #tfMunicipality'
        },
        {
            ref: 'ReportDateField',
            selector: '#correctionOfLimitsPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'correctionOfLimitsPanelMultiselectwindowaspect',
            fieldSelector: '#correctionOfLimitsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#correctionOfLimitsPanelMunicipalitySelectWindow',
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
        var programCr1 = this.getSfProgramCr1().getValue();
        var programCr2 = this.getSfProgramCr2().getValue();
        var reportDate = this.getReportDateField().getValue();

        if (programCr1 == null || programCr1 === "") {
            return "Не указан параметр \"Программа 1\"";
        }
        
        if (programCr2 == null || programCr2 === "") {
            return "Не указан параметр \"Программа 2\"";
        }

        if (reportDate == null || reportDate == Date.min) {
            return "Не указан параметр \"Дата отчета\"";
        }
        
        return true;
    },

    init: function () {
        this.control(
            {
                '#correctionOfLimitsPanel #sfProgramCr1': {
                    beforeload: function (store, operation) {
                        operation.params = {};
                        operation.params.notOnlyHidden = true;
                    }
                },
                '#correctionOfLimitsPanel #sfProgramCr2': {
                    beforeload: function (store, operation) {
                        operation.params = {};
                        operation.params.notOnlyHidden = true;
                    }
                }
            }
        );
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField1 = this.getSfProgramCr1();
        var programmField2 = this.getSfProgramCr2();
        var municipalityIdField = this.getMunicipalityTriggerField();
        var reportDateField = this.getReportDateField();
        
        return {
            programCrId1: (programmField1 ? programmField1.getValue() : null),
            programCrId2: (programmField2 ? programmField2.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
Ext.define('B4.controller.report.ProgramCrReport', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ProgramCrReportPanel',
    mainViewSelector: '#programCrReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#programCrReportPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#programCrReportPanel #sfProgramCr'
        },
        {
            ref: 'DopProgramField',
            selector: '#programCrReportPanel #sfDopProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#programCrReportPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrMultiselectwindowaspect',
            fieldSelector: '#programCrReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrReportPanelMunicipalitySelectWindow',
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
        var prCr1 = this.getProgramCrSelectField();
        var date = this.getReportDateField();
        var prCr2 = this.getDopProgramField();
        return (prCr1 && prCr1.isValid() && date && date.isValid() && prCr2 && prCr2.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var dopProgramField = this.getDopProgramField();

        //получаем компоне
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            dopProgram: (dopProgramField ? dopProgramField.getValue() : null)
        };
    }
});
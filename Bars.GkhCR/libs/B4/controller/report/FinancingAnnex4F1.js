Ext.define('B4.controller.report.FinancingAnnex4F1', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.FinancingAnnex4F1Panel',
    mainViewSelector: '#financingAnnex4F1Panel',

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
            selector: '#financingAnnex4F1Panel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#financingAnnex4F1Panel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#financingAnnex4F1Panel #dfReportDate'
        },
        {
            ref: 'TypeFinanceGroup',
            selector: '#financingAnnex4F1Panel #cbTypeFinanceGroup'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'financingAnnex4F1Multiselectwindowaspect',
            fieldSelector: '#financingAnnex4F1Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#financingAnnex4F1ReportPanelMunicipalitySelectWindow',
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
        var prCrId = this.getProgramCrSelectField();
        var date = this.getReportDateField();
        var fin = this.getTypeFinanceGroup();
        return (prCrId && prCrId.isValid() && date && date.isValid() && fin && fin.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var typeFinanceGroup = this.getTypeFinanceGroup();

        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            typeFinanceGroup: (typeFinanceGroup ? typeFinanceGroup.getValue() : null)
        };
    }
});
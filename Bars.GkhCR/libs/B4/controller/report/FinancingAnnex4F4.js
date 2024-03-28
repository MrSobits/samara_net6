Ext.define('B4.controller.report.FinancingAnnex4F4', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.FinancingAnnex4F4Panel',
    mainViewSelector: '#financingAnnex4F4Panel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.FinancingAnnex4F4Panel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#financingAnnex4F4Panel #tfMunicipality'
        },
        {
            ref: 'ProgramCrField',
            selector: '#financingAnnex4F4Panel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#financingAnnex4F4Panel #dfReportDate'
        },
        {
            ref: 'TypeFinanceGroupField',
            selector: '#financingAnnex4F4Panel #cbxTypeFinGroup'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'financingAnnex4F4Multiselectwindowaspect',
        fieldSelector: '#financingAnnex4F4Panel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#financingAnnex4F4PanelMunicipalitySelectWindow',
        storeSelect: 'dict.MunicipalityForSelect',
        storeSelected: 'dict.MunicipalityForSelected',
        columnsGridSelect: [
            {
                header: 'Наименование',
                xtype: 'gridcolumn',
                dataIndex: 'Name',
                flex: 1,
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
        var finId = this.getTypeFinanceGroupField();
        var prCrId = this.getProgramCrField();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && finId && finId.isValid() && date && date.isValid());
    },

    init: function () {
        var actions = {};
        actions['#financingAnnex4F4Panel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var mcpField = this.getMunicipalityTriggerField();
        var reportDateField = this.getReportDateField();
        var typeFinanceGroupField = this.getTypeFinanceGroupField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            typeFinanceGroup: (typeFinanceGroupField ? typeFinanceGroupField.getValue() : null)
        };
    }
});
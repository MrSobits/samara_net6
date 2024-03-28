Ext.define('B4.controller.report.CountRealObjByBacklogWork', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CountRealObjByBacklogWorkPanel',
    mainViewSelector: '#countRealObjByBacklogWorkPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.CountRealObjByBacklogWorkPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#countRealObjByBacklogWorkPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrField',
            selector: '#countRealObjByBacklogWorkPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#countRealObjByBacklogWorkPanel #dfReportDate'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#countRealObjByBacklogWorkPanel #tfFinSources'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'countRealObjByBacklogWorkMultiselectwindowaspect',
        fieldSelector: '#countRealObjByBacklogWorkPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#countRealObjByBacklogWorkPanelMunicipalitySelectWindow',
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
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'finSourcesMultiselectwindowaspect',
        fieldSelector: '#countRealObjByBacklogWorkPanel #tfFinSources',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#countRealObjByBacklogWorkPanelFinSourcesSelectWindow',
        storeSelect: 'dict.FinanceSourceSelect',
        storeSelected: 'dict.FinanceSourceSelected',
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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
        var prCrId = this.getProgramCrField();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    init: function () {
        var actions = {};
        actions['#worksProgressPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
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
        var finSourcesField = this.getFinSourcesTriggerField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
Ext.define('B4.controller.report.ObjectCrInfoService', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ObjectCrInfoServicePanel',
    mainViewSelector: '#objectCrInfoServicePanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.WorksProgressPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#objectCrInfoServicePanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#objectCrInfoServicePanel #dfReportDate'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#objectCrInfoServicePanel #tfFinSources'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'finSourcesMultiselectwindowaspect',
        fieldSelector: '#objectCrInfoServicePanel #tfFinSources',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#objectCrInfoServicePanelFinSourcesSelectWindow',
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
        var prCr = this.getProgramCrField();
        var date = this.getReportDateField();
        return (prCr && prCr.isValid() && date && date.isValid());
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
        var finSourcesField = this.getFinSourcesTriggerField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
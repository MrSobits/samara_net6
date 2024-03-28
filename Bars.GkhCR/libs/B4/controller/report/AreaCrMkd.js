Ext.define('B4.controller.report.AreaCrMkd', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AreaCrMkdPanel',
    mainViewSelector: '#areaCrMkdPanel',

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
        'report.AreaCrMkdPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#areaCrMkdPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#areaCrMkdPanel #dfReportDate'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#areaCrMkdPanel #tfFinSources'
        },
        {
            ref: 'GraphField',
            selector: '#areaCrMkdPanel #graph'
        }
    
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'finSourcesMultiselectwindowaspect',
        fieldSelector: '#areaCrMkdPanel #tfFinSources',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#areaCrMkdPanelFinSourcesSelectWindow',
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
    

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var finSourcesField = this.getFinSourcesTriggerField();
        var reportDateField = this.getReportDateField();
        var graphField = this.getGraphField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null),
            graph: (graphField ? graphField.getValue() : null)
        };
    }
});
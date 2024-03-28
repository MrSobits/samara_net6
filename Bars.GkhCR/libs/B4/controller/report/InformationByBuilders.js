Ext.define('B4.controller.report.InformationByBuilders', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationByBuildersPanel',
    mainViewSelector: '#informationByBuildersPanel',

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
            ref: 'DateReportField',
            selector: '#informationByBuildersPanel #dfReportDate'
        },
        {
            ref: 'ProgramCrField',
            selector: '#informationByBuildersPanel #sfProgramCr'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#informationByBuildersPanel #tfFinSources'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'finSourcesMultiselectwindowaspect',
        fieldSelector: '#informationByBuildersPanel #tfFinSources',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#informationByBuildersFinSourcesSelectWindow',
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
        var date = this.getDateReportField();
        var prCrId = this.getProgramCrField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var dateReportField = this.getDateReportField();
        var finSourcesField = this.getFinSourcesTriggerField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            dateReport: (dateReportField ? dateReportField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null)
        };
    }
});
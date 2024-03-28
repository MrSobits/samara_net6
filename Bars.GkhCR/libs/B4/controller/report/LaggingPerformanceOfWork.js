Ext.define('B4.controller.report.LaggingPerformanceOfWork', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.LaggingPerformanceOfWorkPanel',
    mainViewSelector: '#laggingPerformanceOfWorkPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.WorkSelect',
        'dict.WorkSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.LaggingPerformanceOfWorkPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#laggingPerformanceOfWorkPanel #sfProgramCr'
        },
        {
            ref: 'TypeWorkCrTriggerField',
            selector: '#laggingPerformanceOfWorkPanel #tfTypeWorkCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#laggingPerformanceOfWorkPanel #tfMunicipality'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#laggingPerformanceOfWorkPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#laggingPerformanceOfWorkPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'laggingPerformanceOfWorkPanelMultiselectwindowaspect',
            fieldSelector: '#laggingPerformanceOfWorkPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#laggingPerformanceOfWorkPanelMunicipalitySelectWindow',
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
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'laggingPerformanceOfWorkPanelFinMultiselectwindowaspect',
            fieldSelector: '#laggingPerformanceOfWorkPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#laggingPerformanceOfWorkPanelFinSourcesSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1
                }
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
            name: 'laggingPerformanceOfWorkPanelWorkTypeMultiselectwindowaspect',
            fieldSelector: '#laggingPerformanceOfWorkPanel #tfTypeWorkCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#laggingPerformanceOfWorkPanelWorkTypeSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1
                }
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
        var workId = this.getTypeWorkCrTriggerField();
        return (prCrId && prCrId.isValid() && date && date.isValid() && workId && workId.isValid());
    },
    
    //init: function () {
    //    var actions = {};
    //    actions['#laggingPerformanceOfWorkPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
    //    this.control(actions);

    //    this.callParent(arguments);
    //},

    //onBeforeLoadProgramCr: function (field, options) {
    //    options.params = {};
    //    options.params.notOnlyHidden = true;
    //},
    
    getParams: function () {
        var programCrField = this.getProgramCrSelectField();
        var mcpField = this.getMunicipalityTriggerField();
        var finSourcesField = this.getFinSourcesTriggerField();
        var typeWorkCrField = this.getTypeWorkCrTriggerField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            workTypeIds: (typeWorkCrField ? typeWorkCrField.getValue(): null)
        };
    }
});
Ext.define('B4.controller.report.DataStartedFinishedWorkReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DataStartedFinishedWorkReportPanel',
    mainViewSelector: '#dataStartedFinishedWorkReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.WorkSelect',
        'dict.WorkSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.DataStartedFinishedWorkReportPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#dataStartedFinishedWorkReportPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#dataStartedFinishedWorkReportPanel #dfReportDate'
        },
        {
            ref: 'WorkCrTriggerField',
            selector: '#dataStartedFinishedWorkReportPanel #tfWorkCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'dataStartedFinishedWorkReportPanelMunMultiselectwindowaspect',
            fieldSelector: '#dataStartedFinishedWorkReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dataStartedFinishedWorkReportPanelMunicipalitySelectWindow',
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
                //{ header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
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
            name: 'dataStartedFinishedWorkReportPanelWorkTypeMultiselectwindowaspect',
            fieldSelector: '#dataStartedFinishedWorkReportPanel #tfWorkCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dataStartedFinishedWorkReportPanelWorkTypeSelectWindow',
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
        var workId = this.getWorkCrTriggerField();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && workId && workId.isValid() && date && date.isValid());
    },

    getParams: function () {
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var workCrField = this.getWorkCrTriggerField();

        //получаем компонент
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            workId: (workCrField ? workCrField.getValue() : null)
        };
    }
});
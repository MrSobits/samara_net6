Ext.define('B4.controller.report.CompletionOfTheGraphicOnGivenDate', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CompletionOfTheGraphicOnGivenDatePanel',
    mainViewSelector: '#CompletionOfTheGraphicOnGivenDatePanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#CompletionOfTheGraphicOnGivenDatePanel #tfMunicipality'
        },
        {
            ref: 'DateReportField',
            selector: '#CompletionOfTheGraphicOnGivenDatePanel #dfDateReport'
        },
        {
            ref: 'ProgramCrField',
            selector: '#CompletionOfTheGraphicOnGivenDatePanel #sfProgramCr'
        },
        {
            ref: 'TimeSchedField',
            selector: '#CompletionOfTheGraphicOnGivenDatePanel #cbTimeSched'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            fieldSelector: '#CompletionOfTheGraphicOnGivenDatePanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#CompletionOfTheGraphicOnGivenDatePanelMunicipalitySelectWindow',
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
        var date = this.getDateReportField();
        var prCr = this.getProgramCrField();
        return (date && date.isValid() && prCr && prCr.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var dateReport = this.getDateReportField();
        var programCr = this.getProgramCrField();
        var timeSched = this.getTimeSchedField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateReport: (dateReport ? dateReport.getValue() : null),
            programCr: (programCr ? programCr.getValue() : null),
            timeSchedule: (timeSched ? timeSched.getValue() : null)
        };
    }
});
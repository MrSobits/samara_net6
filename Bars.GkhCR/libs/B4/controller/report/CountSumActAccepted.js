Ext.define('B4.controller.report.CountSumActAccepted', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.CountSumActAcceptedPanel',
    mainViewSelector: '#countSumActAcceptedPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'ProgramsCrTriggerField',
            selector: '#countSumActAcceptedPanel #tfProgramsCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'countSumActAcceptedMultiselectwindowaspect',
            fieldSelector: '#countSumActAcceptedPanel #tfProgramsCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#countSumActAcceptedPanelProgramsCrSelectWindow',
            storeSelect: 'dict.ProgramCrForSelect',
            storeSelected: 'dict.ProgramCrForSelected',
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
                        url: '/ProgramCr/List'
                    }
                },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params = {};
                operation.params.activePrograms = true;
            }
        }
    ],

    validateParams: function () {
        var prCrId = this.getProgramsCrTriggerField();
        return (prCrId && prCrId.isValid());
    },

    getParams: function () {

        var programsField = this.getProgramsCrTriggerField();

        return {
            programCrIds: (programsField ? programsField.getValue() : null)
        };
    }
});
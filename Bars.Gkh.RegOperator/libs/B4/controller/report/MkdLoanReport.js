Ext.define('B4.controller.report.MkdLoanReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.MkdLoanReportPanel',
    mainViewSelector: 'mkdLoanReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'ProgramsCrSelectField',
            selector: 'mkdLoanReportPanel #tfCrPrograms'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: 'mkdLoanReportPanel #tfMunicipality'
        },
        {
            ref: 'LoanStatusCombo',
            selector: 'mkdLoanReportPanel #tfLoanStatus'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'chargeReportMunicipalityMultiselectwindowaspect',
            fieldSelector: 'mkdLoanReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#chargeReportMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: { xtype: 'textfield' }
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
            name: 'programCrMkdLoanReportMultiselectwindowaspect',
            fieldSelector: 'mkdLoanReportPanel #tfCrPrograms',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#MkdLoanReportPanelProgramsCrSelectWindow',
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
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var statusField = this.getLoanStatusCombo();
        var programsField = this.getProgramsCrSelectField();
        var municipalField = this.getMunicipalityTriggerField();

        return {
            loanStatus: (statusField ? statusField.getValue() : null),
            programCrIds: (programsField ? programsField.getValue() : null),
            municipalityIds: (municipalField ? municipalField.getValue() : null)
        };
    }
});
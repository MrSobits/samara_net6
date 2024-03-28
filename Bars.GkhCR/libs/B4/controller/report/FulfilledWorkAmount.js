Ext.define('B4.controller.report.FulfilledWorkAmount', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.FulfilledWorkAmountReport',
    mainViewSelector: '#fulfilledWorkAmountReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.FulfilledWorkAmountReport'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#fulfilledWorkAmountReportPanel #tfMunicipality'
        },
        {
            ref: 'FinSourceTriggerField',
            selector: '#fulfilledWorkAmountReportPanel #tfFinSource'
        },
        {
            ref: 'ProgramCrField',
            selector: '#fulfilledWorkAmountReportPanel #sfProgramCr'
        },
        {
            ref: 'RenovTypeField',
            selector: '#fulfilledWorkAmountReportPanel #cbRenovType'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'workAmountReportMultiselectwindowaspect',
            fieldSelector: '#fulfilledWorkAmountReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workAmountReportPanelMunicipalitySelectWindow',
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
            name: 'workAmountReportFinSourceMultiselectwindowaspect',
            fieldSelector: '#fulfilledWorkAmountReportPanel #tfFinSource',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workAmountReportPanelFinSourceSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
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
        var prCrId = this.getProgramCrField();
        var renov = this.getRenovTypeField();
        return (prCrId && prCrId.isValid() && renov && renov.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programCrField = this.getProgramCrField();
        var finSourceField = this.getFinSourceTriggerField();
        var renovTypeField = this.getRenovTypeField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            programCr: (programCrField ? programCrField.getValue() : null),
            finSource: (finSourceField ? finSourceField.getValue() : null),
            renovType: (renovTypeField ? renovTypeField.getValue() : null)
        };
    }
});
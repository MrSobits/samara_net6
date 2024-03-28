Ext.define('B4.controller.report.HousesCompletedWorkFact', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.HousesCompletedWorkFactPanel',
    mainViewSelector: '#HousesCompletedWorkFactPanel',

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
            selector: '#HousesCompletedWorkFactPanel #tfMunicipality'
        },
        {
            ref: 'DateReportField',
            selector: '#HousesCompletedWorkFactPanel #dfDateReport'
        },
        {
            ref: 'ProgramCrField',
            selector: '#HousesCompletedWorkFactPanel #sfProgramCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            fieldSelector: '#HousesCompletedWorkFactPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#HousesCompletedWorkFactPanelMunicipalitySelectWindow',
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
        var prCrId = this.getProgramCrField();
        var date = this.getDateReportField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var dateReport = this.getDateReportField();
        var programCr = this.getProgramCrField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateReport: (dateReport ? dateReport.getValue() : null),
            programCr: (programCr ? programCr.getValue() : null)
        };
    }
});
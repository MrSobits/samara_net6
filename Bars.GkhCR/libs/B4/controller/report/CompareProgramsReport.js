Ext.define('B4.controller.report.CompareProgramsReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CompareProgramsPanel',
    mainViewSelector: '#compareProgramsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#compareProgramsPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrOneSelectField',
            selector: '#compareProgramsPanel #sfProgramCrOne'
        },
        {
            ref: 'ProgramCrTwoSelectField',
            selector: '#compareProgramsPanel #sfProgramCrTwo'
        },
        {
            ref: 'FinanceSourceTriggerField',
            selector: '#compareProgramsPanel #tfFinanceSource'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'compareProgramsMultiselectwindowaspect',
            fieldSelector: '#compareProgramsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrReportPanelMunicipalitySelectWindow',
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
              name: 'financialCompareProgramsMultiselectwindowaspect',
              fieldSelector: '#compareProgramsPanel #tfFinanceSource',
              multiSelectWindow: 'SelectWindow.MultiSelectWindow',
              multiSelectWindowSelector: '#CompareProgramsReportFinancialSelectWindow',
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
        var prCrOneId = this.getProgramCrOneSelectField();
        var prCrTwoId = this.getProgramCrTwoSelectField();
        return (prCrOneId && prCrOneId.isValid() && prCrTwoId && prCrTwoId.isValid());
    },

    getParams: function () {
        var municipalitiesField = this.getMunicipalityTriggerField();
        var programOneField = this.getProgramCrOneSelectField();
        var programTwoField = this.getProgramCrTwoSelectField();
        var financeSourceField = this.getFinanceSourceTriggerField();

        return {
            ProgramCrOne: programOneField ? programOneField.getValue() : null,
            ProgramCrTwo: programTwoField ? programTwoField.getValue() : null,
            Municipalities: municipalitiesField ? municipalitiesField.getValue() : null,
            FinanceSource: financeSourceField ? financeSourceField.getValue() : null
        };
    }
});
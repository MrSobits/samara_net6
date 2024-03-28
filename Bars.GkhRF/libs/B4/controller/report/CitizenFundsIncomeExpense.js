Ext.define('B4.controller.report.CitizenFundsIncomeExpense', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CitizenFundsIncomeExpense',
    mainViewSelector: '#CitizenFundsIncomeExpensePanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    refs: [
            {
                ref: 'DfReportDateStart',
                selector: '#CitizenFundsIncomeExpensePanel #dfReportDateStart'
            },
            {
                ref: 'DfReportDateFinish',
                selector: '#CitizenFundsIncomeExpensePanel #dfReportDateFinish'
            },
            {
                ref: 'TfMunicipality',
                selector: '#CitizenFundsIncomeExpensePanel #tfMunicipality'
            },
            {
                ref: 'SfProgramCr',
                selector: '#CitizenFundsIncomeExpensePanel #sfProgramCr'
            }
      ],

    views: [
        'report.CitizenFundsIncomeExpense',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
          {
              xtype: 'gkhtriggerfieldmultiselectwindowaspect',
              name: 'CitizenFundsIncomeExpenseMultiSelectWindowAspect',
              fieldSelector: '#CitizenFundsIncomeExpensePanel #tfMunicipality',
              multiSelectWindow: 'SelectWindow.MultiSelectWindow',
              multiSelectWindowSelector: '#CitizenFundsIncomeExpenseSelectWindow',
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
        var startDate = this.getDfReportDateStart().getValue();
        var endDate = this.getDfReportDateFinish().getValue();
        var municipalities = this.getTfMunicipality().getValue();
        var programCr = this.getSfProgramCr().getValue();
        
        if (programCr == null || programCr === "") {
            return "Выберите программу капитального ремонта!";
        }

        if (startDate == null || startDate == Date.min) {
            return "Не указан параметр \"Дата начала\"";
        }

        if (endDate === null || endDate == Date.min) {
            return "Не указан параметр \"Дата окончания\"";
        }
        
        if (municipalities == null || municipalities == "") {
            return "Не указан параметр \"Муниципальные образования\"";
        }
        
        return true;
    },


    getParams: function () {
        var repDateStartField = this.getDfReportDateStart();
        var repDateFinishField = this.getDfReportDateFinish();
        var mcpField = this.getTfMunicipality();
        var repCrProgram = this.getSfProgramCr();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDateStart: (repDateStartField ? repDateStartField.getValue() : null),
            reportDateFinish: (repDateFinishField ? repDateFinishField.getValue() : null),
            reportCrProgram: (repCrProgram ? repCrProgram.getValue() : null)
        };
    }
});
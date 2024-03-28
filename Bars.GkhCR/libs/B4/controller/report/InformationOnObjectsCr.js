Ext.define('B4.controller.report.InformationOnObjectsCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationOnObjectsCrPanel',
    mainViewSelector: '#informationOnObjectsCrPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'DateReportField',
            selector: '#informationOnObjectsCrPanel #dfDateReport'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#informationOnObjectsCrPanel #sfProgramCr'
        },
        {
            ref: 'FinanceSourceTriggerField',
            selector: '#informationOnObjectsCrPanel #tfFinanceSource'
        }
    ],

    aspects: [
          {
              xtype: 'gkhtriggerfieldmultiselectwindowaspect',
              name: 'financialInformationOnObjectsCrMultiselectwindowaspect',
              fieldSelector: '#informationOnObjectsCrPanel #tfFinanceSource',
              multiSelectWindow: 'SelectWindow.MultiSelectWindow',
              multiSelectWindowSelector: '#CnformationOnObjectsCrFinancialSelectWindow',
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
        
        var dateReportField = this.getDateReportField();
        var programField = this.getProgramCrSelectField();

        var isValid = true;
        var msg = 'Необходимо заполнить обязательные поля:';
        var fields = [];

        if (Ext.isEmpty(dateReportField.getValue())) {
            isValid = false;
            fields.push('На дату');
        }
        
        if (Ext.isEmpty(programField.getValue())) {
            isValid = false;
            fields.push('Программа');
        }
               
        if (!isValid) {
            return msg + fields.join(',') + '!';
        }

        return true;
    },

    getParams: function () {
        
        var dateReportField = this.getDateReportField();
        var programField = this.getProgramCrSelectField();
        var financeSourceField = this.getFinanceSourceTriggerField();

        return {
            
            dateReport: (dateReportField ? dateReportField.getValue() : null),
            ProgramCr: programField ? programField.getValue() : null,
            FinanceSource: financeSourceField ? financeSourceField.getValue() : null
            
        };
    }
});
Ext.define('B4.controller.report.Form1StateHousingInspection', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.Form1StateHousingInspectionPanel',
    mainViewSelector: '#form1StateHousingInspectionPanel',

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
            ref: 'ProgramCrField',
            selector: '#form1StateHousingInspectionPanel #sfProgramCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#form1StateHousingInspectionPanel #tfMunicipality'
        },
        {
            ref: 'ReportDateField',
            selector: '#form1StateHousingInspectionPanel #dfReportDate'
        },
         {
             ref: 'DateStartField',
             selector: '#form1StateHousingInspectionPanel #dfDateStart'
         },
        {
            ref: 'DateEndField',
            selector: '#form1StateHousingInspectionPanel #dfDateEnd'
        }
    ],

    aspects: [
         {
             /*
             аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
             */
             xtype: 'gkhtriggerfieldmultiselectwindowaspect',
             name: 'form1StateHousingInspectionMultiselectwindowaspect',
             fieldSelector: '#form1StateHousingInspectionPanel #tfMunicipality',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#form1StateHousingInspectionPanelSelectWindow',
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
        var reportDate = this.getReportDateField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var program = this.getProgramCrField();
        
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid() && reportDate && reportDate.isValid() && program && program.isValid());
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField();
        var programCrField = this.getProgramCrField();
        var reportDateField = this.getReportDateField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        
        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});
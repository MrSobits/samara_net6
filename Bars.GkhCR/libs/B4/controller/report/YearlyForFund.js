Ext.define('B4.controller.report.YearlyForFund', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.YearlyForFundPanel',
    mainViewSelector: '#yearlyForFundPanel',

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
            selector: '#yearlyForFundPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#yearlyForFundPanel #sfProgramCr'
        },
        {
            ref: 'FinSourcesField',
            selector: '#yearlyForFundPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#yearlyForFundPanel #dfReportDate'
        },
        {
            ref: 'AssemblyToField',
            selector: '#yearlyForFundPanel #cbAssemblyTo'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'yearlyForFundMunMultiselectwindowaspect',
            fieldSelector: '#yearlyForFundPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#yearlyForFundPanelMunicipalitySelectWindow',
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
            name: 'yearlyForFundFinMultiselectwindowaspect',
            fieldSelector: '#yearlyForFundPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#yearlyForFundPanelFinSourcesSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
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
        var prCr = this.getProgramCrSelectField();
        var date = this.getReportDateField();
        return (prCr && prCr.isValid() && date && date.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var finSourcesField = this.getFinSourcesField();
        var assemblyToField = this.getAssemblyToField();

        //получаем компоне
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            finSources: (finSourcesField ? finSourcesField.getValue() : null),
            assemblyTo: (assemblyToField ? assemblyToField.getValue() : null)
        };
    }
});
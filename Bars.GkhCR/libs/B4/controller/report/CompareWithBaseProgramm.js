Ext.define('B4.controller.report.CompareWithBaseProgramm', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CompareWithBaseProgrammPanel',
    mainViewSelector: '#compareWithBaseProgrammPanel',

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
            selector: '#compareWithBaseProgrammPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#compareWithBaseProgrammPanel #sfProgramCr'
        },
        {
            ref: 'ProgramCrSelectFieldAdditional',
            selector: '#compareWithBaseProgrammPanel #sfProgramCrAdd'
        },
        {
            ref: 'FinancialTriggerField',
            selector: '#compareWithBaseProgrammPanel #tfFinancial'
        },
        {
            ref: 'ReportDateField',
            selector: '#compareWithBaseProgrammPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'municipMultiselectwindowaspect',
            fieldSelector: '#compareWithBaseProgrammPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#compareWithBaseProgrammPanelMunicipalitySelectWindow',
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
            name: 'financialMultiselectwindowaspect',
            fieldSelector: '#compareWithBaseProgrammPanel #tfFinancial',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#compareWithBaseProgrammPanelFinancialSelectWindow',
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
            titleGridSelected: 'Выбранная запись'//,
            //textProperty: 'Name'
        }
    ],
    
    init: function () {
        var actions = {};
        actions['#compareWithBaseProgrammPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },
    
    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },

    validateParams: function () {
        var prCrId = this.getProgramCrSelectField();
        var prCrAddId = this.getProgramCrSelectFieldAdditional();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && prCrAddId && prCrAddId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var finField = this.getFinancialTriggerField();
        var programmField = this.getProgramCrSelectField();
        var programmFieldAdditional = this.getProgramCrSelectFieldAdditional();
        var reportDate = this.getReportDateField();
        
        
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            financialSourceIds: (finField ? finField.getValue() : null),
            programCrAdditionalId: (programmFieldAdditional ? programmFieldAdditional.getValue() : null),
            reportDate: (reportDate ? reportDate.getValue() : null)
        };
    }
});
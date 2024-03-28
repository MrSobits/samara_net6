Ext.define('B4.controller.report.QuartAndAnnualRepByFundExt', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.QuartAndAnnualRepByFundExtPanel',
    mainViewSelector: '#quartAndAnnualRepByFundExtPanel',

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
            ref: 'ProgramCrSelectField',
            selector: '#quartAndAnnualRepByFundExtPanel #sfProgramCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#quartAndAnnualRepByFundExtPanel #tfMunicipality'
        },
        {
            ref: 'FinSourcesField',
            selector: '#quartAndAnnualRepByFundExtPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#quartAndAnnualRepByFundExtPanel #dfReportDate'
        },
        {
            ref: 'AssemblyToField',
            selector: '#quartAndAnnualRepByFundExtPanel #cbAssemblyTo'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'quartAndAnnualRepByFundExtMunMultiselectwindowaspect',
            fieldSelector: '#quartAndAnnualRepByFundExtPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#quartAndAnnualRepByFundExtPanelMunicipalitySelectWindow',
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
            name: 'quartAndAnnualRepByFundExtFinMultiselectwindowaspect',
            fieldSelector: '#quartAndAnnualRepByFundExtPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#quartAndAnnualRepByFundExtPanelFinSourcesSelectWindow',
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
    
    init: function () {
        this.control({
            '#quartAndAnnualRepByFundExtPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
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
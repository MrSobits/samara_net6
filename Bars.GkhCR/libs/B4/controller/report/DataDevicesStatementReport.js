Ext.define('B4.controller.report.DataDevicesStatementReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DataDevicesStatementReportPanel',
    mainViewSelector: '#dataDevicesStatementReportPanel',

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
            selector: '#dataDevicesStatementReportPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#dataDevicesStatementReportPanel #sfProgramCr'
        },
        {
            ref: 'FinSourcesField',
            selector: '#dataDevicesStatementReportPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#dataDevicesStatementReportPanel #dfReportDate'
        }
    ],

    init: function () {
        this.control({
            '#dataDevicesStatementReportPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'dataDevicesStatementReportMunMultiselectwindowaspect',
            fieldSelector: '#dataDevicesStatementReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dataDevicesStatementReportPanelMunicipalitySelectWindow',
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
                //{ header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
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
            name: 'dataDevicesStatementReportPanelFinMultiselectwindowaspect',
            fieldSelector: '#dataDevicesStatementReportPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dataDevicesStatementReportPanelFinSourcesSelectWindow',
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
        var prCrId = this.getProgramCrSelectField();
        var date = this.getReportDateField();
        var finId = this.getFinSourcesField();
        return (prCrId && prCrId.isValid() && date && date.isValid() && finId && finId.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var finSourcesField = this.getFinSourcesField();

        //получаем компоне
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            finSources: (finSourcesField ? finSourcesField.getValue() : null)
        };
    }
});
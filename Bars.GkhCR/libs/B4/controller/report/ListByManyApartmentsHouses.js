Ext.define('B4.controller.report.ListByManyApartmentsHouses', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ListByManyApartmentsHousesPanel',
    mainViewSelector: '#listByManyApartmentsHousesPanel',

    views: [
        'report.ListByManyApartmentsHousesPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],
    
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    refs: [
        {
            ref: 'SfProgramCr',
            selector: '#listByManyApartmentsHousesPanel #sfProgramCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#listByManyApartmentsHousesPanel #tfMunicipality'
        },
        {
            ref: 'FinSourcesField',
            selector: '#listByManyApartmentsHousesPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#listByManyApartmentsHousesPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'listByManyApartmentsHousesMultiselectwindowaspect',
            fieldSelector: '#listByManyApartmentsHousesPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#listByManyApartmentsHousesPanelMunicipalitySelectWindow',
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
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                // operation.params.levelMun = 1;
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'listByManyApartmentsHousesFinMultiselectwindowaspect',
            fieldSelector: '#listByManyApartmentsHousesPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#listByManyApartmentsHousesPanelFinSourcesSelectWindow',
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
        var prCrId = this.getSfProgramCr();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    init: function () {
        this.control({
            '#listByManyApartmentsHousesPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getSfProgramCr();
        var municipalityIdField = this.getMunicipalityTriggerField();
        var financeIdField = this.getFinSourcesField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            financeIds : (financeIdField ? financeIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});
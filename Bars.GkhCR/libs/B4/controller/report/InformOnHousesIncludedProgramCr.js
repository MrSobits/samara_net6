Ext.define('B4.controller.report.InformOnHousesIncludedProgramCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformOnHousesIncludedProgramCrPanel',
    mainViewSelector: '#informOnHousesIncludedProgramCrPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.PeriodForSelect',
        'dict.PeriodForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    municipalityTriggerFieldSelector: '#informOnHousesIncludedProgramCrPanel #tfMunicipality',
    periodFieldSelector: '#informOnHousesIncludedProgramCrPanel #tfPeriod',
    stateFieldSelector: '#informOnHousesIncludedProgramCrPanel #tfState',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'informOnHousesIncludedProgramCrMultiselectwindowaspect',
            fieldSelector: '#informOnHousesIncludedProgramCrPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informOnHousesIncludedProgramCrMunicipalitySelectWindow',
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
            name: 'informOnHousesIncludedProgramCrPeriodMultiselectwindowaspect',
            fieldSelector: '#informOnHousesIncludedProgramCrPanel #tfPeriod',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informOnHousesIncludedProgramCrPeriodSelectWindow',
            storeSelect: 'dict.PeriodForSelect',
            storeSelected: 'dict.PeriodForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }                
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
            name: 'informOnHousesIncludedProgramCrStateMultiselectwindowaspect',
            fieldSelector: '#informOnHousesIncludedProgramCrPanel #tfState',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informOnHousesIncludedProgramCrStateSelectWindow',
            storeSelect: 'dict.StateForSelect',
            storeSelected: 'dict.StateForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.typeId = 'cr_object';
            }
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var periodField = Ext.ComponentQuery.query(this.periodFieldSelector)[0];
        var stateField = Ext.ComponentQuery.query(this.stateFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            periodIds: (periodField ? periodField.getValue() : null),
            stateIds: (stateField? stateField.getValue() : null)
        };
    }
});
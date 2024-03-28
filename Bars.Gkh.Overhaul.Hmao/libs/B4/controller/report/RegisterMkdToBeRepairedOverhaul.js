Ext.define('B4.controller.report.RegisterMkdToBeRepairedOverhaul', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ListByManyApartmentsHousesOverhaulPanel',
    mainViewSelector: '#listByManyApartmentsHousesOverhaulPanel',

    views: [
        'report.ListByManyApartmentsHousesOverhaulPanel',
        'SelectWindow.MultiSelectWindow',
        'B4.ux.button.Update',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],
    
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#listByManyApartmentsHousesOverhaulPanel #tfMunicipality'
        },
        {
            ref: 'StartYearField',
            selector: '#listByManyApartmentsHousesOverhaulPanel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: '#listByManyApartmentsHousesOverhaulPanel [name=EndYear]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'listByManyApartmentsHousesOverhaulMultiselectwindowaspect',
            fieldSelector: '#listByManyApartmentsHousesOverhaulPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#listByManyApartmentsHousesOverhaulPanelMunicipalitySelectWindow',
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
                operation.params.levelMun = 1;
            }
        }
    ],

    getParams: function () {
        var startYear = this.getStartYearField();
        var endYear = this.getEndYearField();
        var municipalityIdField = this.getMunicipalityTriggerField();

        return {
            startYear: (startYear ? startYear.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            endYear: (endYear ? endYear.getValue() : null)
        };
    }
});
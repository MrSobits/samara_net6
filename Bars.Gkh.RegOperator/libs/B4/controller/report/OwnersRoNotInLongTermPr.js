Ext.define('B4.controller.report.OwnersRoNotInLongTermPr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OwnersRoNotInLongTermPrPanel',
    mainViewSelector: '#ownersRoNotInLongTermPrPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected' 
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    parentMuTriggerFieldSelector: '#ownersRoNotInLongTermPrPanel #tfParentMu',
    municipalityTriggerFieldSelector: '#ownersRoNotInLongTermPrPanel #tfMunicipality',
    actualityDateFieldSelector: '#ownersRoNotInLongTermPrPanel #dfActualityDate',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'ParentMuMultiselectwindowaspect',
            fieldSelector: '#ownersRoNotInLongTermPrPanel #tfParentMu',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ownersRoNotInLongTermPrPanelParentMuSelectWindow',
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
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'MuMultiselectwindowaspect',
            fieldSelector: '#ownersRoNotInLongTermPrPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ownersRoNotInLongTermPrPanelMunicipalitySelectWindow',
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
            onBeforeLoad: function (store, operation) {
                var parentMuField = Ext.ComponentQuery.query('#ownersRoNotInLongTermPrPanel #tfParentMu')[0];
                operation.params.parentMuIds = parentMuField.getValue();
            },
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],
    
    validateParams: function () {
        var municipalities = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var parentMu = Ext.ComponentQuery.query(this.parentMuTriggerFieldSelector)[0];
        var actualityDate = Ext.ComponentQuery.query(this.actualityDateFieldSelector)[0];
        
        return (municipalities && municipalities.isValid() && actualityDate && actualityDate.isValid());
        
    },

    getParams: function () {
        var municipalityIdField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var parentMuTriggerField = Ext.ComponentQuery.query(this.parentMuTriggerFieldSelector)[0];
        var actualityDateField = Ext.ComponentQuery.query(this.actualityDateFieldSelector)[0];

        return {
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            parentMoIds: (parentMuTriggerField ? parentMuTriggerField.getValue() : null),
            actualityDate: (actualityDateField ? actualityDateField.getValue() : null)
        };
    }
});
Ext.define('B4.controller.report.RoomAreaControl', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RoomAreaControlPanel',
    mainViewSelector: '#reportRoomAreaControlPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.enums.ConditionHouse',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.ConditionHouseForSelect',
        'dict.ConditionHouseForSelected',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.TypeOwnershipForSelect',
        'dict.TypeOwnershipForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'report.RoomAreaControlPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    municipalityTriggerFieldSelector: '#reportRoomAreaControlPanel #tfMunicipality',
    conditionHouseTriggerFieldSelector: '#reportRoomAreaControlPanel #tfConditionHouse',
    typeOwnershipTriggerFieldSelector: '#reportRoomAreaControlPanel #tfTypeOwnership',
    collectBySelector: '#reportRoomAreaControlPanel #cbCollectBy',


    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportRoomAreaControlPanelMultiselectwindowaspect',
            fieldSelector: '#reportRoomAreaControlPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportRoomAreaControlPanelMunicipalitySelectWindow',
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
            name: 'reportRoomAreaControlPanelConditionHouseMultiselectwin',
            fieldSelector: '#reportRoomAreaControlPanel #tfConditionHouse',
            valueProperty: 'Value',
            textProperty: 'Display',
            idProperty: 'Value',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportRoomAreaControlPanelConditionHouseSelectWindow',
            storeSelect: 'dict.ConditionHouseForSelect',
            storeSelected: 'dict.ConditionHouseForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportRoomAreaControlPanelTypeOwnershipMultiselectwindowaspect',
            fieldSelector: '#reportRoomAreaControlPanel #tfTypeOwnership',
            valueProperty: 'Id',
            textProperty: 'Name',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportRoomAreaControlPanelTypeOwnershipSelectWindow',
            storeSelect: 'dict.TypeOwnershipForSelect',
            storeSelected: 'dict.TypeOwnershipForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для выбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    getParams: function () {
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var conditionHouseField = Ext.ComponentQuery.query(this.conditionHouseTriggerFieldSelector)[0];
        var typeOwnershipField = Ext.ComponentQuery.query(this.typeOwnershipTriggerFieldSelector)[0];
        var clByField = Ext.ComponentQuery.query(this.collectBySelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            conditonHouseIds: (conditionHouseField ? conditionHouseField.getValue() : null),
            typeOwnershipIds: (typeOwnershipField ? typeOwnershipField.getValue() : null),
            collectBy: (clByField ? clByField.getValue() : null)
        };
    }
});
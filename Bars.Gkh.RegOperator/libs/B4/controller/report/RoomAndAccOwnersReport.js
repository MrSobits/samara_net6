Ext.define('B4.controller.report.RoomAndAccOwnersReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RoomAndAccOwnersReportPanel',
    mainViewSelector: 'roomandaccownersreportpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update',
        'dict.HouseTypeForSelect',
        'dict.HouseTypeForSelected',
        'dict.ConditionHouseForSelect',
        'dict.ConditionHouseForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        { ref: 'municipalityTriggerField', selector: 'roomandaccownersreportpanel [name=Municipalities]' },
        { ref: 'houseTypeTriggerField', selector: 'roomandaccownersreportpanel [name=HouseTypes]' },
        { ref: 'houseConditionTriggerField', selector: 'roomandaccownersreportpanel [name=HouseConditions]' }

 
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'roomAndAccOwnReportMuMultiselectwinaspect',
            fieldSelector: 'roomandaccownersreportpanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#roomAndAccOwnReportMuSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
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
            name: 'roomAndAccOwnReportHouseTypeMultiselectwinaspect',
            fieldSelector: 'roomandaccownersreportpanel [name=HouseTypes]',
            valueProperty: 'Value',
            textProperty: 'Display',
            idProperty: 'Value',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#roomAndAccOwnReportHouseTypeSelectWindow',
            storeSelect: 'dict.HouseTypeForSelect',
            storeSelected: 'dict.HouseTypeForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'roomAndAccOwnReportHouseConditionMultiselectwindowaspect',
            fieldSelector: 'roomandaccownersreportpanel [name=HouseConditions]',
            valueProperty: 'Value',
            textProperty: 'Display',
            idProperty: 'Value',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#roomAndAccOwnReportHouseConditionSelectWindow',
            storeSelect: 'dict.ConditionHouseForSelect',
            storeSelected: 'dict.ConditionHouseForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this,
            mcpField = me.getMunicipalityTriggerField(),
            houseTypeField = me.getHouseTypeTriggerField(),
            houseConditionField = me.getHouseConditionTriggerField();


        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            houseCondition: (houseConditionField ? houseConditionField.getValue() : null)
        };

    }
});
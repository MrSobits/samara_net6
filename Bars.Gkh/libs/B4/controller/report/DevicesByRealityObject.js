Ext.define('B4.controller.report.DevicesByRealityObject', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DevicesByRealityObjectPanel',
    mainViewSelector: '#devicesByRealityObjectPanel',

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

    refs: [
        {
            
            ref: 'MunicipalityTriggerField',
            selector: '#devicesByRealityObjectPanel #tfMunicipality'
        },
        {
            ref: 'TypeDeviceField',
            selector: '#devicesByRealityObjectPanel #sfTypeDevice'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'devicesByRealityObjectMunMultiselectwindowaspect',
            fieldSelector: '#devicesByRealityObjectPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#devicesByRealityObjectPanelMunicipalitySelectWindow',
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
        }
    ],

    validateParams: function () {
        var typeDev = this.getTypeDeviceField();
        if (!typeDev.isValid()) {
            return "Не указан параметр \"Тип прибора\"";
        }

        return true;
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var typeDev = this.getTypeDeviceField();

        return {
            municipality: (mcpField ? mcpField.getValue() : null),
            typedevice: (typeDev ? typeDev.getValue() : null)
        };
    }
});
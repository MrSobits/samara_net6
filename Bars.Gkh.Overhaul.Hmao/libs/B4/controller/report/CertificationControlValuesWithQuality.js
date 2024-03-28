Ext.define('B4.controller.report.CertificationControlValuesWithQuality', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CertificationControlValuesPanel',
    mainViewSelector: 'reportCertificationControlValuesPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.ConditionHouseForSelect',
        'dict.ConditionHouseForSelected',
        'dict.HouseTypeForSelect',
        'dict.HouseTypeForSelected',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'reportCertificationControlValuesPanel #tfMunicipality'
        },
        {
            ref: 'HouseTypeTriggerField',
            selector: 'reportCertificationControlValuesPanel #tfHouseType'
        },
        {
            ref: 'ConditionHouseTriggerField',
            selector: 'reportCertificationControlValuesPanel #tfConditionHouse'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportCertificationControlValuesPanelMunicipalityMultiselectwindowaspect',
            fieldSelector: 'reportCertificationControlValuesPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportCertificationControlValuesPanelMunicipalitySelectWindow',
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
            name: 'reportCertificationControlValuesPanelHouseTypeMultiselectwindowaspect',
            fieldSelector: 'reportCertificationControlValuesPanel #tfHouseType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportCertificationControlValuesPanelPanelHouseTypeSelectWindow',
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
            name: 'reportCertCtrlValuesPanelConditionHouseMultiselectwin',
            fieldSelector: 'reportCertificationControlValuesPanel #tfConditionHouse',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportCertCtrlValPanelConditionHouseSelectWindow',
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
        var muId = this.getMunicipalityTriggerField();
        return muId && muId.isValid();
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField();
        var houseTypeField = this.getHouseTypeTriggerField();
        var conditionHouseField = this.getConditionHouseTriggerField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            conditionHouses: (conditionHouseField ? conditionHouseField.getValue() : null)
        };
    }
});
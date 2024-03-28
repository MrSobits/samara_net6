Ext.define('B4.controller.report.ControlCertificationOfBuild', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ControlCertificationOfBuildPanel',
    mainViewSelector: '#reportControlCertificationOfBuildPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.HouseTypeForSelect',
        'dict.HouseTypeForSelected',
        'dict.municipality.ByParam'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#reportControlCertificationOfBuildPanel #tfMunicipality'
        },

        {
            ref: 'HouseTypeTriggerField',
            selector: '#reportControlCertificationOfBuildPanel #tfHouseType'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportControlCertificationOfBuildPanelMultiselectwindowaspect',
            fieldSelector: '#reportControlCertificationOfBuildPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportControlCertificationOfBuildPanelMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        store: Ext.create('B4.store.dict.municipality.ByParam'),
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' }
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
            name: 'reportControlCertificationOfBuildPanelHouseTypeMultiselectwindowaspect',
            fieldSelector: '#reportControlCertificationOfBuildPanel #tfHouseType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportControlCertificationOfBuildPanelHouseTypeSelectWindow',
            storeSelect: 'dict.HouseTypeForSelect',
            storeSelected: 'dict.HouseTypeForSelected',
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
        var mcpField = this.getMunicipalityTriggerField();
        var houseTypeField = this.getHouseTypeTriggerField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null)
        };
    }
});
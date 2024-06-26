﻿Ext.define('B4.controller.report.HouseInformationCeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.HouseInformationCeReport',
    mainViewSelector: '#reportHouseInformationCePanel',

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
        'CommonEstateObjectForSelect',
        'CommonEstateObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#reportHouseInformationCePanel #tfMunicipality'
        },
        {
            ref: 'HouseTypeTriggerField',
            selector: '#reportHouseInformationCePanel #tfHouseType'
        },
        {
            ref: 'CeoTriggerField',
            selector: '#reportHouseInformationCePanel #tfCeo'
        },
        {
            ref: 'HousesListEnumField',
            selector: '#reportHouseInformationCePanel #housesList'
        },
        {
            ref: 'cbMainField',
            selector: '#reportHouseInformationCePanel #cbMain'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportHouseInformationCePanelMultiselectwindowaspect',
            fieldSelector: '#reportHouseInformationCePanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportHouseInformationCePanelMunicipalitySelectWindow',
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
            name: 'reportHouseInformationCePanelHouseTypeMultiselectwindowaspect',
            fieldSelector: '#reportHouseInformationCePanel #tfHouseType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportHouseInformationCePanelHouseTypeSelectWindow',
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
            name: 'reportHouseInformationCePanelCeoMultiselectwindowaspect',
            fieldSelector: '#reportHouseInformationCePanel #tfCeo',
            valueProperty: 'Id',
            textProperty: 'Name',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportHouseInformationCePanelCeoSelectWindow',
            storeSelect: 'CommonEstateObjectForSelect',
            storeSelected: 'CommonEstateObjectForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this;
        var mcpField = me.getMunicipalityTriggerField();
        var houseTypeField = me.getHouseTypeTriggerField();
        var ceoField = me.getCeoTriggerField();
        var enumHousesField = me.getHousesListEnumField(); 
        var cbMainField = me.getCbMainField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            ceoIds: (ceoField ? ceoField.getValue() : null),
            housesList: (enumHousesField ? enumHousesField.getValue() : null),
            cbMain: (cbMainField ? cbMainField.getValue() : null)
        };
    }
});
﻿Ext.define('B4.controller.report.FormFundNotSetMkdInfo', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.FormFundNotSetMkdInfoPanel',
    mainViewSelector: '#reportFormFundNotSetMkdInfoPanel',

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
        'dict.municipality.ByParam',
        'dict.HouseTypeForSelect',
        'dict.HouseTypeForSelected',
        'dict.ConditionHouseForSelect',
        'dict.ConditionHouseForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        { ref: 'municipalityTriggerField', selector: '#reportFormFundNotSetMkdInfoPanel #tfMunicipality' },
        { ref: 'dateTimeReportField', selector: '#reportFormFundNotSetMkdInfoPanel datefield[name="DateTimeReport"]' },
        { ref: 'HouseTypeTriggerField', selector: '#reportFormFundNotSetMkdInfoPanel #tfHouseType' },
        { ref: 'HouseConditionTriggerField', selector: '#reportFormFundNotSetMkdInfoPanel #tfHouseCondition' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportFormFundNotSetMkdInfoPanelMultiselectwindowaspect',
            fieldSelector: '#reportFormFundNotSetMkdInfoPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportFormFundNotSetMkdInfoPanelMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ByParam',
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
            name: 'reportFormFundNotSetMkdInfoPanelHouseTypeMultiselectwindowaspect',
            fieldSelector: '#reportFormFundNotSetMkdInfoPanel #tfHouseType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportFormFundNotSetMkdInfoPanelHouseTypeSelectWindow',
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
            name: 'reportFormFundNotSetMkdInfoPanelHouseConditionMultiselectwindowaspect',
            fieldSelector: '#reportFormFundNotSetMkdInfoPanel #tfHouseCondition',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportFormFundNotSetMkdInfoPanelHouseConditionSelectWindow',
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
    
    onLaunch: function () {

        var tfHouseType = Ext.ComponentQuery.query('#reportFormFundNotSetMkdInfoPanel #tfHouseType');
        var tfHouseCondition = Ext.ComponentQuery.query('#reportFormFundNotSetMkdInfoPanel #tfHouseCondition');

        if (tfHouseType.length > 0) {
            tfHouseType[0].setValue(30);
            tfHouseType[0].updateDisplayedText('Многоквартирный');
        }

        if (tfHouseCondition.length > 0) {
            tfHouseCondition[0].setValue('30, 20');
            tfHouseCondition[0].updateDisplayedText('Исправный, Ветхий');
        }
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField(),
            dateTimeReportField = this.getDateTimeReportField();
        var houseTypeField = this.getHouseTypeTriggerField();
        var houseConditionField = this.getHouseConditionTriggerField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateTimeReport: (dateTimeReportField ? dateTimeReportField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            houseCondition: (houseConditionField ? houseConditionField.getValue() : null)
        };
    }
});
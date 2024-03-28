Ext.define('B4.controller.report.FormFundNotSetMkdInfo', {
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
        { ref: 'HouseConditionTriggerField', selector: '#reportFormFundNotSetMkdInfoPanel #tfHouseCondition' },
        { ref: 'WithoutPhysicalWearingField', selector: '#reportFormFundNotSetMkdInfoPanel #cbWithoutPhysicalWearing' }
 
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportFormFundNotSetMkdInfoPanelMultiselectwindowaspect',
            fieldSelector: '#reportFormFundNotSetMkdInfoPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportFormFundNotSetMkdInfoPanelMunicipalitySelectWindow',
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

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField(),
            dateTimeReportField = this.getDateTimeReportField();
        var houseTypeField = this.getHouseTypeTriggerField();
        var houseConditionField = this.getHouseConditionTriggerField();
        var wihoutWearing = this.getWithoutPhysicalWearingField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateTimeReport: (dateTimeReportField ? dateTimeReportField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            houseCondition: (houseConditionField ? houseConditionField.getValue() : null),
            withoutPhysicalWear : (wihoutWearing ? wihoutWearing.getValue() : null)
        };

    }
});
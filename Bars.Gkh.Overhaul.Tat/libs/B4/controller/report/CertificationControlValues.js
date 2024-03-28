Ext.define('B4.controller.report.CertificationControlValues', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CertificationControlValuesPanel',
    mainViewSelector: 'reportCertificationControlValuesPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse',
        'B4.ux.button.Update'
    ],

    stores: [
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
            ref: 'EmergencyField',
            selector: 'reportCertificationControlValuesPanel #cbEmergency'
        },
        {
            ref: 'DilapidatedField',
            selector: 'reportCertificationControlValuesPanel #cbDilapidated'
        },
        {
            ref: 'ServiceableField',
            selector: 'reportCertificationControlValuesPanel #cbServiceable'
        },
        {
            ref: 'RazedField',
            selector: 'reportCertificationControlValuesPanel #cbRazed'
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
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.levelMun = 1;
            }
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
        }
    ],

    validateParams: function () {
        var muId = this.getMunicipalityTriggerField();
        
        var emergencyField = this.getEmergencyField(),
           dilapidatedField = this.getDilapidatedField(),
           serviceableField = this.getServiceableField(),
           razedField = this.getRazedField();

        if (!emergencyField.getValue() && !dilapidatedField.getValue() && !serviceableField.getValue() && !razedField.getValue()) {
            return 'Необходимо выбрать хотябы один тип дома';
        }

        return muId && muId.isValid();
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField();
        var houseTypeField = this.getHouseTypeTriggerField();
        
        var emergencyField = this.getEmergencyField(),
            dilapidatedField = this.getDilapidatedField(),
            serviceableField = this.getServiceableField(),
            razedField = this.getRazedField();
        
        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            houseTypes: (houseTypeField ? houseTypeField.getValue() : null),
            emergency: (emergencyField ? emergencyField.getValue() : null),
            dilapidated: (dilapidatedField ? dilapidatedField.getValue() : null),
            serviceable: (serviceableField ? serviceableField.getValue() : null),
            razed: (razedField ? razedField.getValue() : null)
        };
    }
});
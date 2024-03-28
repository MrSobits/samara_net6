Ext.define('B4.controller.report.ChangeCommunalServicesTariff', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ChangeCommunalServicesTariffPanel',
    mainViewSelector: '#changeCommunalServicesTariffPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    startDateFieldSelector: '#changeCommunalServicesTariffPanel #dfStartDate',
    endDateFieldSelector: '#changeCommunalServicesTariffPanel #dfEndDate',
    municipalityTriggerFieldSelector: '#changeCommunalServicesTariffPanel #tfMunicipality',
    serviceTypeFieldSelector: '#changeCommunalServicesTariffPanel #tfServiceType',
    transmitOrgFieldSelector: '#changeCommunalServicesTariffPanel #cbTransmitOrg',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'changeCommunalServicesTariffMultiselectwindowaspect',
            fieldSelector: '#changeCommunalServicesTariffPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#changeCommunalServicesTariffMunicipalitySelectWindow',
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
                }
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
            name: 'changeCommunalServicesTariffServiceTypeMultiselectwindowaspect',
            fieldSelector: '#changeCommunalServicesTariffPanel #tfServiceType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#changeCommunalServicesTariffServiceTypeSelectWindow',
            storeSelect: 'dict.ServiceTypeForSelect',
            storeSelected: 'dict.ServiceTypeForSelected',
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

        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var startDate = Ext.ComponentQuery.query(this.startDateFieldSelector)[0];
        var endDate = Ext.ComponentQuery.query(this.endDateFieldSelector)[0];
        var serviceType = Ext.ComponentQuery.query(this.serviceTypeFieldSelector)[0];
        var transmitOrg = Ext.ComponentQuery.query(this.transmitOrgFieldSelector)[0];
             

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            startDate: (startDate ? startDate.getValue() : null),
            endDate: (endDate ? endDate.getValue() : null),
            serviceType: (serviceType ? serviceType.getValue() : null),
            transmitOrg: (transmitOrg ? transmitOrg.getValue() : null)
        };
    }
});
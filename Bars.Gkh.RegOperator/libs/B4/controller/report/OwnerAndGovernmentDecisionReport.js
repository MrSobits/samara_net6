Ext.define('B4.controller.report.OwnerAndGovernmentDecisionReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OwnerAndGovernmentDecisionReportPanel',
    mainViewSelector: 'ownerandgovernmentdecisionreportpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.municipality.ByParamPaging',
        'dict.MunicipalityForSelect',
        'RealtyObjectByMo',
        'RealtyObjectByMoSelected'
        
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'ownerandgovernmentdecisionreportpanel #tfMunicipality'
        },
        {
            ref: 'AddressSelectField',
            selector: 'ownerandgovernmentdecisionreportpanel #tfAddress'
        },
        {
            ref: 'DecisionTypeField',
            selector: 'ownerandgovernmentdecisionreportpanel b4combobox'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'municipalityMultiselectwindowaspect',
        fieldSelector: 'ownerandgovernmentdecisionreportpanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#ownerandgovernmentdecisionreportpanelMunicipalitySelectWindow',
        storeSelect: 'dict.municipality.ByParamPaging',
        storeSelected: 'dict.MunicipalityForSelect',
        columnsGridSelect: [
            {
                header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 2,
                filter: {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    storeAutoLoad: false,
                    hideLabel: true,
                    editable: false,
                    valueField: 'Name',
                    emptyItem: { Name: '-' },
                    url: '/Municipality/ListByParam'
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
        listeners: {
            getdata: function () {
                Ext.ComponentQuery.query('ownerandgovernmentdecisionreportpanel #tfAddress')[0].onTrigger2Click();
            }
        }
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'AddressMultiselectwindowaspect',
        fieldSelector: 'ownerandgovernmentdecisionreportpanel #tfAddress',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#ownerandgovernmentdecisionreportpanelAddressSelectWindow',
        storeSelect: 'RealtyObjectByMo',
        storeSelected: 'RealtyObjectByMoSelected',
        columnsGridSelect: [
            { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Name', filter: { xtype: 'textfield' } }
        ],
        onBeforeLoad: function (store, operation) {
            var municipalityField = this.controller.getMunicipalitySelectField();
            operation.params.settlementId = municipalityField.getValue();
            if (!operation.params.settlementId) {
                operation.params.settlementId = 0;
            }
        },
        columnsGridSelected: [
            { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
        ],
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
        var municipality = me.getMunicipalitySelectField();
        var address = me.getAddressSelectField();
        var decType = me.getDecisionTypeField();

        return {
            muIds: (municipality ? municipality.getValue() : null),
            roIds: (address ? address.getValue() : null),
            decisionType: decType.getValue()
        };
    }
});
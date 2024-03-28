Ext.define('B4.controller.report.PersonalAccountChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PersonalAccountChargeReportPanel',
    mainViewSelector: 'personalAccountChargeReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'RealtyObjectByMo'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'personalAccountChargeReportPanel #tfParentMu'
        },
        {
            ref: 'SettlementSelectField',
            selector: 'personalAccountChargeReportPanel #tfMunicipality'
        },
        {
            ref: 'AddressSelectField',
            selector: 'personalAccountChargeReportPanel #tfAddress'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'ParentMuMultiselectwindowaspect',
        fieldSelector: 'personalAccountChargeReportPanel #tfParentMu',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#personalAccountChargeReportPanelParentMuSelectWindow',
        storeSelect: 'dict.municipality.MoArea',
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
        listeners: {
            getdata: function () {
                Ext.ComponentQuery.query('personalAccountChargeReportPanel #tfMunicipality')[0].onTrigger2Click();
            }
        }
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'MuMultiselectwindowaspect',
        fieldSelector: 'personalAccountChargeReportPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#personalAccountChargeReportPanelMunicipalitySelectWindow',
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
        onBeforeLoad: function (store, operation) {
            var parentMuField = this.controller.getMunicipalitySelectField();
            operation.params.parentMuIds = parentMuField.getValue();
            if (!operation.params.parentMuIds) {
                operation.params.parentMuIds = 0;
            }
        },
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
        ],
        titleSelectWindow: 'Выбор записи',
        titleGridSelect: 'Записи для отбора',
        titleGridSelected: 'Выбранная запись'
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'AddressMultiselectwindowaspect',
        fieldSelector: 'personalAccountChargeReportPanel #tfAddress',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#personalAccountChargeReportPanelAddressSelectWindow',
        storeSelect: 'RealtyObjectByMo',
        storeSelected: 'RealtyObjectByMo',
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name',  filter: { xtype: 'textfield' }, flex: 1 }
        ],
        onBeforeLoad: function (store, operation) {
            var settlementField = this.controller.getSettlementSelectField();
            operation.params.isDecisionRegOp = true;
            operation.params.settlementId = settlementField.getValue();
            if (!operation.params.settlementId) {
                operation.params.settlementId = 0;
            }
        },
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
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
        var municipalityR = this.getMunicipalitySelectField();
        var municipalityO = this.getSettlementSelectField();
        var address = this.getAddressSelectField();

        return {
            mrIds: (municipalityR ? municipalityR.getValue() : null),
            moIds: (municipalityO ? municipalityO.getValue() : null),
            roIds: (address ? address.getValue() : null)
        };
    }
});
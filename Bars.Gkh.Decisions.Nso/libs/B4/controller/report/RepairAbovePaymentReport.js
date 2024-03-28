Ext.define('B4.controller.report.RepairAbovePaymentReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RepairAbovePaymentReportPanel',
    mainViewSelector: '#repairAboveReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'manorg.ForSelect',
        'manorg.ForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'DateField',
            selector: '#repairAboveReportPanel #dfDate'
        },
        {
            ref: 'MunicipalitySelectField',
            selector: '#repairAboveReportPanel #tfParentMu'
        },
        {
            ref: 'SettlementSelectField',
            selector: '#repairAboveReportPanel #tfMunicipality'
        },
        {
            ref: 'CrFundFormationDecisionTypeField',
            selector: '#repairAboveReportPanel #cbCrFund'
        },
        {
            ref: 'FondSizeField',
            selector: '#repairAboveReportPanel #cbFondSize'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'ParentMuMultiselectwindowaspect',
        fieldSelector: '#repairAboveReportPanel #tfParentMu',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#ownersRoInLongTermPrPanelParentMuSelectWindow',
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
                Ext.ComponentQuery.query('#repairAboveReportPanel #tfMunicipality')[0].onTrigger2Click();
            }
        }
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'MuMultiselectwindowaspect',
        fieldSelector: '#repairAboveReportPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#ownersRoInLongTermPrPanelMunicipalitySelectWindow',
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
    }
    ],

    validateParams: function () {

        var dateField = this.getDateField().getValue();

        if (dateField == null || dateField == Date.min) {
            return "Не указан параметр \"По состоянию на\"";
        }

        if (this.getMunicipalitySelectField().getValue() == null) {
            return "Не указан параметр \"Муниципальное образование\"";
        }

        return true;
    },

    getParams: function () {

        var date = this.getDateField().getValue();
        var ms = this.getMunicipalitySelectField().getValue();
        var settlments = this.getSettlementSelectField().getValue();
        var cr = this.getCrFundFormationDecisionTypeField().getValue();
        var equalsMin = this.getFondSizeField().getValue();

        return {
            date: date,
            msIds: ms,
            settlIds: settlments,
            methodCr: cr,
            equalsMin: equalsMin
        };
    }
});
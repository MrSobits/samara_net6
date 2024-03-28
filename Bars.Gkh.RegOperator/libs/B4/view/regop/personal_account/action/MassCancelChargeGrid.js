Ext.define('B4.view.regop.personal_account.action.MassCancelChargeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.masscancelchargegrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store'
    ],

    cls: 'x-large-head',

    accountOperationCode: null,

    initComponent: function () {
        var me = this,
            massCancelChargeInfoStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasePersonalAccount',
                    listAction: 'GetOperationDataForUI',
                    timeout: 60 * 1000, // 1 минута
                    extraParams: {
                        operationCode: me.accountOperationCode
                    },
                    actionMethods: {
                        read: 'POST'
                    }
                },
                fields: [
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'RoomAddress' },
                    { name: 'PersonalAccountNum' },

                    { name: 'CancelBaseTariffSum' },
                    { name: 'CancelDecisionTariffSum' },
                    { name: 'CancelPenaltySum' }
                ]
            });

        Ext.applyIf(me, {
            store: massCancelChargeInfoStore,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 0.7,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    width: 100,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Отменить начисления по базовому тарифу в размере',
                    dataIndex: 'CancelBaseTariffSum',
                    width: 100,
                    decimalSeparator: ',',
                    sortable: false,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Отменить начисления по тарифу решения в размере',
                    dataIndex: 'CancelDecisionTariffSum',
                    width: 100,
                    decimalSeparator: ',',
                    sortable: false,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Отменить начисления по пени в размере',
                    dataIndex: 'CancelPenaltySum',
                    width: 100,
                    decimalSeparator: ',',
                    sortable: false,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                }              
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            viewConfig: {
                loadMask: true
            },

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: massCancelChargeInfoStore,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
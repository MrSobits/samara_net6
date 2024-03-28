Ext.define('B4.view.regop.personal_account.action.MassSaldoChangeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.masssaldochangegrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store'
    ],

    cls: 'x-large-head',

    accountData: null,

    initComponent: function () {
        var me = this,
            massChangeSaldoStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                data: me.accountData,
                proxy: {
                    type: 'memory'
                },
                fields: [
                    { name: 'Id' },
                    { name: 'AccountNumber' },
                    { name: 'Municipality' },
                    { name: 'Address' },
                    { name: 'AccountNumber' },
                    { name: 'SaldoByBaseTariff' },
                    { name: 'NewSaldoByBaseTariff' },
                    { name: 'SaldoByDecisionTariff' },
                    { name: 'NewSaldoByDecisionTariff' },
                    { name: 'SaldoByPenalty' },
                    { name: 'NewSaldoByPenalty' },
                    { name: 'Saldo' },
                    { name: 'NewSaldo' }
                ]
            });

        Ext.applyIf(me, {
            store: massChangeSaldoStore,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1.5,
                    sortable: false,
                    text: 'Муниципальный район'
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    sortable: false,
                    flex: 2
                },
                {
                    text: 'Номер ЛС',
                    dataIndex: 'AccountNumber',
                    sortable: false,
                    width: 70
                },
                {
                    text: 'Сальдо по базовому тарифу',
                    dataIndex: 'SaldoByBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    readOnly: true,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Новое сальдо по базовому тарифу',
                    dataIndex: 'NewSaldoByBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        allowBlank: false
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Сальдо по тарифу решения',
                    dataIndex: 'SaldoByDecisionTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    readOnly: true,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Новое сальдо по тарифу решения',
                    dataIndex: 'NewSaldoByDecisionTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        allowBlank: false
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Сальдо по пени',
                    dataIndex: 'SaldoByPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    readOnly: true,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Новое сальдо по пени',
                    dataIndex: 'NewSaldoByPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        allowBlank: false
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    dataIndex: 'Saldo',
                    text: 'Текущее сальдо',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    readOnly: true,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    dataIndex: 'NewSaldo',
                    text: 'Новое сальдо',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    readOnly: true,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                }
            ],

            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEdit'
                })
            ],

            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
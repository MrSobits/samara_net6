Ext.define('B4.view.transferctr.PaymentDetailGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.transferctr.PaymentDetail'
    ],

    alias: 'widget.transferctrpaydetailgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.transferctr.PaymentDetail');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            emptyText: 'У дома нет доступных средств',
            columns: [
                {
                    dataIndex: 'WalletName',
                    text: 'Источник поступления',
                    flex: 1,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    dataIndex: 'Balance',
                    text: 'Сальдо, руб.',
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    xtype: 'numbercolumn',
                    format: '0.00',
                    width: 100
                },
                {
                    dataIndex: 'Amount',
                    text: 'Оплата, руб.',
                    xtype: 'numbercolumn',
                    format: '0.00',
                    width: 100,
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    editor: {
                        xtype: 'numberfield',
                        minValue: 0,
                        decimalSeparator: ','
                    }
                },
                {
                    dataIndex: 'RefundSum',
                    text: 'Возврат, руб.',
                    width: 100,
                    xtype: 'numbercolumn',
                    format: '0.00',
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            features: [
                {
                    ftype: 'summary'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            me.getStore().load();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
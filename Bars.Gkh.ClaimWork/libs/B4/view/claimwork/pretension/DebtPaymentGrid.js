Ext.define('B4.view.claimwork.pretension.DebtPaymentGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',
        'B4.store.claimwork.pretension.DebtPayment'
    ],

    alias: 'widget.pretensiondebtpaymentgrid',
    closable: false,
    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.pretension.DebtPayment');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PaymentDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Дата оплаты',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'PaymentType',
                    flex: 2,
                    text: 'Тип платежа',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
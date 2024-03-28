Ext.define('B4.view.transitaccount.DebetGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.transitaccount.ControlDebet'
    ],

    title: 'Дебет транзитного счета',
    alias: 'widget.transitaccountdebetgrid',
    closable: false,
    cls:'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.transitaccount.ControlDebet');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    dataIndex: 'Number',
                    width: 100,
                    text: '№ реестра',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата реестра',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'PaymentAgentName',
                    flex: 1,
                    text: 'Платежный агент',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Sum',
                    width: 150,
                    text: 'Сумма по реестру',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
                    }
                },
                {
                    dataIndex: 'ConfirmSum',
                    width: 150,
                    text: 'Подтвержденная сумма',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
                    }
                },
                {
                    dataIndex: 'Divergence',
                    width: 150,
                    text: 'Расхождение',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
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
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    labelWidth: 200,
                                    width: 300,
                                    name: 'UnallocatedBalance',
                                    labelAlign: 'right',
                                    fieldLabel: 'Нераспределенный остаток',
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать дебет',
                                    action: 'MakeDebet'
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
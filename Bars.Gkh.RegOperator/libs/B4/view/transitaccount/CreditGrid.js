Ext.define('B4.view.transitaccount.CreditGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.transitaccount.ControlCredit'
    ],

    title: 'Кредит транзитного счета',
    alias: 'widget.transitaccountcreditgrid',
    closable: false,
    cls:'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.transitaccount.ControlCredit');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '№',
                    width: 50
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'CreditOrgName',
                    flex: 1,
                    text: 'Кредитная организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'CalcAccount',
                    width: 170,
                    text: '№ р/с',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Sum',
                    width: 150,
                    text: 'Сумма по п/п',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'ConfirmSum',
                    width: 150,
                    text: 'Подтвержденная сумма',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ','
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'Divergence',
                    width: 150,
                    text: 'Расхождение',
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
                                },
                                {
                                    xtype: 'button',
                                    text: 'Выгрузка в 1С',
                                    action: 'Export'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать кредит',
                                    action: 'MakeCredit'
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
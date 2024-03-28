Ext.define('B4.view.claimwork.DebtorPaymentsWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.claimworkoperationwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.base.Store'
    ],

    modal: true,
    width: 700,
    height: 350,
    title: 'Операции за период',
    layout: 'fit',
   
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Name' },
                    { name: 'Amount' },
                    { name: 'Period' },
                    { name: 'TransferId' },
                    { name: 'PaymentSource' },
                    { name: 'Date' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'Debtor',
                    listAction: 'ListOperationDetails',
                    timeout: 9999999
                },
                autoLoad: false
            });

        Ext.applyIf(me, {
            items: [{                
                xtype: 'gridpanel',
                border: false,
                header: false,
                store: store,
                plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
                columnLines: true,
                enableColumnHide: false,
                columns: [
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'Date',
                        format: 'd.m.Y',
                        text: 'Дата оплаты',
                        flex: 1,
                        filter: {
                            xtype: 'datefield',
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        text: 'Операция',
                        dataIndex: 'Name',
                        flex: 2,
                        filter: {
                            xtype: 'textfield',
                            operand: CondExpr.operands.icontains
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Сумма',
                        dataIndex: 'Amount',
                        format: '0.00',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        text: 'Назначение платежа',
                        dataIndex: 'PaymentSource',
                        flex: 2,
                        filter: {
                            xtype: 'textfield',
                            operand: CondExpr.operands.icontains
                        }
                    },
                    //{
                    //    xtype: 'actioncolumn',
                    //    text: 'Документ',
                    //    scope: me,
                    //    width: 60,
                    //    icon: 'content/img/icons/page_copy.png',
                    //    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                    //        gridView.ownerCt.fireEvent('rowaction', gridView.ownerCt, 'edit', rec);
                    //    }
                    //}
                ]
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            action: 'Accept',
                            text: 'Применить',
                            tooltip: 'Применить',
                            iconCls: 'icon-accept'
                        },
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ],
            viewConfig: {
                enableTextSelection: true
            }

        });
        me.callParent(arguments);
    }
});
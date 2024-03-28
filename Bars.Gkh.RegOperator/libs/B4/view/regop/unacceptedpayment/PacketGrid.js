Ext.define('B4.view.regop.unacceptedpayment.PacketGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.store.regop.UnacceptedPaymentPacket',
        'B4.enums.PaymentOrChargePacketState'
    ],

    title: 'Неподтвержденные оплаты',

    alias: 'widget.unacceptedpaymentpacketgrid',

    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.UnacceptedPaymentPacket'),
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            });

        Ext.apply(me, {
            selModel: selModel,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    text: 'Описание',
                    dataIndex: 'Description',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата формирования',
                    dataIndex: 'CreateDate',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        decimalSeparator: ','
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PaymentOrChargePacketState',
                    text: 'Состояние',
                    dataIndex: 'State',
                    flex: 1,
                    filter: true
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Действия',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Accept',
                                    tooltip: 'По нажатию произойдет подтверждение начислений выбранной записи',
                                    text: 'Подтвердить',
                                    iconCls: 'icon-tick'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Cancel',
                                    iconCls: 'icon-decline',
                                    text: 'Отменить'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Delete',
                                    iconCls: 'icon-cross',
                                    text: 'Удалить'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            items: [
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Показать подтвержденные',
                                    fieldStyle: 'vertical-align: middle;',
                                    name: 'ShowAccepted'
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
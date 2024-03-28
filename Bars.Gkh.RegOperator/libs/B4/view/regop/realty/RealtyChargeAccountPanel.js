Ext.define('B4.view.regop.realty.RealtyChargeAccountPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.regop.realty.RealtyChargeAccountOperationGrid',
        'B4.ux.button.Update'
    ],

    alias: 'widget.realtychargeaccpanel',
    closable: true,
    title: 'Счет начислений',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    docked: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        me.fireEvent('updateme', me);
                                    }
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    text: 'Обновить баланс',
                                    handler: function () {
                                        me.fireEvent('updatebalance', me);
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    
                    xtype: 'form',
                    border: false,
                    padding: '0 0 5 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '10 0 0 0',
                            defaults: {
                                labelWidth: 170,
                                width: 280,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    fieldLabel: 'Номер счета',
                                    xtype: 'textfield',
                                    name: 'AccountNum',
                                    itemId: 'tfAccountNum',
                                    labelWidth: 160,
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 200,
                                    fieldLabel: 'Дата открытия счета',
                                    itemId: 'dfDateOpen',
                                    name: 'DateOpen',
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата закрытия счета',
                                    itemId: 'dfDateClose',
                                    name: 'DateClose',
                                    editable: false
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '7 0 10 0',
                            defaults: {
                                labelWidth: 200,
                                width: 280,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                //{
                                //    xtype: 'textfield',
                                //    fieldLabel: '№ р/с',
                                //    name: 'BankAccountNum'
                                //},
                                {
                                    xtype: 'datefield',
                                    labelWidth: 160,
                                    format: 'd.m.Y',
                                    hideTrigger: true,
                                    fieldLabel: 'Дата последней операции',
                                    name: 'LastOperationDate'
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Начисления всего',
                                    name: 'ChargeTotal'
                                },
                                {
                                    xtype: 'numberfield',
                                    labelWidth: 170,
                                    fieldLabel: 'Оплата всего',
                                    name: 'PaidTotal'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'realtychargeaccopgrid',
                    flex: 1,
                    border: false,
                    header: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
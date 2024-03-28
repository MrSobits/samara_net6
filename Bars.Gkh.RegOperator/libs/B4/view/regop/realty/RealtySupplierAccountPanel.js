Ext.define('B4.view.regop.realty.RealtySupplierAccountPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.regop.realty.RealtySupplierAccountOperationGrid'
    ],

    alias: 'widget.realtysuppaccpanel',
    closable: true,
    title: 'Счет расчета с поставщиками',

    initComponent: function() {
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
                                    handler: function () {
                                        me.fireEvent('updateme', me);
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
                                labelWidth: 130,
                                width: 220,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    labelWidth: 160,
                                    width: 270,
                                    fieldLabel: 'Номер счета',
                                    itemId: 'tfAccountNum',
                                    name: 'AccountNum'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата открытия счета',
                                    itemId: 'dfDateOpen',
                                    name: 'OpenDate'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата закрытия счета',
                                    itemId: 'dfDateClose',
                                    name: 'CloseDate'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '7 0 10 0',
                            defaults: {
                                labelWidth: 130,
                                width: 220,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    width: 270,
                                    fieldLabel: 'Дата последней операции',
                                    labelWidth: 160,
                                    name: 'LastOperationDate'
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Cальдо',
                                    name: 'Saldo'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '№ р/с',
                                    name: 'BankAccountNum',
                                    itemId: 'tfBankAccountNum',
                                    width: 350
                                }
                            ]
                        }
                    ]
                },
                {
                    flex: 1,
                    xtype: 'realtysuppaccopgrid',
                    header: false,
                    border: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
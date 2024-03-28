Ext.define('B4.view.longtermprobject.paymentaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paymentaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 750,
    minHeight: 500,
    height: 500,
    width: 750,
    title: 'Cчет оплат',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.longtermprobject.paymentaccount.BankStatGrid'
    ],
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    enableTabScroll: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Основная информация',
                            border: false,
                            padding: '5 5 5 5',
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    allowBlank: false,
                                    maxLength: 50,
                                    name: 'Number',
                                    fieldLabel: 'Номер'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'OpenDate',
                                            fieldLabel: 'Дата открытия',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'CloseDate',
                                            fieldLabel: 'Дата закрытия'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OverdraftLimit',
                                    fieldLabel: 'Лимит по овердрафту',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'paymentaccountbankstatgrid',
                                    columnLines: true,
                                    flex: 1
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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
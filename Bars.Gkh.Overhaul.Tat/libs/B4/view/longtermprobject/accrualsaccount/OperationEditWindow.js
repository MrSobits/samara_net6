Ext.define('B4.view.longtermprobject.accrualsaccount.OperationEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.accrualsaccountopereditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    minHeight: 215,
    maxHeight: 215,
    bodyPadding: 5,
    title: 'Операция по счету начисления',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'AccrualDate',
                                    fieldLabel: 'Дата',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TotalDebit',
                                    fieldLabel: 'Итого по приходу',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TotalCredit',
                                    fieldLabel: 'Итого по расходу',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OpeningBalance',
                                    fieldLabel: 'Входящее сальдо',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ClosingBalance',
                                    fieldLabel: 'Исходящее сальдо',
                                    allowBlank: false
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
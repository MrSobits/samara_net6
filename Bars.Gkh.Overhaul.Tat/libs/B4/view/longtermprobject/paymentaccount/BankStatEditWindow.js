Ext.define('B4.view.longtermprobject.paymentaccount.BankStatEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paymentaccountbankstateditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 750,
    minHeight: 400,
    height: 400,
    width: 750,
    bodyPadding: 5,
    closable: false,
    title: 'Банковская выписка',
    itemId: 'bankStatEditWindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.longtermprobject.paymentaccount.OperationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                    {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 160,
                            flex: 1,
                            labelAlign: 'right',
                            allowBlank: false
                        },
                        padding: '0 0 5 0',
                        items: [
                            {
                                xtype: 'textfield',
                                name: 'Number',
                                itemId: 'tfNumber',
                                fieldLabel: 'Номер',
                                maxLength: 20,
                                regex: /^\d*$/,
                                regexText: 'Данное поле может содержать только цифры!'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DocumentDate',
                                itemId: 'dfDocumentDate',
                                format: 'd.m.Y',
                                fieldLabel: 'От'
                            }
                        ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                readOnly: true,
                                labelWidth: 160,
                                flex: 1,
                                labelAlign: 'right',
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ','
                            },
                            padding: '0 0 5 0',
                            items: [
                                {
                                    name: 'BalanceIncome',
                                    fieldLabel: 'Входящий остаток (руб.)'
                                },
                                {
                                    name: 'BalanceOut',
                                    fieldLabel: 'Исходящий остаток (руб.)'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            padding: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    allowBlank: true,
                                    readOnly: true,
                                    name: 'LastOperationDate',
                                    fieldLabel: 'Дата последней операции по счету',
                                    labelWidth: 160,
                                    flex: 1,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container'
                                }
                            ]
                        },
                        {
                            xtype: 'paymentaccountoperationgrid',
                            columnLines: true,
                            flex: 1,
                            disabled: true
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    action: 'StateChange',
                                    text: 'Статус',
                                    menu: []
                                },
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
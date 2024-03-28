Ext.define('B4.view.bankstatement.PaymentOrderOutEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    itemId: 'paymentOrderOutEditWindow',
    title: 'Расход',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeFinanceSource'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'column'
                    },
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            defaults: {
                                labelWidth: 130,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер П/П',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'BidNum',
                                    fieldLabel: 'Номер заявки',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Sum',
                                    fieldLabel: 'Сумма по документу*'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocId',
                                    fieldLabel: 'ID документа',
                                    maxLength: 250
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            defaults: {
                                labelWidth: 180,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата П/П',
                                    anchor: null,
                                    width: 280,
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'BidDate',
                                    fieldLabel: 'Дата заявки',
                                    anchor: null,
                                    width: 280,
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Разрез финансирования*',
                                    store: B4.enums.TypeFinanceSource.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'TypeFinanceSource'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            padding: '0 0 0 5',
                                            name: 'RepeatSend',
                                            itemId: 'chkbxRepeatSend',
                                            fieldLabel: ' Повт. направ. средства:',
                                            labelWidth: 140
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'RedirectFunds',
                                            itemId: 'dcmfRedirectFunds',
                                            disabled: true,
                                            fieldLabel: 'Сумма',
                                            flex: 1,
                                            labelWidth: 50
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'PayerContragent',
                    fieldLabel: 'Плательщик*',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'ReceiverContragent',
                    fieldLabel: 'Получатель*',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1 }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'PayPurpose',
                    fieldLabel: 'Назначение платежа',
                    maxLength: 300,
                    flex: 1
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
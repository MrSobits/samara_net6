Ext.define('B4.view.suspenseaccount.NewAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccountnewaddwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.enums.SuspenseAccountTypePayment',
        'B4.enums.MoneyDirection'
    ],

    title: 'Счет невыясненных сумм',

    width: 600,
    height: 300,
    minHeight: 300,

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 125,
                margin: 5
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            border: 0,
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateReceipt',
                    fieldLabel: 'Дата поступления',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип платежа',
                    store: B4.enums.SuspenseAccountTypePayment.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false,
                    name: 'SuspenseAccountTypePayment',
                    value: 10
                },
                {
                    xtype: 'textfield',
                    maskRe: /[0-9]/i, //только числа
                    allowBlank: false,
                    hideTrigger: true,
                    fieldLabel: 'Расчетный счет',
                    name: 'AccountBeneficiary',
                    allowDecimals: false,
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    allowBlank: false,
                    hideTrigger: true,
                    fieldLabel: 'Сумма',
                    name: 'Sum',
                    minValue: 0
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Приход/расход',
                    store: B4.enums.MoneyDirection.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'MoneyDirection'
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Назначение платежа',
                    name: 'DetailsOfPayment',
                    flex: 1,
                    maxLength: 500
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
                                    xtype: 'b4savebutton',
                                    text: 'Сохранить'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть'
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
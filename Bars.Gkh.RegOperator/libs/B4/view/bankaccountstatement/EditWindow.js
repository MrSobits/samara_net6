Ext.define('B4.view.bankaccountstatement.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.bankaccountstatementeditwindow',
    title: 'Федеральный стандарт взноса на КР',

    requires: [
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    hidetrigger: true,
                    readOnly: true,
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа'         
                },
                {
                    xtype: 'textfield',
                    readOnly: true,
                    name: 'DocumentNum',
                    fieldLabel: 'Номер документа'
                },
                {
                    xtype: 'textfield',
                    readOnly: true,
                    name: 'AccountNum',
                    fieldLabel: 'Р/с получателя'
                },
                {
                    xtype: 'textarea',
                    readOnly: true,
                    name: 'PaymentDetails',
                    fieldLabel: 'Назначение платежа'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    readOnly: true,
                    name: 'Sum',
                    fieldLabel: 'Сумма'
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
Ext.define('B4.view.calcaccount.PaymentCrSpecAccNotRegopEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.store.calcaccount.PaymentCrSpecAccNotRegop'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 15,
    alias: 'widget.paymentCrSpecAccNotRegopEditWindow',
    title: 'Редактирование данных',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Address',
                    fieldLabel: 'Адрес дома',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    margin: '0 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'AmountIncome',
                            fieldLabel: 'Сумма поступления',
                            hideTrigger: true,
                            minValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            name: 'EndYearBalance',
                            fieldLabel: 'Остаток на конец года',
                            hideTrigger: true,
                            minValue: 0
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
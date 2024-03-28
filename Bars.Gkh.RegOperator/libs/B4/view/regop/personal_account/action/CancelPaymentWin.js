Ext.define('B4.view.regop.personal_account.action.CancelPaymentWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.cancelpaymentwin',

    requires: [
        'B4.form.FileField'
    ],

    modal: true,
    closable: false,
    width: 500,
    title: 'Отмена оплаты',
    layout: 'fit',
    closeAction: 'destroy',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Account'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'AvailableSum',
                            readOnly: true,
                            fieldLabel: 'Поступления за открытый период',
                            decimalSeparator: ','
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма списания',
                            decimalSeparator: ',',
                            minValue: 0,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'Document',
                            fieldLabel: 'Документ-основание'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Reason',
                            fieldLabel: 'Причина',
                            allowBlank: false,
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
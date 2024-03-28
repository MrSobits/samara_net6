Ext.define('B4.view.bankstatement.PaymentOrderPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Платежные поручения',
    alias: 'widget.paymentOrderPanel',
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.bankstatement.PaymentOrderFilterPanel',
        'B4.view.bankstatement.PaymentOrderGrid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'paymentorderfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    frame: true,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'paymentordergrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

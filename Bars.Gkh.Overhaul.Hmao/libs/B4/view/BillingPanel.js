Ext.define('B4.view.BillingPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
       
    ],

    title: 'Модуль начисления',
    alias: 'widget.billingpanel',
    layout: 'fit',
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'component',
                    name: 'overhaulBillingFrame',
                    layout: 'fit',
                    autoEl: {
                        tag: 'iframe'
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});
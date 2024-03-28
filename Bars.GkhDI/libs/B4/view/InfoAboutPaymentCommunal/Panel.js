Ext.define('B4.view.infoaboutpaymentcommunal.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.infoaboutpaymentcommunalgridpanel',
    closable: true,
    itemId: 'infoAboutPaymentCommunalGridPanel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Сведения об оплатах коммунальных услуг',
    
    requires: [
        'B4.view.infoaboutpaymentcommunal.Grid',
        'B4.view.infoaboutpaymentcommunal.InlineGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'infoaboutpaymentcommunalgrid',
                    flex: 1
                },
                {
                    xtype: 'infoaboutpaymentcommunalinlinegrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});

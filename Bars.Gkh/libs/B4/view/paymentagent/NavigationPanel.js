Ext.define('B4.view.paymentagent.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'paymentagent.NavigationMenu',
    title: 'Платежные агенты',
    itemId: 'paymentAgentNavigationPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'paymentAgentInfoLabel'
                },
                {
                    id: 'paymentAgentMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'paymentAgentMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

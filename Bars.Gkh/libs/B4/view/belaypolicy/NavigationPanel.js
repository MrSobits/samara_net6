Ext.define('B4.view.belaypolicy.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],
    closable: true,
    storeName: 'belaypolicy.NavigationMenu',
    title: 'Страховой полис',
    itemId: 'belayPolicyNavigationPanel',
    layout: {
        type: 'border'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'belayPolicyInfoLabel'
                },
                {
                    id: 'belayPolicyMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    id: 'belayPolicyMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.politicauth.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'politicauth.NavigationMenu',
    title: 'Орган государственной власти',
    itemId: 'politicAuthNavigationPanel',
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
                    itemId: 'politicAuthInfoLabel'
                },
                {
                    id: 'politicAuthMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'politicAuthMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

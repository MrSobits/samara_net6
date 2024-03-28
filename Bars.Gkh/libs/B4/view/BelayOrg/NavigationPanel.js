Ext.define('B4.view.belayorg.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'belayorg.NavigationMenu',
    title: 'Страховые организации',
    itemId: 'belayOrgNavigationPanel',
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
                    itemId: 'belayOrgInfoLabel'
                },
                {
                    id: 'belayOrgMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    id: 'belayOrgMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

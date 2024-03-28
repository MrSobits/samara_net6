Ext.define('B4.view.localgov.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'localgov.NavigationMenu',
    title: 'Орган местного самоуправления',
    itemId: 'localGovNavigationPanel',
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
                    itemId: 'localGovInfoLabel'
                },
                {
                    id: 'localGovMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'localGovMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

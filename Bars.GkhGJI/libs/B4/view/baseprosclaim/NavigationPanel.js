Ext.define('B4.view.baseprosclaim.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'baseprosclaim.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseProsClaimNavigationPanel',
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
                    itemId: 'baseProsClaimInfoLabel'
                },
                {
                    id: 'baseProsClaimMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseProsClaimMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

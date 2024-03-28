Ext.define('B4.view.gkuinfo.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'gkuinfo.NavigationMenu',
    title: 'Сведения по дому',
    itemId: 'gkuinfoNavigationPanel',
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
                    itemId: 'gkuinfoInfoLabel'
                },
                {
                    id: 'gkuinfoMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0',
                    width: 300
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'gkuinfoMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

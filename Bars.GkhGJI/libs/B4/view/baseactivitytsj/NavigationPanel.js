Ext.define('B4.view.baseactivitytsj.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.baseActivityTsjNavigationPanel',
    closable: true,
    storeName: 'baseactivitytsj.NavigationMenu',
    itemId: 'baseActivityTsjNavigationPanel',
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
                    itemId: 'baseActivityTsjInfoLabel'
                },
                {
                    id: 'baseActivityTsjMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseActivityTsjMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
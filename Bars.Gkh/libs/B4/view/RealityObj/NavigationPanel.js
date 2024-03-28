Ext.define('B4.view.realityobj.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.realityobjNavigationPanel',
    
    closable: true,
    title: 'Жилой дом',
    
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    storeName: 'realityobj.NavigationMenu',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.NavigationMenu');

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs'
                },
                {
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: store,
                    margins: '0 2 0 0',
                    width: 300,
                    collapsible: true
                },
                {
                    xtype: 'tabpanel',
                    nId: 'navtabpanel',
                    region: 'center',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

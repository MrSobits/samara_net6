﻿Ext.define('B4.view.manorglicense.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.manorglicenseNavigationPanel',
    
    closable: true,
    title: 'Лицензия УО',
    
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.store.manorglicense.NavigationMenu'
    ],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.NavigationMenu');

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
                    width: 300
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

Ext.define('B4.view.Control.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Навигационная панель',
    itemId: 'navigationPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'informLabel'
                },
                {
                    id: 'menuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    margins: '0 2 0 0'
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'mainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
/*
Ext.define('B4.view.Control.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Навигационная панель',
    itemId: 'navigationPanel',
    layout: {
        type: 'border'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    height: 25,
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;',
                    items: [{ xtype: 'label', id: 'informLabel', hideLabel: true}]
                },
                {
                    id: 'menuTree',
                    xtype: 'treepanel',
                    title: 'Разделы',
                    region: 'west',
                    width: 200,
                    rootVisible: false,
                    store: Ext.create(this.storeName),
                    split: false,
                    collapsible: false,
                    margins: '0 2 0 0'
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'mainTab',
                    enableTabScroll: true 
                }
            ]
        });

        me.callParent(arguments);
    }
});
*/
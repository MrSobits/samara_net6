Ext.define('B4.view.wastecollection.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.wastecollectionnavigationpanel',

    closable: true,
    title: 'Площадка сбора ТБО и ЖБО',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.store.wastecollection.NavigationMenu'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.wastecollection.NavigationMenu');

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
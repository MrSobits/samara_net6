Ext.define('B4.view.objectoutdoorcr.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.objectoutdoorcrnavigationpanel',

    closable: true,
    title: 'Объект программы благоустройства',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectoutdoorcr.ObjectOutdoorCrNavigationMenu');

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
Ext.define('B4.view.tatarstanprotocolgji.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.tatarstanprotocolgjinavigationpanel',

    closable: true,
    title: 'Протокол ГЖИ',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.tatarstanprotocolgji.TatarstanProtocolGjiNavigationMenu');

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
                    nId:'navtabpanel',
                    region: 'center',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
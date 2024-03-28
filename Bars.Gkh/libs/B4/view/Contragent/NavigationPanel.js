Ext.define('B4.view.contragent.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    storeName: 'contragent.NavigationMenu',

    title: 'Контрагент',

    alias: 'widget.contragentNavigationPanel',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.contragent.NavigationMenu');

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
                    margins: '0 2 0 0'
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

Ext.define('B4.view.publicservorg.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.publicservorgnavigationpanel',

    closable: true,
    storeName: 'publicservorg.NavigationMenu',
    title: 'Поставщик ресурсов',
    
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
                    xtype: 'breadcrumbs'
                },
                {
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: me.storeName,
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

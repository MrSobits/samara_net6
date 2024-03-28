Ext.define('B4.view.supplyresourceorg.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.supplyresorgnavigationpanel',

    closable: true,
    storeName: 'supplyresourceorg.NavigationMenu',
    title: 'Поставщик коммунальных услуг',
    
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
                {   xtype: 'tabpanel',
                    region: 'center',
                    nId: 'navtabpanel',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

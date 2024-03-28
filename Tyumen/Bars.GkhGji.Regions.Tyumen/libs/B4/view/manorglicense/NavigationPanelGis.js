Ext.define('B4.view.manorglicense.NavigationPanelGis', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.manorglicenseGisNavigationPanel',

    closable: true,
    title: 'Лицензия УО',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.store.manorglicense.NavigationMenuGis'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.NavigationMenuGis');

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
                    nId: 'navtabpanelgis',
                    region: 'center',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.gasequipmentorg.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Поставщик ВДГО',
    alias: 'widget.gasequipmentorgNavigationPanel',
    layout: { type: 'vbox', align: 'stretch' },
    storeName: 'gasequipmentorg.NavigationMenu',

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    height: 25,
                    xtype: 'breadcrumbs'
                },
                {
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'menutreepanel',
                            title: 'Разделы',
                            store: me.storeName,
                            margins: '0 2 0 0',
                            width: 240
                        },
                        {
                            xtype: 'tabpanel',
                            nId: 'navtabpanel',
                            enableTabScroll: true,
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
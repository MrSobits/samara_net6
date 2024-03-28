Ext.define('B4.view.objectcr.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'objectcr.NavigationMenu',
    title: 'Объект КР',
    alias: 'widget.objectcrNavigationPanel',
    layout: { type: 'vbox', align: 'stretch' },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.objectcr.NavigationMenu');

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
                            store: store,
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
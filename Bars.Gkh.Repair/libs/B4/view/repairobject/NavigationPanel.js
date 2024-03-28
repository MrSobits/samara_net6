Ext.define('B4.view.repairobject.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'repairobject.NavigationMenu',
    title: 'Объект текущего ремонта',
    itemId: 'repairObjectNavigationPanel',
    layout: { type: 'vbox', align: 'stretch' },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    height: 25,
                    xtype: 'breadcrumbs',
                    itemId: 'repairObjectInfoLabel'
                },
                {
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            id: 'repairObjectMenuTree',
                            xtype: 'menutreepanel',
                            title: 'Разделы',
                            store: this.storeName,
                            margins: '0 2 0 0',
                            width: 240
                        },
                        {
                            xtype: 'tabpanel',
                            id: 'repairObjectMainTab',
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
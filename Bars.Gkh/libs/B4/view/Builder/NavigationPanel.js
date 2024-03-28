Ext.define('B4.view.builder.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],
    closable: true,
    storeName: 'builder.NavigationMenu',
    title: 'Подрядчик',
    itemId: 'builderNavigationPanel',
    layout: {
        type: 'border'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'builderInfoLabel'
                },
                {
                    id: 'builderMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    id: 'builderMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
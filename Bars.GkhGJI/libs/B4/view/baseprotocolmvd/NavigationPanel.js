Ext.define('B4.view.baseprotocolmvd.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'protocolmvd.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseProtocolMvdNavigationPanel',
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
                    xtype: 'breadcrumbs',
                    itemId: 'baseProtocolMvdInfoLabel'
                },
                {
                    id: 'baseProtocolMvdMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseProtocolMvdMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

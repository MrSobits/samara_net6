Ext.define('B4.view.baseprotocol197.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'protocol197.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseProtocol197NavigationPanel',
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
                    itemId: 'baseProtocol197InfoLabel'
                },
                {
                    id: 'baseProtocol197MenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseProtocol197MainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.basedefault.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'basedefault.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseDefaultNavigationPanel',
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
                    itemId: 'baseDefaultInfoLabel'
                },
                {
                    id: 'baseDefaultMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                { 
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseDefaultMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
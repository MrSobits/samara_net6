Ext.define('B4.view.baseomsu.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'baseomsu.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseOMSUNavigationPanel',
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
                    itemId: 'baseOMSUInfoLabel'
                },
                {
                    id: 'baseOMSUMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseOMSUMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

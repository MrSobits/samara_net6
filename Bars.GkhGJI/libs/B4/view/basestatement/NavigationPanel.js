Ext.define('B4.view.basestatement.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'basestatement.NavigationMenu',
    title: 'Проверка ГЖИ',
    itemId: 'baseStatementNavigationPanel',
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
                    itemId: 'baseStatementInfoLabel'
                },
                {
                    id: 'baseStatementMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseStatementMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

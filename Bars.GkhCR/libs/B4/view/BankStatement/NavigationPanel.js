Ext.define('B4.view.bankstatement.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'bankstatement.NavigationMenu',
    title: 'Банковские выписки',
    itemId: 'bankStatementNavigationPanel',
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
                    itemId: 'bankStatementInfoLabel'
                },
                {
                    id: 'bankStatementMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'bankStatementMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

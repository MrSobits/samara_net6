Ext.define('B4.view.regoperator.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'regoperator.Navigation',
    title: 'Региональный оператор',
    itemId: 'regoperatorNavigationPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'regoperatorInfoLabel'
                },
                {
                    id: 'regoperatorMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'regoperatorMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
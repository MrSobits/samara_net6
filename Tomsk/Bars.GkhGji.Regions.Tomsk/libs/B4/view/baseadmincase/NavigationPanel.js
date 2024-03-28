Ext.define('B4.view.baseadmincase.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'admincase.NavigationMenu',
    title: 'Административное дело',
    itemId: 'baseAdminCaseNavigationPanel',
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
                    itemId: 'baseAdminCaseInfoLabel'
                },
                {
                    id: 'baseAdminCaseMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseAdminCaseMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.activitytsj.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'activitytsj.NavigationMenu',
    title: 'Деятельность ТСЖ и ЖСК',
    itemId: 'activityTsjNavigationPanel',
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
                    itemId: 'activityTsjInfoLabel'
                },
                {
                    id: 'activityTsjMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    id: 'activityTsjMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.inspectionpreventiveaction.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'inspectionpreventiveaction.NavigationMenu',
    title: 'Проверка по профилактическому мероприятию',
    itemId: 'inspectionPreventiveActionNavigationPanel',
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
                    itemId: 'inspectionPreventiveActionInfoLabel'
                },
                {
                    id: 'inspectionPreventiveActionMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'inspectionPreventiveActionMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
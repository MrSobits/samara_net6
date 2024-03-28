Ext.define('B4.view.preventiveaction.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'preventiveaction.NavigationMenu',
    title: 'Профилактическое мероприятие',
    itemId: 'preventiveActionNavigationPanel',
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
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'preventiveActionInfoLabel'
                },
                {
                    id: 'preventiveActionMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0',
                    width: 300
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'preventiveActionMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

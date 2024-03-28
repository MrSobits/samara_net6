Ext.define('B4.view.baseplanaction.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.basePlanActionNavigationPanel',
    closable: true,
    storeName: 'baseplanaction.NavigationMenu',
    title: 'Проверка ГЖИ',
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
                    itemId: 'basePlanActionInfoLabel'
                },
                {
                    id: 'basePlanActionMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'basePlanActionMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

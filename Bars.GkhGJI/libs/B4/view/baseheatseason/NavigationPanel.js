Ext.define('B4.view.baseheatseason.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.baseheatseasonnavigationpanel',
    closable: true,
    storeName: 'baseheatseason.NavigationMenu',
    itemId: 'baseHeatSeasonNavigationPanel',
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
                    itemId: 'baseHeatSeasonInfoLabel'
                },
                {
                    id: 'baseHeatSeasonMenuTree',
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
                    id: 'baseHeatSeasonMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

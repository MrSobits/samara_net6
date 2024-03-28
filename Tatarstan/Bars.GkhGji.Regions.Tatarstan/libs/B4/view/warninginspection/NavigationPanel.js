Ext.define('B4.view.warninginspection.NavigationPanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.warninginspectionnavigationpanel',

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    closable: true,
    storeName: 'warninginspection.NavigationMenu',
    title: 'Предостережение',
    layout: {
        type: 'border'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'warninginspectionInfoLabel'
                },
                {
                    id: 'warninginspectionMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: me.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {
                    id: 'warninginspectionMainTab',
                    xtype: 'tabpanel',
                    region: 'center',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
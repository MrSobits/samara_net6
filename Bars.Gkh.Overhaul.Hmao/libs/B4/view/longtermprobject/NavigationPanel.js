Ext.define('B4.view.longtermprobject.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'longtermprobject.NavigationMenu',
    title: 'Жилой дом',
    alias: 'widget.longtermprobjectNavigationPanel',
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
                    itemId: 'longtermprobjectInfoLabel'
                },
                {
                    id: 'longtermprobjectMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0',
                    width: 300
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'longtermprobjectMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

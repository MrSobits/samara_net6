Ext.define('B4.view.baselicensereissuance.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'baselicensereissuance.NavigationMenu',
    title: 'Проверка ГЖИ',
    alias: 'widget.baselicensereissuancenavigationpanel',
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
                    itemId: 'baseLicenseReissuanceInfoLabel'
                },
                {
                    id: 'baseLicenseReissuanceMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseLicenseReissuanceMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

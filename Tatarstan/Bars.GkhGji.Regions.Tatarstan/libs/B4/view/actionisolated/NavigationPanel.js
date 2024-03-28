Ext.define('B4.view.actionisolated.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'actionisolated.NavigationMenu',
    title: 'Мероприятие без взаимодействия с контролируемым лицом',
    itemId: 'actionIsolatedNavigationPanel',
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
                    itemId: 'actionisolatedInfoLabel'
                },
                {
                    id: 'actionisolatedMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'actionisolatedMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
﻿Ext.define('B4.view.inspectionactionisolated.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'inspectionactionisolated.NavigationMenu',
    title: 'Мероприятие без взаимодействия с контролируемым лицом',
    itemId: 'inspectionActionIsolatedNavigationPanel',
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
                    itemId: 'inspectionActionIsolatedInfoLabel'
                },
                {
                    id: 'inspectionActionIsolatedMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'inspectionActionIsolatedMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.emergencyobj.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'emergencyobj.NavigationMenu',
    title: 'Реестр аварийных домов',
    itemId: 'emergencyObjNavigationPanel',
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
                    itemId: 'emergencyObjInfoLabel'
                },
                {
                    id: 'emergencyObjMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    id: 'emergencyObjMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

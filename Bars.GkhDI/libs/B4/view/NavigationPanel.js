Ext.define('B4.view.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'menu.NavigationMenu',
    title: 'Редактирование жилого дома',
    itemId: 'disclosureInfoRealityObjNavigationPanel',
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
                    itemId: 'disclosureInfoRealityObjInfoLabel'
                },
                {
                    id: 'disclosureInfoRealityObjMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    minWidth: 300,
                    maxWidth: 400,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    split: true
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'disclosureInfoRealityObjMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

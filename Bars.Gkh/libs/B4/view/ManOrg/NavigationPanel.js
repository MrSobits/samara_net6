Ext.define('B4.view.manorg.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.manorgnavigationpanel',
    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],
    closable: true,
    storeName: 'manorg.NavigationMenu',
    title: 'Управляющая организация',

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
                    itemId: 'manorgInfoLabel'
                },
                {
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { xtype: 'tabpanel',
                    region: 'center',
                    nId: 'navtabpanel',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

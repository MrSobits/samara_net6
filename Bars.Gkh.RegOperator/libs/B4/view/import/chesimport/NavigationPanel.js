Ext.define('B4.view.import.chesimport.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.chesimportnavigationpanel',
    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],
    closable: true,
    storeName: 'import.chesimport.NavigationMenu',
    title: 'Анализ сведений из биллинга',

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
                    itemId: 'chesimportInfoLabel'
                },
                {
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                { 
                    xtype: 'tabpanel',
                    region: 'center',
                    nId: 'navtabpanel',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

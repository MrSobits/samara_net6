Ext.define('B4.view.appealcits.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'appealcits.NavigationMenu',
    title: 'Мотивированное представление',
    itemId: 'motivatedPresentationAppealCitsNavigationPanel',
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
                    itemId: 'motivatedPresentationAppealCitsInfoLabel'
                },
                {
                    id: 'motivatedPresentationAppealCitsMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'motivatedPresentationAppealCitsMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
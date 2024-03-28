Ext.define('B4.view.baselicenseapplicants.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'baselicenseapplicants.NavigationMenu',
    title: 'Проверка ГЖИ',
    alias: 'widget.baselicenseappnavigationpanel',
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
                    itemId: 'baseLicenseApplicantsInfoLabel'
                },
                {
                    id: 'baseLicenseApplicantsMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0',
                    collapsible: true
                },
                {   xtype: 'tabpanel',
                    region: 'center',
                    id: 'baseLicenseApplicantsMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});

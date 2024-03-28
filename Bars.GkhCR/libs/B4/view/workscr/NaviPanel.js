Ext.define('B4.view.workscr.NaviPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.workscrnavipanel',
    
    closable: true,
    title: 'Объект КР (работы)',
    
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.store.workscr.Navi'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'breadcrumbs',
                    region: 'north'
                },
                {
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: 'workscr.Navi',
                    margins: '0 2 0 0',
                    width: 300
                },
                {
                    xtype: 'tabpanel',
                    nId: 'navtabpanel',
                    region: 'center',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
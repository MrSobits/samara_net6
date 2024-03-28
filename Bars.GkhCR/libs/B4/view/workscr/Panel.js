Ext.define('B4.view.workscr.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Объекты капитального ремонта (работы)',
    alias: 'widget.workcrpanel',

    layout: {
        type: 'border'
    },

    closeAction: 'destroy',
    requires: [
        'B4.view.workscr.FilterPanel',
        'B4.view.workscr.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'workscrfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'workscrgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

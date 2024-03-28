Ext.define('B4.view.objectcr.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Объекты капитального ремонта',
    alias: 'widget.objectCrPanel',

    layout: {
        type: 'border'
    },

    closeAction: 'destroy',
    requires: [
        'B4.view.objectcr.FilterPanel',
        'B4.ux.grid.Panel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'objectcrfilterpnl',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'objectCrTabPanel',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'objectcrgrid',
                            region: 'center',
                            title: 'Объекты капитального ремонта'
                        },
                        {
                            xtype: 'deletedobjectcrgrid',
                            title: 'Удаленные объекты капитального ремонта'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

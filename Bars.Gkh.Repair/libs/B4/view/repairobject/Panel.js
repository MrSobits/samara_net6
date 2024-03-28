Ext.define('B4.view.repairobject.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Объекты текущего ремонта',
    alias: 'widget.repairObjectPanel',
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.repairobject.FilterPanel',
        'B4.view.repairobject.Grid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'repairobjectfilterpnl',
                   region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'repairobjectgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

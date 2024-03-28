Ext.define('B4.view.logentity.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    
    title: 'Журнал действий пользователя',
    
    alias: 'widget.logentitypnl',
    
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.logentity.FilterPanel',
        'B4.view.logentity.Grid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'logentityfilterpnl',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2
                },
                {
                    xtype: 'logentitygrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
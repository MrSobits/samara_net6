Ext.define('B4.view.workactregister.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр актов выполненных работ',
    alias: 'widget.workActRegisterPanel',
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.workactregister.Grid',
        'B4.view.workactregister.FilterPanel'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'workactregfilterpnl',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                
                {
                    xtype: 'workactreggrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

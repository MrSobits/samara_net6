Ext.define('B4.view.claimwork.restructdebt.MainPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.restructdebtpanel',
    title: 'Рестуктуризация долга',

    
    bodyPadding: 5,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.claimwork.restructdebt.Edit',
        'B4.view.claimwork.restructdebt.ScheduleGrid',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'restructdebteditpanel',
                },
                {
                    xtype: 'restructschedulegrid',
                    flex: 1
                }
            ],
        });
        me.callParent(arguments);
    }
});
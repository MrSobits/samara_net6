Ext.define('B4.view.report.CrShortTermPlanPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportcrshorttermplanpanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: ['B4.view.Control.GkhTriggerField'],

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все'
                },
                {
                    xtype: 'numberfield',
                    name: 'StartYear',
                    fieldLabel: 'Год начала периода',
                    allowBlank: false,
                    allowDecimals: false
                },
                {
                    xtype: 'numberfield',
                    name: 'EndYear',
                    fieldLabel: 'Год окончания периода',
                    allowBlank: false,
                    allowDecimals: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
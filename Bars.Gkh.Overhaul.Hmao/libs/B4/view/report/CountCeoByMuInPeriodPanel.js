Ext.define('B4.view.report.CountCeoByMuInPeriodPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.countceobymuinperiodpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: ['B4.view.Control.GkhTriggerField'],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'numberfield',
                    name: 'StartYear',
                    fieldLabel: 'Год начала',
                    allowBlank: false,
                    allowDecimals: false
                },
                {
                    xtype: 'numberfield',
                    name: 'EndYear',
                    fieldLabel: 'Год окончания',
                    allowBlank: false,
                    allowDecimals: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});
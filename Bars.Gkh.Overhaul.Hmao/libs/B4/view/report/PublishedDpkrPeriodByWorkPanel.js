Ext.define('B4.view.report.PublishedDpkrPeriodByWorkPanel', {
    
    alias: 'widget.reportPublishedDpkrPeriodByWorkPanel',

    extend: 'Ext.form.Panel',
    title: '',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

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
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'numberfield',
                    minValue: 1,
                    maxValue: 10,
                    name: 'GroupPeriod',
                    fieldLabel: 'Период группировки ДПКР (лет)',
                    value: 5
                }
            ]
        });

        me.callParent(arguments);
    }
});
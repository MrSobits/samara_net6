Ext.define('B4.view.report.PublishedDpkrExtendedPanel', {
    
    alias: 'reportPublishedDpkrExtendedPanel',

    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'reportPublishedDpkrExtendedPanel',
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
                    itemId: 'tfMunicipality',
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
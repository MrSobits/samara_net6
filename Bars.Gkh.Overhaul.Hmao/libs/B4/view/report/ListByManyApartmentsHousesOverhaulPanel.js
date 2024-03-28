Ext.define('B4.view.report.ListByManyApartmentsHousesOverhaulPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'listByManyApartmentsHousesOverhaulPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
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
                }
            ]
        });
        me.callParent(arguments);
    }
});
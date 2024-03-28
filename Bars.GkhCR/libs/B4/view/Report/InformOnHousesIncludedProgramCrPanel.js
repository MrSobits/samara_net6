Ext.define('B4.view.report.InformOnHousesIncludedProgramCrPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'informOnHousesIncludedProgramCrPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
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
                    xtype: 'gkhtriggerfield',
                    name: 'Period',
                    itemId: 'tfPeriod',
                    fieldLabel: 'Период',
                    emptyText: 'Все периоды'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'StateObj',
                    itemId: 'tfState',
                    fieldLabel: 'Статус объекта',
                    emptyText: 'Все статусы объектов'
                }
            ]
        });

        me.callParent(arguments);
    }
});
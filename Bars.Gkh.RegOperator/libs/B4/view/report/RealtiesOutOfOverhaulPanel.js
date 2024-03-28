Ext.define('B4.view.report.RealtiesOutOfOverhaulPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportRealtiesOutOfOverhaulPanel',
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
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});
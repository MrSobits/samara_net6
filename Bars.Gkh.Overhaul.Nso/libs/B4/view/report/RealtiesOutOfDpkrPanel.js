Ext.define('B4.view.report.RealtiesOutOfDpkrPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportRealtiesOutOfDpkrPanel',
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
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальный район',
                    emptyText: 'Все МР'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Settlements',
                    fieldLabel: 'Муниципальное образование',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});
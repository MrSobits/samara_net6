Ext.define('B4.view.report.CheckMkdAndOwnersPremisesPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'checkMkdAndOwnersPremisesPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Municipality'
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
                    itemId: 'Municipality',
                    fieldLabel: 'Муниципальный район',
                    emptyText: 'Все МР'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});
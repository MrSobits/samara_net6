Ext.define('B4.view.report.OwnersRoNotInLongTermPrPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'ownersRoNotInLongTermPrPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox'
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
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ActualityDate',
                    itemId: 'dfActualityDate',
                    fieldLabel: 'Дата актуальности',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ParentMu',
                    itemId: 'tfParentMu',
                    fieldLabel: 'Муниципальные районы',
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
Ext.define('B4.view.report.PeopleFundsTransferToGisuInfoPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'peopleFundsTransferToGisuInfoPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid'
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
                    xtype: 'datefield',
                    name: 'dateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'dateEnd',
                    itemId: 'dfdDateEnd',
                    fieldLabel: 'Конец периода',
                    format: 'd.m.Y',
                    value: new Date(),
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.report.ReferenceOnGroundsAccidentPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'gisuOrderPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
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
                    xtype: 'datefield',
                    name: 'DateReport',
                    itemId: 'dfDateReport',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false
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

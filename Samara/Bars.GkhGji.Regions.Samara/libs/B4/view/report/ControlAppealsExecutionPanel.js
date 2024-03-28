Ext.define('B4.view.report.ControlAppealsExecutionPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'controlAppealsExecutionPanel',
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
                    xtype: 'datefield',
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Inspectors',
                    itemId: 'tfInspectors',
                    fieldLabel: 'Инспекторы',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.report.Form1ControllPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'form1ControllPanel',
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
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    fieldLabel: 'Окончание периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Head',
                    itemId: 'tfHead',
                    fieldLabel: 'Руководитель организации',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Person',
                    itemId: 'tfPerson',
                    fieldLabel: 'Должностное лицо',
                    allowBlank: false,
                    editable: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
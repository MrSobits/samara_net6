Ext.define('B4.view.report.ProtocolResponsibilityPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'protocolResponsibilityPanel',
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
                    xtype: 'b4combobox',
                    name: 'Canceled',
                    itemId: 'cbCanceled',
                    fieldLabel: 'Отмененные протоколы',
                    editable: false,
                    value: 0,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']]
                },
                {
                    xtype: 'b4combobox',
                    name: 'Returned',
                    itemId: 'cbReturned',
                    fieldLabel: 'Возвращенные протоколы',
                    editable: false,
                    value: 0,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']]
                },
                {
                    xtype: 'b4combobox',
                    name: 'Remarked',
                    itemId: 'cbRemarked',
                    fieldLabel: 'Прекращенные протоколы',
                    editable: false,
                    value: 0,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']]
                }
            ]
        });

        me.callParent(arguments);
    }
});
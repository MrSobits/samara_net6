Ext.define('B4.view.report.ProtocolTotalTablePanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'protocolTotalTablePanel',
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
                    fieldLabel: 'Отмененные постановления',
                    editable: false,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']],
                    value: 0
                },
                {
                    xtype: 'b4combobox',
                    name: 'Returned',
                    itemId: 'cbReturned',
                    fieldLabel: 'Постановления, возвращенные на новое рассмотрение',
                    editable: false,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']],
                    value: 0
                },
                {
                    xtype: 'b4combobox',
                    name: 'Remarked',
                    itemId: 'cbRemarked',
                    fieldLabel: 'Прекращенные/устные замечания',
                    editable: false,
                    items: [[0, 'Учитывать'], [1, 'Не учитывать']],
                    value: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});
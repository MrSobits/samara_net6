Ext.define('B4.view.report.DevicesByRealityObjectPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'devicesByRealityObjectPanel',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeDevice',
                    itemId: 'sfTypeDevice',
                    fieldLabel: 'Тип прибора',
                    editable: false,
                    items: [[22, 'Счетчик тепловой энергии (отопления)'], [18, 'Счетчик ГВС'], [17, 'Счетчик XВС'], [21, 'Счетчик электроэнергии'], [20, 'Счетчик газоснабжения']],
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});

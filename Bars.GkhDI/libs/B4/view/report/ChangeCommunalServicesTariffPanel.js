Ext.define('B4.view.report.ChangeCommunalServicesTariffPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'changeCommunalServicesTariffPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
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
                    name: 'StartDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    value: new Date(),
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    itemId: 'dfEndDate',
                    fieldLabel: 'Окончание периода',
                    format: 'd.m.Y',
                    value: new Date(),
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
                    name: 'ServiceType',
                    itemId: 'tfServiceType',
                    fieldLabel: 'Тип услуги',
                    emptyText: 'Все услуги'
                },
                {
                    xtype: 'b4combobox',
                    name: 'TransmitOrg',
                    allowBlank: false,
                    itemId: 'cbTransmitOrg',
                    fieldLabel: 'Организации, передавшие функции управления',
                    editable: false,
                    items: [[10, 'Учитывать'], [20, 'Не учитывать']],
                    value: 10
                }
            ]
        });

        me.callParent(arguments);
    }
});
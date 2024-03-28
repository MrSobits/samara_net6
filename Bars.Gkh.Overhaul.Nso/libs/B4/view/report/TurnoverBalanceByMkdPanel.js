Ext.define('B4.view.report.TurnoverBalanceByMkdPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'turnoverBalanceByMkdPanel',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
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
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    labelAlign: 'right',
                    name: 'MethodFormFund',
                    itemId: 'mffMethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    displayField: 'Display',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['Value', 'Display'],
                        data: [
                            { "Value": 10, "Display": "На счете регионального оператора" },
                            { "Value": 20, "Display": "На специальном счете" }
                        ]
                    }),
                    labelWidth: 200,
                    valueField: 'Value',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
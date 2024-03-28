Ext.define('B4.view.report.FundFormationStimulPanel', {    
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'fundFormationStimulPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
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
                    xtype: 'b4selectfield',
                    name: 'MunicipalitiesR',
                    itemId: 'municipalityR',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.municipality.MoArea',
                    editable: false,
                    emptyText: 'Все МР',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            header: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
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
                            { "Value": 0, "Display": "На специальном счете" },
                            { "Value": 1, "Display": "На счете регионального оператора" },
                            { "Value": 2, "Display": "На специальном счете, владелец региональный оператор" },
                            { "Value": 3, "Display": "Не выбрано"}
                        ]
                    }),
                    labelWidth: 200,
                    value: 0,
                    valueField: 'Value'
                },
                {
                    xtype: 'datefield',
                    name: 'DateFrom',
                    itemId: 'dfDateFrom',
                    fieldLabel: 'Дата с',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateTo',
                    itemId: 'dfDateTo',
                    fieldLabel: 'Дата по',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ]
        });
        
        me.callParent(arguments);
    }
});
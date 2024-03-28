Ext.define('B4.view.report.FundFormationPanel', {    
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'fundFormationPanel',
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
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'MunicipalitiesO',
                    itemId: 'municipalityO',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.MunicipalityByParent',
                    editable: false,
                    emptyText: 'Все МО',
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'MethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Contragents',
                    itemId: 'tfContragents',
                    fieldLabel: 'Наименование владельца спецсчета',
                    emptyText: 'Любой'
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    labelAlign: 'right',
                    name: 'BankReference',
                    itemId: 'cbBankReference',
                    fieldLabel: 'Справка банка (наличие)',
                    displayField: 'Display',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['Value', 'Display'],
                        data: [
                            { "Value": 0, "Display": "-"},
                            { "Value": 1, "Display": "Да" },
                            { "Value": 2, "Display": "Нет" }
                        ]
                    }),
                    labelWidth: 200,
                    value: 0,
                    valueField: 'Value'
                },
                {
                    xtype: 'checkbox',
                    itemId: 'cbIncludeRosNotInPublishedProgram',
                    name: 'IncludeRosNotInPublishedProgram',
                    labelWidth: 450,
                    fieldLabel: 'Не фильтровать дома по признаку присутствия в опубликованной программе'
                }
            ]
        });
        
        me.callParent(arguments);
    }
});
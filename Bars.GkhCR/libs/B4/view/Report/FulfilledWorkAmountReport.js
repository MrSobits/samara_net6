Ext.define('B4.view.report.FulfilledWorkAmountReport', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'fulfilledWorkAmountReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid'
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
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    fieldLabel: 'Программа',
                    store: 'B4.store.dict.ProgramCr',
                   

                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'FinSource',
                    itemId: 'tfFinSource',
                    fieldLabel: 'Разрез финансирования',
                    emptyText: 'Все разрезы'
                },
                {
                    xtype: 'b4combobox',
                    name: 'RenovType',
                    itemId: 'cbRenovType',
                    fieldLabel: 'Вид ремонта',
                    editable: false,
                    items: [[0, 'Не задано'], [1, 'Приборы учета'], [2, 'Комплексный'], [3, 'Комплексный (основной)']],
                    value: 0,
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
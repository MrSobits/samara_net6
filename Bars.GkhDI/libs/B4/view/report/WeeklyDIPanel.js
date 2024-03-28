Ext.define('B4.view.report.WeeklyDIPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'weeklyDIPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.PeriodDi',
        'B4.view.dict.perioddi.Grid'
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
                    name: 'PeriodDi',
                    itemId: 'sfPeriodDi',
                    fieldLabel: 'Период',
                    store: 'B4.store.dict.PeriodDi',
                   

                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования'
                },
                {
                   xtype: 'numberfield',
                   fieldLabel: 'Начато раскрытие информации от, %',
                   itemId: 'fDiStarted',
                   name: 'DiStarted',
                   minValue: 0,
                   maxValue: 100,
                   value: 10
                   
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Раскрыта информация полностью, от %',
                    itemId: 'fDiFull',
                    name: 'DiFull',
                    minValue: 0,
                    maxValue: 100,
                    value: 100
                },
                {
                    xtype: 'b4combobox',
                    name: 'TranssferredManag',
                    itemId: 'cbTranssferredManag',
                    fieldLabel: 'Передано управление',
                    editable: false,
                    items: [[null, ' '], [true, 'Учитывать'], [false, 'Не учитывать']],
                    defaultValue: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});
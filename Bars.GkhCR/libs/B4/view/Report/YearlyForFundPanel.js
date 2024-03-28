Ext.define('B4.view.report.YearlyForFundPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'yearlyForFundPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid',
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
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',
                   

                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
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
                    name: 'FinanceSources',
                    itemId: 'tfFinSources',
                    fieldLabel: 'Разрезы финансирования',
                    emptyText: 'Все разрезы'
                },
                {
                    xtype: 'b4combobox',
                    name: 'AssemblyTo',
                    itemId: 'cbAssemblyTo',
                    fieldLabel: 'Сборка по',
                    editable: false,
                    items: [[10, 'По ходу работ'], [20, 'По плану']],
                    value: 10
                }
            ]
        });

        me.callParent(arguments);
    }
});

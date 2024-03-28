Ext.define('B4.view.report.JournalKr34Panel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'journalKr34Panel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid',
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
                    textProperty: 'Name',
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
                    xtype: 'b4combobox',
                    name: 'TypeWork',
                    itemId: 'cbTypeWork',
                    fieldLabel: 'Работы',
                    editable: false,
                    value: 0,
                    store: Ext.create('Ext.data.Store', {
                        fields: ['value', 'name'],
                        data: [
                            { 'value': 0, 'name': 'Начаты' },
                            { 'value': 1, 'name': 'Завершены' }
                        ]
                    }),
                    valueField: 'value',
                    displayField: 'name',
                    queryMode: 'local',
                    allowBlank: false
                }

            ]
        });
        me.callParent(arguments);
    }
});
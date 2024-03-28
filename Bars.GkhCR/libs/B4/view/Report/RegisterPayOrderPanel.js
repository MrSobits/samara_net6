Ext.define('B4.view.report.RegisterPayOrderPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'registerPayOrderPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid',
        'B4.enums.TypeFinanceGroup'
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
                    xtype: 'combobox', editable: false,
                    itemId: 'cbxTypeFinGroup',
                    fieldLabel: 'Группа финансирования',
                    store: B4.enums.TypeFinanceGroup.getStore(),
                    allowBlank: false,
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeFinanceGroup'
                }
            ]
        });

        me.callParent(arguments);
    }
});
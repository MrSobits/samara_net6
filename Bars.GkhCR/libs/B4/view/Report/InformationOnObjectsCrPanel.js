Ext.define('B4.view.report.InformationOnObjectsCrPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'InformationOnObjectsCrPanel',

    layout: { type: 'vbox' },

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
                    xtype: 'datefield',
                    name: 'DateReport',
                    itemId: 'dfDateReport',
                    fieldLabel: 'На дату',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
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
                    name: 'FinanceSource',
                    itemId: 'tfFinanceSource',
                    fieldLabel: 'Разрез финансирования',
                    emptyText: 'Все разрезы'
                }
            ]
        });

        me.callParent(arguments);
    }
});
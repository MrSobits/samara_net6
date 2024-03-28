Ext.define('B4.view.report.CompareProgramsPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'compareProgramsPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
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
                    name: 'ProgramCrOne',
                    itemId: 'sfProgramCrOne',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап.ремонта 1',
                    store: 'B4.store.dict.ProgramCr',
                   

                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCrTwo',
                    itemId: 'sfProgramCrTwo',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап.ремонта 2',
                    store: 'B4.store.dict.ProgramCr',
                   

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
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                     xtype: 'gkhtriggerfield',
                     name: 'FinanceSource',
                     itemId: 'tfFinanceSource',
                     fieldLabel: 'Разрезы финансирования',
                     emptyText: 'Все разрезы'
                 }

            ]
        });
        me.callParent(arguments);
    }
});
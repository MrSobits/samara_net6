Ext.define('B4.view.report.AllocationFundsToPeopleInCrPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'allocationFundsToPeopleInCrPanel',
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
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    allowBlank: false,
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',
                   

                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'dateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'dateEnd',
                    itemId: 'dfdDateEnd',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y',
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
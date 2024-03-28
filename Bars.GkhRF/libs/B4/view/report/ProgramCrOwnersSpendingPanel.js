Ext.define('B4.view.report.ProgramCrOwnersSpendingPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'ProgramCrOwnersSpendingPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField'
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
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
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
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'FinanceSources',
                    itemId: 'tfFinSources',
                    fieldLabel: 'Разрезы финансирования',
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'CrPrograms',
                    itemId: 'tfCrPrograms',
                    fieldLabel: 'Программы кап. ремонта',
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    name: 'Returned',
                    itemId: 'cbReturned',
                    fieldLabel: 'Лимит по программам',
                    editable: false,
                    items: [[30, 'По активным'], [40, 'По всем']],
                    value: 40
                }
            ]
        });

        me.callParent(arguments);
    }
});
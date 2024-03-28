Ext.define('B4.view.report.CreatedRealtyObjectPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'CreatedRealtyObjectPanel',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'CrPrograms',
                    itemId: 'tfCrPrograms',
                    fieldLabel: 'Программы кап. ремонта',
                    emptyText: 'Все программы КР'
                },
                {
                    xtype: 'b4combobox',
                    name: 'AssemblyTo',
                    itemId: 'cbAssemblyTo',
                    fieldLabel: 'Сборка по',
                    editable: false,
                    items: [[10, 'По всем домам'], [20, 'По наличию договора с ГИСУ']],
                    defaultValue: 10,
                    emptyText: 'По всем домам'
                }
            ]
        });

        me.callParent(arguments);
    }
});
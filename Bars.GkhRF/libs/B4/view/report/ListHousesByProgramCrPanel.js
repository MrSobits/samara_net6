Ext.define('B4.view.report.ListHousesByProgramCrPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'listHousesByProgramCrPanel',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ProgramsCr',
                    itemId: 'sfProgramsCr',
                    fieldLabel: 'Программы кап. ремонта',
                    allowBlank: false
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
                    name: 'AssemblyTo',
                    itemId: 'cbAssemblyTo',
                    fieldLabel: 'Сборка по',
                    editable: false,
                    items: [[10, 'Всем домам'], [20, 'Наличию договора с ГИСУ']],
                    defaultValue: 10,
                    emptyText: 'Всем домам'
                }

            ]
        });
        me.callParent(arguments);
    }
});
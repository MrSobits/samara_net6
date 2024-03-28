Ext.define('B4.view.report.NeedMaterialsExtendedReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'needMaterialsExtendedReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid',
        'B4.store.dict.Work',
        'B4.view.dict.work.Grid'
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
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'На дату',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipalities',
                    itemId: 'sfMunicipality',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.dict.Municipality',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'TypeWorkCr',
                    itemId: 'tfTypeWorkCr',
                    fieldLabel: 'Вид работы',
                    emptyText: 'Все работы'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Condition',
                    itemId: 'cbCondition',
                    fieldLabel: 'Условие',
                    editable: false,
                    items: [[0, 'Не задано'], [1, 'Больше'], [2, 'Меньше'], [3, 'Равно']],
                    value: 0,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    anchor: '100%',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'Sum',
                    itemId: 'nfSum',
                    fieldLabel: 'Сумма'
                }
            ]
        });

        me.callParent(arguments);
    }
});
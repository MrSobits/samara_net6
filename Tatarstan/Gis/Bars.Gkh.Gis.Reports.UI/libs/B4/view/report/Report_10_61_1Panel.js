Ext.define('B4.view.report.Report_10_61_1Panel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.GisMonthPicker',
        'B4.form.SelectField',
        'B4.enums.ReportService',
        'B4.store.kp50.District'
    ],

    itemId: 'ReportPanel_10_61_1',

    layout: { type: 'vbox' },

    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                width: 400,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4gismonthpicker',
                    name: 'ReportDate',
                    width: 250
                },
                {
                    xtype: 'b4selectfield',
                    name: 'District',
                    fieldLabel: 'Район',
                    store: 'B4.store.kp50.District',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                        { text: 'Код', dataIndex: 'Code', flex: 1 }
                    ]
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    name: 'Service',
                    fieldLabel: 'Услуга',
                    displayField: 'Display',
                    store: B4.enums.ReportService.getStore(),
                    valueField: 'Value'
                }
            ]
        });

        me.callParent(arguments);
    }
});

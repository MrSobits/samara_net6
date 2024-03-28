Ext.define('B4.view.report.Report_5_37_3Panel', {
    extend: 'Ext.form.Panel',
    itemId: 'ReportPanel_5_37_3',

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality'
    ],
    
    layout: { type: 'vbox' },

    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartReportDate',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600
                },
                {
                    xtype: 'datefield',
                    name: 'EndReportDate',
                    fieldLabel: 'Дата окончания',
                    format: 'm-Y',
                    allowBlank: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Region',
                    textProperty: 'Name',
                    fieldLabel: 'Регион',
                    store: 'B4.store.dict.Municipality',
                    listView: 'B4.view.dict.municipality.Grid',
                    editable: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name', flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

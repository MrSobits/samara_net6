Ext.define('B4.view.report.Report_SZ_Collection_ServicePanel', {
    extend: 'Ext.form.Panel',
    itemId: 'ReportPanel_SZ_Collection_Service',

    requires: [
        'B4.form.ComboBox',
        'B4.enums.TypeReportCollection'
    ],
    
    layout: { type: 'vbox' },

    border: false,

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
                    name: 'ReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    storeAutoLoad: true,
                    queryMode: 'local',
                    name: 'ReportType',
                    editable: false,
                    fieldLabel: 'Тип услуг',
                    emptyText: 'Выберите тип услуг...',
                    valueField: 'Value',
                    displayField: 'Display',
                    store: B4.enums.TypeReportCollection.getStore()
                },
                {
                    xtype: 'b4combobox',
                    storeAutoLoad: true,
                    queryMode: 'local',
                    name: 'ReportArea',
                    editable: false,
                    fieldLabel: 'Район',
                    emptyText: 'Выберите район...',
                    valueField: 'Id',
                    displayField: 'Name',
                    url: '/ReportArea/ListWithoutPaging'
                }
            ]
        });

        me.callParent(arguments);
    }
});
 
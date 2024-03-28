Ext.define('B4.view.report.Report_5_37_2Panel', {
    extend: 'Ext.form.Panel',
    itemId: 'ReportPanel_5_37_2',

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
                    format: 'd.m.Y',
                    allowBlank: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600
                }
            ]
        });

        me.callParent(arguments);
    }
});

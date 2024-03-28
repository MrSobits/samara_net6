Ext.define('B4.view.report.Report_MKDPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'ReportPanel_MKD',

    layout: { type: 'vbox' },

    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'datefield',
                    name: 'ReportDate',
                    fieldLabel: 'Дата',
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

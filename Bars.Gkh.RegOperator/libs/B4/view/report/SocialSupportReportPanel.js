Ext.define('B4.view.report.SocialSupportReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.socialSupportReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    initComponent: function() {
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
                    name: 'StartDate',
                    fieldLabel: 'Дата с',
                    allowBlank: false,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Дата по',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});
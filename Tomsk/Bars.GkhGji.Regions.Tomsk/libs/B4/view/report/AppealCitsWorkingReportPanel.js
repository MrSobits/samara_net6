Ext.define('B4.view.report.AppealCitsWorkingReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'appealCitsWorkingReportPanel',
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
                   xype: 'datefield',
                   xtype: 'datefield',
                   name: 'ReportDateStart',
                   itemId: 'dfReportDateStart',
                   fieldLabel: 'Дата начала периода',
                   format: 'd.m.Y',
                   allowBlank: false
               },
               {
                   xype: 'datefield',
                   xtype: 'datefield',
                   name: 'ReportDateEnd',
                   itemId: 'dfReportDateEnd',
                   fieldLabel: 'Дата окончания периода',
                   format: 'd.m.Y',
                   allowBlank: false
               },
               {
                    xtype: 'gkhtriggerfield',
                    name: 'Inspectors',
                    itemId: 'tfInspectors',
                    fieldLabel: 'Инспекторы',
                    emptyText: 'Все'
               }
            ]
        });

        me.callParent(arguments);
    }
});
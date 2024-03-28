Ext.define('B4.view.report.OlapByInspectionReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    widget: 'alias.olapbyinspectionreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    itemId: 'olapbyinspectionreportpanel',
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
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
                    xtype: 'datefield',
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    fieldLabel: 'Дата конца',
                    format: 'd.m.Y',
                    allowBlank: false
                }
            ]
        });
        me.callParent(arguments);
    }
});
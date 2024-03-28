Ext.define("B4.view.report.AnalyticalReportBySubjectPanel", {
    extend: "Ext.form.Panel",
    title: '',
    alias: 'widget.analyticalreportbysubjectpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [                
                {
                    xtype: 'numberfield',
                    name: 'Year',
                    fieldLabel: 'Год',
                    allowBlank: false,
                    allowDecimals: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600,
                    minValue: 1900,
                    value: 2014,
                    maxValue: 2100
                }
            ]
        });

        me.callParent(arguments);
    }
});
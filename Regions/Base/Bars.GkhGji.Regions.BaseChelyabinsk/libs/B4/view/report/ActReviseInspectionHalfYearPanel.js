Ext.define("B4.view.report.ActReviseInspectionHalfYearPanel", {
    extend: "Ext.form.Panel",
    title: '',
    alias: 'widget.actReviseInspectionHalfYearPanel',
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
                    minValue: 2010,
                    value: 2014,
                    maxValue: 2100
                },
                {
                    xtype: 'numberfield',
                    name: 'HalfYear',
                    fieldLabel: 'Полугодие',
                    allowBlank: false,
                    allowDecimals: false,
                    labelWidth: 200,
                    labelAlign: 'right',
                    width: 600,
                    minValue: 1,
                    value: 1,
                    maxValue: 2
                }
            ]
        });

        me.callParent(arguments);
    }
});
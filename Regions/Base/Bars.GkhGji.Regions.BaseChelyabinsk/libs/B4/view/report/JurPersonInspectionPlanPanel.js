Ext.define("B4.view.report.JurPersonInspectionPlanPanel", {
    extend: "Ext.form.Panel",
    title: '',
    alias: 'widget.jurPersonInspectionPlanPanel',
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
                }
            ]
        });

        me.callParent(arguments);
    }
});
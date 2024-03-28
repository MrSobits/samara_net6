Ext.define('B4.view.report.NotificationFormFundReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'notifformfundreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [],

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
                    xtype: 'numberfield',
                    name: 'Year',
                    fieldLabel: 'Отчетный год',
                    labelAlign: 'right',
                    minValue: 0,
                    allowDecimals: false,
                    listeners: {
                        afterrender: function(cmp) {
                            cmp.setValue((new Date()).getFullYear());
                        }
                    }
                }
            ]
        });
        me.callParent(arguments);
    }
});
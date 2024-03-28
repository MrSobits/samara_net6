Ext.define('B4.view.report.PaymentCrMonthByRealObjPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'paymentCrMonthByRealObjPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
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
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});

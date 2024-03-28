Ext.define('B4.view.report.JournalAcPaymentPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'JournalAcPaymentPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
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
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    value: new Date(new Date().getFullYear(), 0, 1),
                    allowBlank: false
                },
                {
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDateFinish',
                    itemId: 'dfReportDateFinish',
                    fieldLabel: 'Конец периода',
                    format: 'd.m.Y',
                    allowBlank: false,
                    value: new Date()
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
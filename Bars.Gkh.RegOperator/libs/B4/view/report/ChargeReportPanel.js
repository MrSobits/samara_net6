Ext.define('B4.view.report.ChargeReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.chargeReportPanel',
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
                    xtype: 'datefield',
                    name: 'startDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'endDate',
                    itemId: 'dfEndDate',
                    fieldLabel: 'Окончание периода',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ManagingOrganizations',
                    itemId: 'tfManagingOrganization',
                    fieldLabel: 'Управляющие организации',
                    emptyText: 'Все УО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'States',
                    itemId: 'tfStates',
                    fieldLabel: 'Статус платежных поручений',
                    emptyText: 'Все статусы'
                }
            ]
        });

        me.callParent(arguments);
    }
});
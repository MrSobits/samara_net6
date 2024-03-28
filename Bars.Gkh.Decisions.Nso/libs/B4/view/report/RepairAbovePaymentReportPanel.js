Ext.define('B4.view.report.RepairAbovePaymentReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.chargeReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.enums.CrFundFormationDecisionType',
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
                    name: 'Date',
                    itemId: 'dfDate',
                    fieldLabel: 'По состоянию на',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ParentMu',
                    itemId: 'tfParentMu',
                    fieldLabel: 'Муниципальные районы',
                    emptyText: 'Все МР'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    itemId: 'cbCrFund',
                    name: 'CrFundFormationDecisionType',
                    fieldLabel: 'Способ формирования фонда',
                    displayField: 'Display',
                    store: B4.enums.CrFundFormationDecisionType.getStore(),
                    valueField: 'Value',
                    value: 0
                },
                {
                    xtype: 'b4combobox',
                    name: 'FondSize',
                    itemId: 'cbFondSize',
                    fieldLabel: 'Размер фонда',
                    editable: false,
                    items: [[true, '= минимальному'], [false, '> минимального']],
                    value: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
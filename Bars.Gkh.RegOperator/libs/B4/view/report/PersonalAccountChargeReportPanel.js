Ext.define('B4.view.report.PersonalAccountChargeReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.personalAccountChargeReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.TreeSelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.MunicipalityTree',
        'B4.store.RealtyObjectByMo'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
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
                    xtype: 'gkhtriggerfield',
                    name: 'Addresses',
                    itemId: 'tfAddress',
                    fieldLabel: 'Адреса МКД',
                    emptyText: 'Все адреса'
                }
            ]
        });

        me.callParent(arguments);
    }
});
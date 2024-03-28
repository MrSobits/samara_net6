Ext.define('B4.view.report.OwnerAndGovernmentDecisionReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ownerandgovernmentdecisionreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Address',
                    itemId: 'tfAddress',
                    fieldLabel: 'Адреса',
                    emptyText: 'Все адреса'
                },
                {
                    xtype: 'b4combobox',
                    name: 'DecisionType',
                    items: [
                        [1, 'Протокол решений собственников'],
                        [2, 'Протокол органов гос. власти'],
                        [3, 'Нет протокола']
                    ],
                    fieldLabel: 'Тип протокола',
                    editable: false,
                    value: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});
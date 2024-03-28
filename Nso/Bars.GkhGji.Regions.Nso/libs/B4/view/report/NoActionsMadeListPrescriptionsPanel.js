Ext.define('B4.view.report.NoActionsMadeListPrescriptionsPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportNoActionsMadeListPrescriptionsPanel',
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
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'Municipalities',
                            itemId: 'tfMunicipality',
                            fieldLabel: 'Муниципальные образования',
                            emptyText: 'Все МО',
                            width: 600
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
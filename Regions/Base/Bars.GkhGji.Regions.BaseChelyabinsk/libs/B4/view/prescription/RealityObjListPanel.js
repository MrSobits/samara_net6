Ext.define('B4.view.prescription.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'prescriptionRealityObjListPanel',
    border: false,
    layout: {
        type: 'border'
    },

    alias: 'widget.prescriptionRealObjListPanel',

    requires: [
        'B4.view.prescription.RealityObjViolationGrid',
        'B4.view.prescription.ViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'prescriptionWestPanel',
                    region: 'west',
                    split: true,
                    collapsible: true,
                    border: false,
                    width: 400,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'prescriptionRealObjViolGrid',
                            border: false
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    layout: 'fit',
                    border: false,
                    items: [
                        {
                            xtype: 'prescriptionViolationGrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

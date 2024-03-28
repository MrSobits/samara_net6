Ext.define('B4.view.prescription.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'prescriptionRealityObjListPanel',
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
                    collapsed: false,
                    border: false,
                    width: 400,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'prescriptionRealObjViolGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            padding: '5 5 5 5'
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
                            bodyStyle: 'backrgound-color:transparent;',
                            padding: '5 5 5 5'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

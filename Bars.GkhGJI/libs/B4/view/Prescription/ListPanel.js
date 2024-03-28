Ext.define('B4.view.prescription.ListPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.prescription.Grid',
        'B4.view.prescription.RelationsGrid'
    ],

    closable: true,
    storeName: null,
    title: 'Предписания',
    itemId: 'prescriptionListPanel',
    layout: {
        type: 'border'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                Ext.create('B4.view.prescription.Grid', {
                    split: true,
                    border: false,
                    region: 'west'
                }),
                Ext.create('B4.view.prescription.RelationsGrid', { 
                    region: 'center',
                    border: false
                })
            ]
        });

        me.callParent(arguments);
    }
});
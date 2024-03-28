Ext.define('B4.view.appealcits.EmergencyHouseMainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр проверок в/а домов',
    alias: 'widget.appealcitsEmergencyHouseMainPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.appealcits.EmergencyHouseGrid',
        'B4.view.appealcits.EmergencyHouseFilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsEmergencyHouseFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'emergencyhousegrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
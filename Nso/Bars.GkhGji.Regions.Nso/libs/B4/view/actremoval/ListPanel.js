Ext.define('B4.view.actremoval.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Акты проверок предписаний',
    itemId: 'actRemovalListPanel',
    layout: {
        type: 'border'
    },
    
    requires: [
        'B4.view.actremoval.Grid'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'actRemovalGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

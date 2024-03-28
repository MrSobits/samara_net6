Ext.define('B4.view.actremoval.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Акты проверок предписаний',
    itemId: 'actRemovalListPanel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    
    requires: [
        'B4.view.actremoval.Grid',
        'B4.view.actremoval.RelationsGrid'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'panel',
                layout: 'fit'
            },
            items: [
                {
                    width: 410,
                    items: [
                        {
                            xtype: 'actRemovalGrid',
                            padding: '5 5 5 5'
                        }
                    ]
                },
                {
                    flex: 1,
                    items: [
                        {
                            xtype: 'actRemovalRealtionsGrid',
                            padding: '5 5 5 0'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.protocolgji.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Протоколы',
    itemId: 'protocolgjiListPanel',
    layout: {
        type: 'border'
    },
    
    requires: [
        'B4.view.protocolgji.Grid',
        'B4.view.protocolgji.RelationsGrid'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'panel',
                layout: 'fit',
                border: false
            },
            items: [
                {
                    region: 'west',
                    split: true,
                    collapsible: false,
                    flex: .5,
                    padding: '0 5 0 0',
                    items: [
                        {
                            xtype: 'protocolgjiGrid'
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'protocolgjiRealtionsgrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

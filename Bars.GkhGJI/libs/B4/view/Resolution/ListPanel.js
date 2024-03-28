Ext.define('B4.view.resolution.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Постановления',
    itemId: 'resolutionListPanel',
    layout: {
        type: 'border'
    },
    
    requires: [
        'B4.view.resolution.Grid',
        'B4.view.resolution.RelationsGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'west',
                    split: true,
                    collapsible: false,
                    border: false,
                    flex: .5,
                    layout: 'fit',
                    padding: '0 5 0 0',
                    items: [
                        {
                            xtype: 'resolutionGrid',
                            bodyStyle: 'backrgound-color:transparent;'
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
                            xtype: 'resolutionRelationsGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

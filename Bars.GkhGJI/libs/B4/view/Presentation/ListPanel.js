Ext.define('B4.view.presentation.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Представления',
    itemId: 'presentationListPanel',
    layout: {
        type: 'border'
    },
    
    requires: [
        'B4.view.presentation.Grid',
        'B4.view.presentation.RelationsGrid'
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
                            xtype: 'presentationGrid'
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
                            xtype: 'presentationRelationsGrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.actcheck.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Акты проверок',
    itemId: 'actCheckListPanel',
    layout: {
        type: 'border'
    },
    border: false,
    requires: [
        'B4.view.actcheck.RelationsGrid',
        'B4.view.actcheck.Grid'
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
                    flex: .5,
                    region: 'west',
                    padding: '0 5 0 0',
                    items: [
                        {
                            xtype: 'actCheckGrid',
                            border: false
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'actCheckRelationsGrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

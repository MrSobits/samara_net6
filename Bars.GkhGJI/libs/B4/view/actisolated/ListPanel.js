Ext.define('B4.view.actisolated.ListPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.actisolatedlistpanel',

    closable: true,
    storeName: null,
    title: 'Акты без взаимодействия',

    layout: {
        type: 'border'
    },
    border: false,
    requires: [
        'B4.view.actisolated.RelationsGrid',
        'B4.view.actisolated.Grid'
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
                            xtype: 'actisolatedgrid',
                            border: false
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'actisolatedrelationsgrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

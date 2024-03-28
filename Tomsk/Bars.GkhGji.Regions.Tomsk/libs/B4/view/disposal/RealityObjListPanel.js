Ext.define('B4.view.disposal.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'disposalRealityObjListPanel',
    layout: {
        type: 'border'
    },

    alias: 'widget.disposalRealObjListPanel',

    requires: [
        'B4.view.disposal.RealityObjViolationGrid',
        'B4.view.disposal.ViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'disposalWestPanel',
                    region: 'west',
                    split: true,
                    collapsible: true,
                    collapsed: false,
                    border: false,
                    width: 400,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'disposalRealObjViolGrid',
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
                            xtype: 'disposalViolationGrid',
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

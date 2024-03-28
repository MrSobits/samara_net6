Ext.define('B4.view.protocolgji.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'protocolRealityObjListPanel',
    layout: {
        type: 'border'
    },

    alias: 'widget.protocolgjiRealityObjListPanel',

    requires: [
        'B4.view.protocolgji.ViolationGrid',
        'B4.view.protocolgji.RealityObjViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'west',
                    itemId: 'protocolWestPanel',
                    split: true,
                    collapsible: true,
                    border: false,
                    width: 400,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'protocolgjiRealityObjViolationGrid',
                            border: false
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
                            xtype: 'protocolgjiViolationGrid',
                            border: false,
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
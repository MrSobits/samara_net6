Ext.define('B4.view.motivationconclusion.ListPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.motivationconclusionlistpanel',

    closable: true,
    storeName: null,
    title: 'Мотивировочные заключения',

    layout: {
        type: 'border'
    },
    border: false,
    requires: [
        'B4.view.motivationconclusion.RelationsGrid',
        'B4.view.motivationconclusion.Grid'
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
                            xtype: 'motivationconclusiongrid',
                            border: false
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'motivationconclusionrelationsgrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

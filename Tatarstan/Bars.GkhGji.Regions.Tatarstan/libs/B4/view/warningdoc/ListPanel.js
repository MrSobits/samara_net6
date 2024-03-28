Ext.define('B4.view.warningdoc.ListPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.warningdoclistpanel',

    closable: true,
    storeName: null,
    title: 'Предостережения',

    layout: {
        type: 'border'
    },
    border: false,
    requires: [
        'B4.view.warningdoc.RelationsGrid',
        'B4.view.warningdoc.Grid'
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
                            xtype: 'warningdocgrid',
                            border: false
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'warningdocrelationsgrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

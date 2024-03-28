Ext.define('B4.view.overhaulpropose.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр предложений по капремонту',
    alias: 'widget.overhaulproposemainpanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.overhaulpropose.Grid',
        'B4.view.overhaulpropose.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'overhaulproposefilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'overhaulproposegrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.protocolmhc.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Протокол МЖК',
    alias: 'widget.protocolMhcPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.protocolmhc.Grid',
        'B4.view.protocolmhc.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'protocolMhcFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'protocolMhcGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.protocolmvd.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Протоколы МВД',
    alias: 'widget.protocolMvdPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.protocolmvd.Grid',
        'B4.view.protocolmvd.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'protocolMvdFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'protocolMvdGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

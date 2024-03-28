Ext.define('B4.view.resolpros.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Постановления прокуратуры',
    alias: 'widget.resolProsPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.resolpros.Grid',
        'B4.view.resolpros.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'resolProsFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'resolprosGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.tatarstanprotocolgji.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Протоколы ГЖИ',
    alias: 'widget.tatarstanprotocolgjimainpanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.tatarstanprotocolgji.Grid',
        'B4.view.tatarstanprotocolgji.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tatarstanprotocolgjifilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'tatarstanprotocolgjigrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
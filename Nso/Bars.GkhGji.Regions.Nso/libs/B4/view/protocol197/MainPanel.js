Ext.define('B4.view.protocol197.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Протоколы по ст.19.7 КоАП РФ',
    alias: 'widget.protocol197Panel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.protocol197.Grid',
        'B4.view.protocol197.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'protocol197FilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'protocol197Grid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

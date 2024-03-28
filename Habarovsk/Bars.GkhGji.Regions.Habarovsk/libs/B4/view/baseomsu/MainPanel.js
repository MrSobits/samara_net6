Ext.define('B4.view.baseomsu.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Плановые проверки органов местного самоуправления',
    alias: 'widget.baseOMSUPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.baseomsu.Grid',
        'B4.view.baseomsu.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'baseOMSUFilterPanel',
                    split: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baseOMSUGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});

Ext.define('B4.view.basedisphead.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Проверки по поручению руководителей',
    alias: 'widget.baseDispHeadPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.basedisphead.FilterPanel',
        'B4.view.basedisphead.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'baseDispHeadFiterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baseDispHeadGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
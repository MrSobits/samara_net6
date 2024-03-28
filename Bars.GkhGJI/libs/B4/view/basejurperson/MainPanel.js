Ext.define('B4.view.basejurperson.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Плановые проверки юридических лиц',
    alias: 'widget.baseJurPersonPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.basejurperson.Grid',
        'B4.view.basejurperson.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'baseJurPersonFilterPanel',
                    split: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baseJurPersonGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});

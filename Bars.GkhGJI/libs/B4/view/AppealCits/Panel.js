Ext.define('B4.view.appealcits.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр обращений',
    alias: 'widget.appealCitsPanel',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.appealcits.FilterPanel',
        'B4.view.appealcits.Grid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    frame: true,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'appealCitsGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});

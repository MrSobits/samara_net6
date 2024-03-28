Ext.define('B4.view.appealcits.PanelFond', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр обращений',
    alias: 'widget.appealCitsPanelFond',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.appealcits.FilterPanelFond',
        'B4.view.appealcits.GridFond'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsFilterPanelFond',
                    region: 'north',
                    split: false,
                    border: false,
                    frame: true,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'appealCitsGridFond',
                    region: 'center'
                }
            ]
        });
        me.callParent(arguments);
    }
});

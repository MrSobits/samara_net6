Ext.define('B4.view.edolog.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Данные по интеграции с ЭДО',
    alias: 'widget.appealCitsEdoLogPanel',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.edolog.FilterPanel',
        'B4.view.edolog.Grid'
    ],
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsEdoLogFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'edologGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.edologrequests.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Лог интеграции с ЭДО',
    alias: 'widget.appealCitsEdoLogRequestsPanel',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.edologrequests.FilterPanel',
        'B4.view.edologrequests.Grid'
    ],
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'requestsFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'edologRequestsGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
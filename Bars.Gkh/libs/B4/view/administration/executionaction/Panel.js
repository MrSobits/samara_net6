Ext.define('B4.view.administration.executionaction.Panel',
{
    extend: 'Ext.panel.Panel',
    alias: 'widget.executionactionpanel',

    requires: [
        'B4.view.administration.executionaction.ResultGrid',
        'B4.view.administration.executionaction.TaskGrid'
    ],

    title: 'Выполнение действия',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        Ext.apply(me,
        {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'executionactiontaskgrid'
                        },
                        {
                            xtype: 'executionactionresultgrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
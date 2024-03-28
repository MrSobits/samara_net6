Ext.define('B4.view.inspectionpreventiveaction.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Проверки по профилактическим мероприятиям',
    alias: 'widget.inspectionpreventiveactionmainpanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.inspectionpreventiveaction.Grid',
        'B4.view.inspectionpreventiveaction.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'inspectionpreventiveactionfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'inspectionpreventiveactiongrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
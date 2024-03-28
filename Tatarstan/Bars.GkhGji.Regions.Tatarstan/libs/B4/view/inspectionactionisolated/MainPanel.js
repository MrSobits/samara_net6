Ext.define('B4.view.inspectionactionisolated.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Проверки по КНМ без взаимодействия',
    alias: 'widget.inspectionactionisolatedmainpanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.inspectionactionisolated.Grid',
        'B4.view.inspectionactionisolated.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'inspectionactionisolatedfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'inspectionactionisolatedgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
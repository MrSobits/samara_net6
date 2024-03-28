Ext.define('B4.view.warninginspection.MainPanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.warninginspectionmainpanel',

    closable: true,
    title: 'Реестр предостережений',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.warninginspection.FilterPanel',
        'B4.view.warninginspection.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'warninginspectionfiterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'warninginspectiongrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
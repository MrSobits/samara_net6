Ext.define('B4.view.baselicensereissuance.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.baselicensereissuancepanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.baselicensereissuance.FilterPanel',
        'B4.view.baselicensereissuance.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            title: 'Проверка лицензата',
            items: [
                {
                    xtype: 'baselicensereissuancefilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'baselicensereissuancegrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
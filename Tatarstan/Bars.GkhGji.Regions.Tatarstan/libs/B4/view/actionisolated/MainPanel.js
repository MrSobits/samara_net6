Ext.define('B4.view.actionisolated.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'КНМ без взаимодействия с контролируемыми лицами',
    alias: 'widget.actionisolatedmainpanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.actionisolated.Grid',
        'B4.view.actionisolated.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'actionisolatedfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'actionisolatedgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
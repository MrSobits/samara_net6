Ext.define('B4.view.realityobj.LiftPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.realityobjliftpanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    title: 'Лифты',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: 'none 0px 0px repeat scroll transparent',
    requires: [
        'B4.view.realityobj.LiftSummaryPanel',
        'B4.view.realityobj.LiftGrid'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'roliftsummarypanel',
                    margin: '0 0 10 0'
                },
                
                {
                    xtype: 'realityobjectliftgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});

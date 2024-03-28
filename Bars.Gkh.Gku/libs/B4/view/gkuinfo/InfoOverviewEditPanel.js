Ext.define('B4.view.gkuinfo.InfoOverviewEditPanel', {
    extend: 'Ext.form.Panel',
    
    alias: 'widget.gkuhseinfoovervieweditpanel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Начисления  по дому',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'hseoverallbalancegrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});
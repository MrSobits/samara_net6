Ext.define('B4.view.realityobj.protocol.Panel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.roprotpanel',

    title: 'Протоколы и решения собственников',

    requires: [
        'B4.view.realityobj.protocol.DecisionGrid',
        'B4.view.realityobj.protocol.Grid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'roprotocolgrid'
                },
                {
                    xtype: 'roprotdecisiongrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});
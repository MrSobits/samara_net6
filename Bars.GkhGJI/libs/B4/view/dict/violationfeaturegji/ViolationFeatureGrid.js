Ext.define('B4.view.dict.violationfeaturegji.ViolationFeatureGrid', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.dict.violationfeaturegji.ViolationGroupsTree',
        'B4.view.dict.violationfeaturegji.ViolationsGrid'
    ],

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    alias: 'widget.violationFeatureGjiGrid',
    title: 'Группы нарушений',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'violationgroupstree',
                border: false,
                flex: 2
            },
            {
                xtype: 'violationsgrid',
                style: 'border-left: solid #99bce8 1px',
                border: false,
                disabled: true,
                flex: 3
            }]
        });

        me.callParent(arguments);
    }
});
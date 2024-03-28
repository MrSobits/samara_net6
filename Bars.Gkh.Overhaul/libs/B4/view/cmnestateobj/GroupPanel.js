Ext.define('B4.view.cmnestateobj.GroupPanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.grouppanel',
    
    title: 'Группы конструктивных элементов',
    
    requires: [
        'B4.view.cmnestateobj.group.Grid',
        'B4.view.cmnestateobj.group.Panel'
    ],

    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'structelgroupgrid',
                    width: 300,
                    margin: -1
                },
                {
                    xtype: 'structelgrouppanel',
                    disabled: true,
                    flex: 1,
                    margin: '-1 -1 -1 0'
                }
            ]
        });

        me.callParent(arguments);
    }
});
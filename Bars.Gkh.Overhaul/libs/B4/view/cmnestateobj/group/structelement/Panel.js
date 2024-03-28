Ext.define('B4.view.cmnestateobj.group.structelement.Panel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.structelgroupelementspanel',
    
    title: 'Конструктивные элементы',
    
    requires: [
        'B4.view.cmnestateobj.group.structelement.Grid',
        'B4.view.cmnestateobj.group.structelement.WorkGrid',
        'B4.view.cmnestateobj.group.structelement.FeatureViolGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'groupelementsgrid',
                    flex: 1,
                    margin: -1
                },
                {
                    xtype: 'tabpanel',
                    margin: -1,
                    flex: 1,
                    items: [
                        {
                            xtype: 'groupelementworksgrid',
                            flex: 1,
                            margin: -1,
                            disabled: true
                        },
                        {
                            xtype: 'featureviolgrid',
                            disabled: true
                        }
                    ]
                }
                //{
                //    xtype: 'groupelementworksgrid',
                //    flex: 1,
                //    margin: -1,
                //    disabled: true
                //}
            ]
        });

        me.callParent(arguments);
    }
});
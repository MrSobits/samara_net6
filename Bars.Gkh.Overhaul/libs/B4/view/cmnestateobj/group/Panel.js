Ext.define('B4.view.cmnestateobj.group.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.structelgrouppanel',
    
    layout: 'fit',

    objectId: null,

    requires: [
        'B4.view.cmnestateobj.group.MainInfoPanel',
        'B4.view.cmnestateobj.group.structelement.Panel',
        'B4.view.cmnestateobj.group.formula.Panel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
                items: [
                {
                    xtype: 'tabpanel',
                    margin: -1,
                    items: [
                        {
                            xtype: 'structelgroupmaininfopanel'
                        },
                        {
                            xtype: 'structelgroupelementspanel',
                            disabled: true
                        },
                        {
                            xtype: 'structelgroupformulapanel',
                            disabled: true
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
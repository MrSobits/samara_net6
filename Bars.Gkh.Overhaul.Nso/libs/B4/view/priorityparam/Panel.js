Ext.define('B4.view.priorityparam.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.priorityparampanel',

    requires: [
        'B4.ux.button.Save',
        'B4.view.priorityparam.Grid',
        'B4.view.priorityparam.QualityGrid',
        'B4.view.priorityparam.QuantGrid',
        'B4.view.priorityparam.multi.Grid',
        'B4.view.priorityparam.AdditionPanel'
    ],

    title: 'Параметры очередности',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'border'
            },
            items: [
                {
                    xtype: 'priorityparamgrid',
                    region: 'west',
                    width: 400,
                    minWidth: 400
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        flex: 1,
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'priorityparamadditionpanel',
                            flex: 1,
                            minHeight: 130,
                            hidden: false
                        },
                        {
                            xtype: 'priorityparamqualitygrid',
                            flex: 10,
                            hidden: true
                        },
                        {
                            xtype: 'priorityparamquantgrid',
                            flex: 10,
                            hidden: true
                        },
                        {
                            xtype: 'priorityparammultigrid',
                            flex: 10,
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
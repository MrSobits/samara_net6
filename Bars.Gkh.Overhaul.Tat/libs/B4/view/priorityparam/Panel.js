Ext.define('B4.view.priorityparam.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.priorityparampanel',

    requires: [
        'B4.ux.button.Save',
        'B4.view.priorityparam.Grid',
        'B4.view.priorityparam.QualityGrid',
        'B4.view.priorityparam.QuantGrid',
        'B4.view.priorityparam.multi.Grid'
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
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'priorityparamqualitygrid',
                            flex: 1,
                            hidden: true
                        },
                        {
                            xtype: 'priorityparamquantgrid',
                            flex: 1,
                            hidden: true
                        },
                        {
                            xtype: 'priorityparammultigrid',
                            flex: 1,
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.costlimit.Panel',
    {
        extend: 'Ext.panel.Panel',
        alias: 'widget.costlimitPanel',
        requires: [
            'B4.view.costlimit.Grid',
        ],
        title: 'Предельная стоимость услуг или работ',
        bodyStyle: Gkh.bodyStyle,
        closable: true,
        autoScroll: true,
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        initComponent: function () {
            var me = this;
            Ext.apply(me,
                {
                    items: [
                        {
                            xtype: 'tabpanel',
                            flex: 1,
                            items: [
                                {
                                    xtype: 'costlimitgrid'
                                },
                            ]
                        }
                    ]
                });
            me.callParent(arguments);
        }
    });
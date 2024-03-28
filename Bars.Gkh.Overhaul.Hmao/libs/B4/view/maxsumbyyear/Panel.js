Ext.define('B4.view.maxsumbyyear.Panel',
    {
        extend: 'Ext.panel.Panel',
        alias: 'widget.maxsumbyyearpanel',

        requires: [
            'B4.view.maxsumbyyear.Grid',
        ],

        title: 'Предельные стоимости в разрезе МО',

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
                                    xtype: 'maxsumbyyeargrid'
                                },
                            ]
                        }
                    ]
                });

            me.callParent(arguments);
        }
    });
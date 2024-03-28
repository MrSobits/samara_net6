Ext.define('B4.view.subprogram.Panel',
    {
        extend: 'Ext.panel.Panel',
        alias: 'widget.subprogrampanel',

        requires: [
            'B4.view.subprogram.Grid',
        ],

        title: 'Критерии попадания в подпрограмму',

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
                                    xtype: 'subprogramgrid'
                                },
                            ]
                        }
                    ]
                });

            me.callParent(arguments);
        }
    });
Ext.define('B4.view.actualisedpkr.Panel',
    {
        extend: 'Ext.panel.Panel',
        alias: 'widget.actualisedpkrpanel',

        requires: [
            'B4.view.actualisedpkr.Grid',
        ],

        title: 'Параметры актуализации ДПКР',

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
                                    xtype: 'actualisedpkrgrid'
                                },
                            ]
                        }
                    ]
                });

            me.callParent(arguments);
        }
    });
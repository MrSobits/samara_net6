Ext.define('B4.view.costlimitooi.Panel',
    {
        extend: 'Ext.panel.Panel',
        alias: 'widget.costlimitpanelooi',
        requires: [
            'B4.view.costlimitooi.Grid',
        ],
        title: 'Предельная стоимость услуг или работ в разрезе ООИ',
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
                                    xtype: 'costlimitgridooi'
                                },
                            ]
                        }
                    ]
                });
            me.callParent(arguments);
        }
    });
Ext.define('B4.view.reportHistory.ParamWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.reporthistoryparamwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.grid.Panel'
    ],

    title: 'Параметры отчета',
    width: 800,
    height: 600,
    bodyPadding: 5,
    autoScroll: true,
    modal: true,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.ReportHistoryParam');

        Ext.apply(me, {
            items: [
                {
                    xtype: 'b4grid',
                    name: 'paramGrid',
                    minHeight: 300,
                    store: store,
                    flex: 1,
                    columns: [
                        {
                            header: 'Наименование параметра',
                            flex: 1,
                            dataIndex: 'DisplayName'
                        },
                        {
                            header: 'Значение',
                            flex: 3,
                            dataIndex: 'DisplayValue'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть',
                                    listeners: {
                                        'click': function (btn) {
                                            btn.up('reporthistoryparamwindow').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
})
Ext.define('B4.view.regop.loan.ResultWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.loanresultwindow',

    requires: [
        'B4.ux.button.Close'
    ],

    modal: true,
    closeAction: 'destroy',
    bodyPadding: 10,
    width: 800,
    minHeight: 350,
    maxHeight: 550,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Результат возврата займов',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Taker' },
                    { name: 'Giver' },
                    { name: 'ReturnedSum' },
                    { name: 'Message' }
                ],
                remoteSort: false,
                remoteFilter: false,
                autoLoad: false
            });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4grid',
                    store: store,
                    flex: 1,
                    columns: [
                        {
                            dataIndex: 'Taker',
                            header: 'Заниматель',
                            flex: 1
                        },
                        {
                            dataIndex: 'ReturnedSum',
                            header: 'Возвращенная сумма',
                            width: 140
                        },
                        {
                            dataIndex: 'Message',
                            header: 'Сообщение',
                            flex: 1,
                            renderer: function(val) {
                                return val ? val : 'Недостаточно средств для возврата займа';
                            }
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
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function() {
                                    me.close();
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});
Ext.define('B4.view.regop.personal_account.PersonalAccountPerfWorkWindow',
{
    extend: 'Ext.window.Window',

    alias: 'widget.paoperatioperfworknwin',

    requires: [
        'B4.ux.button.Close',
        'B4.base.Store',
        'B4.enums.regop.WalletType'
    ],

    modal: true,
    width: 750,
    height: 400,
    title: 'Детализация',
    layout: 'fit',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store',
            {
                fields: [
                    { name: 'PeriodName' },
                    { name: 'TypeOperation' },
                    { name: 'DistributionType' },
                    { name: 'Sum' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasePersonalAccount',
                    listAction: 'ListDistributedPerformedWork'
                },
                autoLoad: false
            });

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'gridpanel',
                    border: false,
                    store: store,
                    columnLines: true,
                    enableColumnHide: false,
                    columns: [
                        {
                            dataIndex: 'PeriodName',
                            text: 'Период',
                            flex: .7,
                            sortable: false
                        },
                        {
                            text: 'Название операции',
                            dataIndex: 'TypeOperation',
                            flex: 1.8,
                            sortable: false
                        },
                        {
                            text: 'Тип операции',
                            dataIndex: 'DistributionType',
                            flex: .8,
                            sortable: false,
                            renderer: function (val) {
                                return B4.enums.regop.WalletType.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'numbercolumn',
                            text: 'Сумма операции',
                            dataIndex: 'Sum',
                            format: '0.00',
                            flex: .7,
                            sortable: false
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
                                click: function(btn) {
                                    btn.up('window').close();
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
Ext.define('B4.view.bankaccountstatement.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.BankAccountStatement'
    ],

    title: 'Реестр банковских выписок',
    alias: 'widget.bankaccstatementgrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.BankAccountStatement'),
            yesNoRenderer = function (val) {
                return val ? 'Да' : 'Нет';
            };

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    header: 'Номер документа',
                    dataIndex: 'DocumentNum',
                    filter:{xtype: 'textfield'}
                },
                {
                    header: 'Р/с плательщик',
                    dataIndex: 'PayerAccountNum',
                    filter:{xtype: 'textfield'}
                },
                {
                    header: 'Р/с получателя',
                    dataIndex: 'RecipientAccountNum',
                    filter: { xtype: 'textfield' }
                },
                {
                    header: 'Назначение платежа',
                    dataIndex: 'PaymentDetails',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function(b) {
                                        b.up('grid').getStore().load();
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
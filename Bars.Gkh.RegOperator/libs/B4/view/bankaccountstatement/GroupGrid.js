Ext.define('B4.view.bankaccountstatement.GroupGrid', {
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
    alias: 'widget.bankaccstatementgroupgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.BankAccountStatementGroup');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
               {
                    xtype: 'b4editcolumn',
                    scope: me
               },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ImportDate',
                    text: 'Дата операции',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserLogin',
                    text: 'Пользователь',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
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
                                    xtype: 'b4updatebutton'
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
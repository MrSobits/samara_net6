Ext.define('B4.view.calcaccount.CreditGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.calcaccount.Credit'
    ],

    title: 'Реестр кредитов',
    alias: 'widget.calcaccountcreditgrid',
    closable: true,
    cls:'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.calcaccount.Credit'),
            decimalFilter = {
                xtype: 'numberfield',
                operand: CondExpr.operands.eq,
                decimalSeparator: ','
            },
            dateFilter = {
                xtype: 'datefield',
                operand: CondExpr.operands.eq,
                format: 'd.m.Y'
            };

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата формирования',
                    filter: dateFilter
                },
                {
                    dataIndex: 'Account',
                    flex: 1,
                    text: 'Р/с кредитуемого',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'AccountOwner',
                    flex: 1,
                    text: 'Владелец счета',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'CreditSum',
                    width: 100,
                    text: 'Сумма кредита',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'PercentSum',
                    width: 100,
                    text: 'Сумма процентов',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'PercentRate',
                    width: 100,
                    text: 'Процентная ставка',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'CreditPeriod',
                    width: 100,
                    text: 'Срок (кол-во месяцев)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: false
                    }
                },
                {
                    dataIndex: 'CreditDebt',
                    width: 100,
                    text: 'Сумма основного долга',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'PercentDebt',
                    width: 100,
                    text: 'Сумма долга по процентам',
                    filter: decimalFilter
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Фактическая дата погашения',
                    filter: dateFilter
                },
                {
                    dataIndex: 'Document',
                    width: 100,
                    text: 'Документ',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                                    xtype: 'b4addbutton'
                                },
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
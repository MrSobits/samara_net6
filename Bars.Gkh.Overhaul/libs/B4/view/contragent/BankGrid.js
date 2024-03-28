Ext.define('B4.view.contragent.BankGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Расчетные счета',
    store: 'contragent.Bank',
    alias: 'widget.contragentBankGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Кредитная организация'
                },
                {
                    dataIndex: 'Bik',
                    width: 100,
                    text: 'БИК'
                },
                {
                    dataIndex: 'Okonh',
                    flex: 1,
                    text: 'ОКОНХ'
                },
                {
                    dataIndex: 'Okpo',
                    flex: 1,
                    text: 'ОКПО'
                },
                {
                    dataIndex: 'CorrAccount',
                    flex: 1,
                    text: 'Корр. счет'
                },
                {
                    dataIndex: 'SettlementAccount',
                    flex: 1,
                    text: 'Расч. счет'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
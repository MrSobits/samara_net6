Ext.define('B4.view.claimwork.partialclaimwork.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNoNotSet'
    ],

    title: 'Долевые ПИР',
    store: 'claimwork.PartialClaimWork',
    alias: 'widget.partialclaimworkgrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.PartialClaimWork');
        Ext.applyIf(me, {
            columnLines: true,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 1,
                    text: 'Id',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Num',
                    text: 'Номер ЗВСП',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtorFullname',
                    text: 'Должник',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtorRoomAddress',
                    text: 'Адрес МКД',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtorRoomNumber',
                    text: 'Номер пом.',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtorDebtPeriod',
                    text: 'Период задолженности',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtorDebtAmount',
                    text: 'Сумма долга',
                    flex: 1,
                    filter: { xtype: 'numberfield' }
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
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'button',
                            action: 'MoveToArchive',
                            icon: B4.Url.content('content/img/icons/book_go.png'),
                            text: 'Перевести в архив ПИР'
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
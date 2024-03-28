Ext.define('B4.view.regop.personal_account.action.DistributeFundsForPerformedWorkGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.distributefundsforperformedworkgrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.MemoryHeaderFilters',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store',
        'Ext.ux.data.PagingMemoryProxy'
    ],

    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                proxy: {
                    type: 'pagingmemory',
                    enablePaging: true
                },
                fields: [
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'Address' },
                    { name: 'PersonalAccountNum' },
                    { name: 'Area' },
                    { name: 'DistributionSum' }
                ]
            });
        Ext.applyIf(me, {
            store: store,
            emptyText: 'После изменения распределяемой суммы необходимо выполнить распределение',
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    text: 'Лицевой счет',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    text: 'Площадь',
                    flex: 1,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DistributionSum',
                    text: 'Сумма',
                    flex: 1,
                    decimalSeparator: ',',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    },
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.00
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.MemoryHeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEdit'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
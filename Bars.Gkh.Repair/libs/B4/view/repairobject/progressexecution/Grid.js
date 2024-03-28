Ext.define('B4.view.repairobject.progressexecution.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Ход выполнения работ',
    store: 'repairobject.ProgressExecutionWork',
    itemId: 'progressExecutionRepairWorkGrid',
    closable: true,

    initComponent: function () {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 2,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед. изм.'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'VolumeOfCompletion',
                    text: 'Объем выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PercentOfCompletion',
                    text: 'Процент выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CostSum',
                    text: 'Сумма расходов',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    text: 'Плановый объем',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Плановая сумма',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
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
                            columns: 3,
                            items: [
                                { xtype: 'b4updatebutton' }
                            ]
                        },
                        {
                            xtype: 'label',
                            text: 'Для редактирования записи нажмите на "карандаш", либо щелкните по записи два раза',
                            padding: "5 0 0 10"
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
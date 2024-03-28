Ext.define('B4.view.specialobjectcr.ProgressExecutionWorkGrid', {
    extend: 'B4.ux.grid.Panel',
    alias:'widget.specialobjectcrprogressexecutionworkgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Ход выполнения работ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.specialobjectcr.ProgressExecutionWork');
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'WorkName',
                    flex: 2,
                    text: 'Вид работы'
                },
                {
                    dataIndex: 'FinanceSourceName',
                    flex: 2,
                    text: 'Разрез финансирования'
                },
                {
                    dataIndex: 'UnitMeasureName',
                    flex: 1,
                    text: 'Ед. изм.'
                },
                {
                    dataIndex: 'VolumeOfCompletion',
                    text: 'Объем выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'PercentOfCompletion',
                    text: 'Процент выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'CostSum',
                    text: 'Сумма расходов',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'Volume',
                    text: 'Плановый объем',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'Sum',
                    text: 'Плановая сумма',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'ManufacturerName',
                    text: 'Производитель',
                    flex: 1
                },
                {
                    dataIndex: 'StageWorkCrName',
                    text: 'Этап работы',
                    flex: 1
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
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    name: 'CalcPercentOfCompletion',
                                    iconCls: 'icon-accept',
                                    text: 'Рассчитать процент выполнения'
                                }
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
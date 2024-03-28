Ext.define('B4.view.dict.resettlementprogram.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.StateResettlementProgram',
        'B4.enums.TypeResettlementProgram',
        'B4.enums.VisibilityResettlementProgram'
    ],

    title: 'Программы переселения',
    store: 'dict.ResettlementProgram',
    alias: 'widget.resettlementProgramGrid',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodName',
                    flex: 1,
                    text: 'Период'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StateProgram',
                    flex: 1,
                    text: 'Состояние',
                    renderer: function (val) { return B4.enums.StateResettlementProgram.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeProgram',
                    flex: 1,
                    text: 'Тип',
                    renderer: function (val) { return B4.enums.TypeResettlementProgram.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Visibility',
                    flex: 1,
                    text: 'Видимость',
                    renderer: function (val) { return B4.enums.VisibilityResettlementProgram.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UseInExport',
                    flex: 1,
                    text: 'Используется при экспорте',
                    renderer: function (val) {
                        var r = 'Да';
                        if ((val == 'false') || (val == 0)) r = 'Нет';
                        return r;
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
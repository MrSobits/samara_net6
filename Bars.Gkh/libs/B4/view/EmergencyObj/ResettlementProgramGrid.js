Ext.define('B4.view.emergencyobj.ResettlementProgramGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Разрезы финансирования по программе',
    store: 'emergencyobj.ResettlementProgram',
    itemId: 'emergencyObjResettlementProgramGrid',
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
                    dataIndex: 'SourceName',
                    flex: 1,
                    text: 'Источник по программе'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountResidents',
                    flex: 1,
                    text: 'Количество жителей'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    flex: .5,
                    text: 'Площадь'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    flex: .5,
                    text: 'Плановая стоимость'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActualCost',
                    flex: .5,
                    text: 'Фактическая стоимость'
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
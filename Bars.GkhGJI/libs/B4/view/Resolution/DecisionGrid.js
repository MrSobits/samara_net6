Ext.define('B4.view.resolution.DecisionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.resolutionDecisionGrid',

    requires: [
        'B4.Url',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeDecisionAnswer',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Решения',
    store: 'resolution.Decision',

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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeDecisionAnswer',
                    dataIndex: 'TypeDecisionAnswer',
                    text: 'Тип решение',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AppealDate',
                    flex: 1,
                    text: 'Дата жалобы',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealNumber',
                    flex: 1,
                    text: 'Номер жалобы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Apellant',
                    flex: 1,
                    text: 'Заявитель'
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